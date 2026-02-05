using DG.Tweening;
using UnityEngine;

public class ArrowUI : MonoBehaviour
{
 
    [SerializeField]
    private float length;
    [SerializeField]
    private float duration;

    [SerializeField]
    bool isRight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<UnityEngine.UI.Image>().transform.DOBlendableMoveBy(new((length * ((isRight)?1.0f:-1.0f)), 0.0f), duration)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine)
        .SetLink(gameObject); // オブジェクト破棄対策
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
