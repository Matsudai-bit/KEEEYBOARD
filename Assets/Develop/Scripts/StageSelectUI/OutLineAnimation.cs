using UnityEngine;
using DG.Tweening;

public class OutLineAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<UnityEngine.UI.Image>().DOFade(0.0f, 1.0f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetLink(gameObject); // オブジェクト破棄対策
    }

}
