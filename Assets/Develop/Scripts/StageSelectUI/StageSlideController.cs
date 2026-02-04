using DG.Tweening;
using TMPro;
using UnityEngine;

public class StageSlideController : MonoBehaviour
{
    [SerializeField]
    private GameObject stageIDText;

    private Vector2 slideTargetPos;
    private Tween slideTween;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slideTargetPos = GetComponent<RectTransform>().anchoredPosition;

        RectTransform slideRect = GetComponent<RectTransform>();
        slideTween = slideRect.DOAnchorPosX(slideTargetPos.x, 0.5f)
           .SetEase(Ease.OutBack, 0.75f)
           .SetAutoKill(false)
           .Pause();

        slideRect.anchoredPosition = new Vector2(slideTargetPos.x + 500.0f, slideTargetPos.y);

    }


    public void SlideIn(StageSelectManager.StageGrade grade, StageSelectManager.StageNumber number)
    {
        // ステージIDをテキストにセット
        SetStageIDToText(grade, number);

        // スライドインの初期位置にセット
        RectTransform slideRect = GetComponent<RectTransform>();
        slideRect.anchoredPosition = new Vector2(slideTargetPos.x + 500.0f, slideTargetPos.y);

        // スライドイン開始
        slideTween.Restart();
    }

    public void SlideOut()
    {
        // スライドアウトの目標位置にセット
        RectTransform slideRect = GetComponent<RectTransform>();
        slideRect.anchoredPosition = slideTargetPos;

        // スライドアウト開始
        slideRect.DOAnchorPosX(slideTargetPos.x + 500.0f, 0.5f)
           .SetEase(Ease.OutBounce)
           .SetLink(gameObject);
    }

    private void SetStageIDToText(StageSelectManager.StageGrade grade, StageSelectManager.StageNumber number)
    {
        // テキストコンポーネントの取得
        TextMeshProUGUI stageIDTextComponent = stageIDText.GetComponent<TextMeshProUGUI>();

        // ステージIDテキストの生成
        int stageGrade = (int)grade + 1;
        int stageNumber = (int)number + 1;
        string fullText = stageGrade.ToString() + "-" + stageNumber.ToString();

        // テキストの設定
        stageIDTextComponent.text = fullText;

    }

}
