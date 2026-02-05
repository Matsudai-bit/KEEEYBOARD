using UnityEngine;
using DG.Tweening;

public class OutLineAnimation : MonoBehaviour
{
    [SerializeField]
    private float minAlpha;
    [SerializeField]
    private float fadeDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<UnityEngine.UI.Image>().DOFade(minAlpha, fadeDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetLink(gameObject); // オブジェクト破棄対策
    }

}
