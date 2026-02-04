using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class BoardsController : MonoBehaviour
{
    [SerializeField]
    ContentsController contentsController;

    private RectTransform rectTransform;
    private Vector2 startAnchoredPosition = new Vector2(999, 999);

    private void Awake()
    {
        // 1. RectTransformコンポーネントを取得してキャッシュ（負荷対策）[3]
        rectTransform = GetComponent<RectTransform>();

        // 2. 初期位置をAnchoredPosition（Canvas上の座標）として記録
        startAnchoredPosition = rectTransform.anchoredPosition;
    }

    public void SlideBoard(StageSelectManager.StageGrade currentGrade)
    {

        float targetPosX = startAnchoredPosition.x + (-1920.0f * (int)currentGrade);

        contentsController.HideContents();

        rectTransform.DOAnchorPosX(targetPosX, 0.5f)
           .SetEase(Ease.InOutSine)
           .OnComplete(() =>
           {
               contentsController.ViewContents(currentGrade);
           })
           .SetLink(gameObject);

    }
}