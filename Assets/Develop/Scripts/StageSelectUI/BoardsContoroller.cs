using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class BoardsController : MonoBehaviour
{
    [SerializeField]
    ContentsController contentsController;

    [SerializeField]
    private List<Sprite> BGImages = new();

    [SerializeField]
    private int m_gradeIndex;

    [SerializeField]
    private GameObject m_lockPanel;

    private RectTransform rectTransform;
    private Vector2 startAnchoredPosition = new Vector2(999, 999);

    private void Awake()
    {
        // 1. RectTransformコンポーネントを取得してキャッシュ（負荷対策）[3]
        rectTransform = GetComponent<RectTransform>();

        // 2. 初期位置をAnchoredPosition（Canvas上の座標）として記録
        startAnchoredPosition = rectTransform.anchoredPosition;
 
     
    }

    void Start()
    {
        // インデックス外のアクセス防止
        if ( BGImages.Count == 0)
        {
            Debug.LogError("BGImagesリストが空です。");
            return;
        }

        GetComponent<Image>().sprite = BGImages[m_gradeIndex];

        var grades = (GameStage.GradeID[])Enum.GetValues(typeof(GameStage.GradeID));

        bool isLocked = GameContext.GetInstance.GetSaveData().gradeDataDict[grades[m_gradeIndex].ToString()].isLocked;
        m_lockPanel.SetActive(isLocked);
    }

    private void Update()
    {
        m_lockPanel.transform.position = transform.position;
    }

    public void SlideBoard(StageSelectManager.StageGrade currentGrade, Action action)
    {
     

        float targetPosX = startAnchoredPosition.x + (-800f * (int)currentGrade);

        contentsController.HideContents();

        rectTransform.DOAnchorPosX(targetPosX, 0.5f)
           .SetEase(Ease.InOutSine)
           .OnComplete(() =>
           {
               contentsController.ViewContents(currentGrade);

               action.Invoke();
           })
           .SetLink(gameObject);

    }
}