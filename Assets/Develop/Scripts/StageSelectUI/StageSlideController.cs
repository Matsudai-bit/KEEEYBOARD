using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageSlideController : MonoBehaviour
{
    [Header("Source Image")]
    [SerializeField]
    private List<Sprite> SlideImages = new();
    [Header("ステージ識別番号テキスト")]
    [SerializeField]
    private GameObject stageIDText;
    [Header("クリア時間テキスト")]
    [SerializeField]
    private TextMeshProUGUI m_clearTimeText;
    [Header("コンプリート画像")]
    [SerializeField]
    private GameObject m_completeUI;

    private Vector2 slideTargetPos;
    private Tween slideTween;
    private Tween completeUITween;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        slideTargetPos = GetComponent<RectTransform>().anchoredPosition;

        RectTransform slideRect = GetComponent<RectTransform>();
        slideTween = slideRect.DOAnchorPosX(slideTargetPos.x, 0.5f)
           .SetEase(Ease.OutBack, 0.75f)
           .SetAutoKill(false)
           .Pause();

        completeUITween = m_completeUI.transform.DOScale(0.3f, 1.0f).SetEase(Ease.InSine).Pause();

        slideRect.anchoredPosition = new Vector2(slideTargetPos.x + 500.0f, slideTargetPos.y);

    }


    public void SlideIn(StageSelectManager.StageGrade grade, StageSelectManager.StageNumber number, bool completed, bool completeAnimation = false)
    {
        m_clearTimeText.gameObject.SetActive(false);
        m_completeUI.SetActive(false);

        if (!completeAnimation)
        {
            SetClearTime(grade, number);
            if (completed)
            {
                ShowComplete(completeAnimation, grade, number);

            }
        }

        // 画像の設定
        SetSlideImage(grade);

        // ステージIDをテキストにセット
        SetStageIDToText(grade, number);

        // スライドインの初期位置にセット
        RectTransform slideRect = GetComponent<RectTransform>();
        slideRect.anchoredPosition = new Vector2(slideTargetPos.x + 500.0f, slideTargetPos.y);

        // スライドイン開始
        slideTween.Restart();
        slideTween.OnComplete(() => {
            if (completed)
            {
                
                ShowComplete(completeAnimation, grade, number);
            }
        });
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

    private void ShowComplete(bool completeAnimation, StageSelectManager.StageGrade grade, StageSelectManager.StageNumber number)
    {
        m_completeUI.SetActive(true);
        if (completeAnimation)
        {
            m_completeUI.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
            completeUITween.Restart();
            completeUITween.OnComplete(()=> 
            {
                m_clearTimeText.color = new(m_clearTimeText.color.r, m_clearTimeText.color.g, m_clearTimeText.color.b, 0.0f);
                m_clearTimeText.DOFade(1.0f, 0.5f);
                SetClearTime(grade, number); 
            });
        }
    }

    private void SetSlideImage(StageSelectManager.StageGrade grade)
    {
        // インデックス外のアクセス防止
        if (SlideImages.Count == 0)
        {
            Debug.LogError("SlideImagesリストが空です。");
            return;
        }

        Image targetSlide = GetComponentInChildren<Image>();
        targetSlide.sprite = SlideImages[(int)grade];
    }

    /// <summary>
    /// クリア時間の設定
    /// </summary>
    /// <param name="grade"></param>
    /// <param name="number"></param>
    private void SetClearTime(StageSelectManager.StageGrade grade, StageSelectManager.StageNumber number)
    {
        m_clearTimeText.gameObject.SetActive(true);
        GameStage.GradeID gradeID;
        GameStage.StageID stageID;
        StageSelectManager.ConvertGradeIDAndStageID(grade, number, out gradeID, out stageID);

        float clearTime = GameContext.GetInstance.GetSaveData().GetStageStatus(gradeID, stageID).clearTime;
        int minute = (int)clearTime / 60;
        int seconds= (int)clearTime % 60;

        m_clearTimeText.text =  minute.ToString("D2") + " : " + seconds.ToString("D2");
    }

    public bool IsAnimation()
    {
        return slideTween.IsPlaying() || completeUITween.IsPlaying();
    }

}
