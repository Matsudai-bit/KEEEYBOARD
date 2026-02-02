using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpriteMultiLineTimer : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Sprite[] numberSprites; // 0〜9の画像を登録

    [Header("UI References")]
    [SerializeField] private Image min10;
    [SerializeField] private Image min01;
    [SerializeField] private Image sec10;
    [SerializeField] private Image sec01;

    [Header("Settings")]
    [SerializeField] private float duration = 65.0f; // タイマー時間

    [Header("Sound Clip")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource limitedAudioSource;

    // 現在表示しているスプライトのインデックスをキャッシュ（負荷軽減用）
    private int[] currentIndices = new int[6] { -1, -1, -1, -1, -1, -1 };
    private Image[] targetImages;
    private Tween timerTween;
    private int previousSecond = -1;

    void Awake()
    {
        // 配列にまとめてアクセスしやすくする
        targetImages = new Image[] { min10, min01, sec10, sec01 };
    }

    void Start()
    {
        StartTimer(duration);
    }

    public void StartTimer(float time)
    {
        timerTween?.Kill();
        float timerValue = time;
        previousSecond = Mathf.CeilToInt(time);

        // 数値を変動させるTween
        timerTween = DOTween.To(
            () => timerValue,
            x => UpdateDisplay(x),
            0f,
            time
        )
        .SetEase(Ease.Linear)
        .SetLink(gameObject); // オブジェクト破棄対策[2]
    }

    public void RestartTimer(float time)
    {
        StartTimer(time);
        limitedAudioSource.Stop();
    }

    // 一秒ごとに呼ばれる更新処理
    private void UpratePerSeconds(int time)
    {
        // オーディオを再生
        audioSource.PlayOneShot(audioSource.clip);

        // 残り30秒を切るともう一音鳴らす
        if (time == 30)
        {
            limitedAudioSource.Play();
        }

    }

    // 毎フレーム呼ばれる更新処理
    private void UpdateDisplay(float time)
    {
        // Rキーが押されたらリスタート
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartTimer(duration);
            return;
        }

        int currentSecond = Mathf.CeilToInt(time);
        if (currentSecond != previousSecond && previousSecond > 0)
        {
            UpratePerSeconds(currentSecond);
            previousSecond = currentSecond;
        }


        // 1. 各桁の数値を計算
        int m = Mathf.FloorToInt(time / 60f);
        int s = Mathf.FloorToInt(time % 60f);
        int ms = Mathf.FloorToInt((time * 100f) % 100f);

        // 2. 表示すべき数字（0-9）を配列化
        int[] digits = new int[4];
        digits[0] = m / 10; // 分の10の位
        digits[1] = m % 10; // 分の1の位
        digits[2] = s / 10; // 秒の10の位
        digits[3] = s % 10; // 秒の1の位
        //digits[4] = ms / 10; // ミリ秒の10の位
        //digits[5] = ms % 10; // ミリ秒の1の位

        // 3. 画像の適用（変更があった箇所のみ）
        for (int i = 0; i < 4; i++)
        {
            // インデックスが範囲内かチェック
            int digit = Mathf.Clamp(digits[i], 0, numberSprites.Length - 1);

            // キャッシュと比較して、変更がある場合のみSpriteを差し替える
            if (currentIndices[i] != digit)
            {
                targetImages[i].sprite = numberSprites[digit];
                currentIndices[i] = digit;
            }
        }

    }
}