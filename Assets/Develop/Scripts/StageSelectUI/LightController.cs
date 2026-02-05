using UnityEngine;
using DG.Tweening;
using UnityEngine.UI; // DOTweenの名前空間

public class LightController : MonoBehaviour
{
    [SerializeField]
    private float baseIntensity = 0.4f; // 基本の明るさ
    [SerializeField]
    private float minIntensity = 0.0f; // 最小の明るさ

    [SerializeField]
    private Image target;

    private Sequence flickerSequence;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StopFlicker(); // 初期状態は停止
    }

    // ■ 再生（スイッチON）
    public void PlayFlicker()
    {
        // 念のため既存のものを消してから開始
        StopFlicker();

        // 点灯時のアニメーション開始
        Sequence lightOnFlick = DOTween.Sequence();
        lightOnFlick.Append(target.DOFade(baseIntensity, 0.02f));
        lightOnFlick.AppendInterval(0.1f);
        lightOnFlick.Append(target.DOFade(0.0f, 0.02f));
        lightOnFlick.AppendInterval(0.1f);
        lightOnFlick.Append(target.DOFade(baseIntensity, 0.02f));
        lightOnFlick.AppendInterval(0.1f);
        lightOnFlick.Append(target.DOFade(0.0f, 0.02f));
        lightOnFlick.AppendInterval(0.5f);
        lightOnFlick.Play();
        lightOnFlick.OnComplete(() =>
        {
            // 点灯アニメーション終了後に不規則点滅を開始
            StartFlicker();
        });

    }

    // ■ 停止（スイッチOFF）
    public void StopFlicker()
    {
        // 1. Sequenceを完全に破棄する
        // ?. はnullチェック。flickerSequenceが存在する場合のみKillを実行
        flickerSequence?.Kill();

        // 2. 参照をnullに戻しておく（二重Kill防止などのため）
        flickerSequence = null;

        // 3. 【重要】見た目をリセットする
        // アニメーションが止まった瞬間が「消灯状態」とは限らないため、
        // 明示的に明るさを指定する必要があります。
        if (target != null)
        {
            target.DOFade(0.0f, 0.05f); // 消灯状態にする
        }
    }

    private void StartFlicker()
    {
        // 既存のTweenがあれば破棄
        flickerSequence?.Kill();

        // 不規則な点滅パターンを作成
        flickerSequence = DOTween.Sequence()
            .SetLink(gameObject); // オブジェクト破棄時に安全に停止[3]

        // --- パターン構築 ---
        // 1. 一瞬消える (バッ)
        flickerSequence.Append(target.DOFade(minIntensity, 0.05f).SetEase(Ease.OutQuad));
        // 2. すぐ戻る (ッ)
        flickerSequence.Append(target.DOFade(baseIntensity, 0.05f).SetEase(Ease.OutQuad));

        // 3. 少し待機 (ジー...)
        flickerSequence.AppendInterval(Random.Range(0.1f, 0.8f));

        // 4. 連続して明滅 (チカチカッ)
        flickerSequence.Append(target.DOFade(minIntensity, 0.02f));
        flickerSequence.Append(target.DOFade(baseIntensity, 0.02f));
        flickerSequence.Append(target.DOFade(minIntensity, 0.02f));
        flickerSequence.Append(target.DOFade(baseIntensity, 0.02f));

        // 5. 安定して点灯 (長めの待機)
        flickerSequence.AppendInterval(Random.Range(1.0f, 3.0f));

        // 6. ループ設定
        // SetLoops(-1)で無限ループさせますが、
        // 完全にランダムにするため、ループ完了時(OnStepComplete)に再構築する手法をとります
        flickerSequence.OnComplete(() => {
            StartFlicker(); // 自分自身を呼び出してパターンを再抽選（疑似ランダムループ）
        });
    }
}
