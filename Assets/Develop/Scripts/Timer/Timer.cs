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

    [Header("Fade Control")]
    [SerializeField] private CanvasGroup timerCanvasGroup;

    // 現在表示しているスプライトのインデックスをキャッシュ（負荷軽減用）
    private int[] currentIndices = new int[6] { -1, -1, -1, -1, -1, -1 };
    private Image[] targetImages;
    private Tween timerTween;
    private int previousSecond = -1;

    private Tween fadeTween; // 生成したTweenを保持する変数
    private int warningAudioSourceID = -1;

    private float currentTime = 10.0f;

    void Awake()
    {
        // 配列にまとめてアクセスしやすくする
        targetImages = new Image[] { min10, min01, sec10, sec01 };
    }

    void Start()
    {

        fadeTween = timerCanvasGroup.DOFade(0.165f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetLink(gameObject) // オブジェクト破棄対策
            .Pause(); // 最初は停止しておく
    }

    public void StartTimer()
    {
        timerTween?.Kill();
        float timerValue = duration;
        previousSecond = Mathf.CeilToInt(timerValue);

        // 数値を変動させるTween
        timerTween = DOTween.To(
            () => timerValue,
            x => UpdateDisplay(x),
            0f,
            timerValue
        )
        .SetEase(Ease.Linear)
        .SetLink(gameObject); // オブジェクト破棄対策[2]
    }

    public void RestartTimer()
    {
        // リスタート時にフェードをリセットする処理を追加
        if (fadeTween != null)
        {
            fadeTween.Kill(); // アニメーション停止
        }
        if (timerCanvasGroup != null)
        {
            timerCanvasGroup.alpha = 1.0f; // 透明度を元に戻す
        }

        StartTimer();
        SoundManager.GetInstance.RequestStopping(warningAudioSourceID);
    }

    // タイマー時間の設定
    public void SetDuration(float time)
    {
        duration = time;
    }

    // タイマー時間の取得
    public float GetDuration()
    {
        return duration;
    }

    // 一秒ごとに呼ばれる更新処理
    private void UpratePerSeconds(int time)
    {
        // オーディオを再生
        SoundManager.GetInstance.RequestPlaying(SoundID.SE_INGAME_TIMER_TICK);

        // 残り30秒を切るともう一音鳴らす
        if (time == 30)
        {
            warningAudioSourceID = SoundManager.GetInstance.RequestPlaying(SoundID.SE_INGAME_TIMEER_WARNING);

            if (timerCanvasGroup != null)
            {
                Debug.Log("Start Fade Animation");
                fadeTween?.Restart();
            }
        }

    }

    // 毎フレーム呼ばれる更新処理
    private void UpdateDisplay(float time)
    {
        currentTime = time;
        //// Rキーが押されたらリスタート
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    RestartTimer();
        //    return;
        //}

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

    public float GetTime()
    {
        return currentTime;
    }

}