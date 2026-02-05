using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class ContentsController : MonoBehaviour
{
    // コンテンツ情報の構造体
    [System.Serializable]
    public struct ContentsInfo
    {
        public GameObject contentObject; // コンテンツのGameObject
        public StageSelectManager.StageGrade grade; // コンテンツの位置する階級
        public StageSelectManager.StageNumber number; // コンテンツの位置するステージ
        public Vector2 pinInitialPos; // ピンのRectTransform

        public void SetPosition(Vector2 pos)
        {
            pinInitialPos = pos;
        }
    }

    // コンテンツのリスト
    [SerializeField]
    private List<ContentsInfo> contentsList;


    // 表示する際に使用するTween
    private List<Tween> viewTween = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // structの中身を書き換えるときは foreach ではなく for を使います
        for (int i = 0; i < contentsList.Count; i++)
        {
            // 1. Tweenの登録（これはコピー経由でも contentObject 自体は参照型なので動きます）
            viewTween.Add(
                contentsList[i].contentObject.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.125f)
                .SetEase(Ease.InOutSine)
                .SetAutoKill(false)
                .SetDelay((int)contentsList[i].number * 0.125f)
                .Pause()
            );

            // 2. ピンの初期座標を取得
            Vector2 copyYes = contentsList[i].contentObject.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition;

            // 3. 重要：リストの要素を一度取り出して、書き換えてから、リストに戻す
            var info = contentsList[i];
            info.SetPosition(copyYes);
            contentsList[i] = info; // ここでリスト内の「本物」が更新されます

            // 4. 最初は非表示にしておく
            //contentsList[i].contentObject.SetActive(false);
            contentsList[i].contentObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }


    public void ViewContents(StageSelectManager.StageGrade grade)
    {
        for (int i = 0; i < contentsList.Count; i++)
        {
            if (contentsList[i].grade == grade)
            {
                contentsList[i].contentObject.SetActive(true);

            }
            else
            {
                contentsList[i].contentObject.SetActive(false);
            }
        }

        for (int i = 0; i < contentsList.Count; i++)
        {

            contentsList[i].contentObject.transform.localScale = Vector3.zero;

            if (contentsList[i].contentObject.activeSelf == false) continue;
            viewTween[i].Restart();

            // 子オブジェクトであるピンのImageのみアニメーションさせる
            Image pinImage = contentsList[i].contentObject.transform.GetChild(0).GetComponent<Image>();
            if (pinImage != null)
            {
                // RectTransformを取得
                RectTransform pinRect = pinImage.GetComponent<RectTransform>();

                // 既存のアニメーションがあれば停止
                pinRect.DOKill();

                // アニメーションの「開始位置」へ移動
                Vector2 startOffset = new Vector2(0f, 50f);
                pinRect.anchoredPosition = contentsList[i].pinInitialPos + startOffset;

                // 本来の位置(pinInitialPos)へ向かって移動
                pinRect.DOAnchorPos(contentsList[i].pinInitialPos, 0.5f)
                    .SetEase(Ease.OutBack)
                    .SetDelay((int)contentsList[i].number * 0.125f)
                    .SetLink(pinImage.gameObject);
            }
        }
    }


    public void HideContents()
    {
        for (int i = 0; i < contentsList.Count; i++)
        {
            contentsList[i].contentObject.transform.DOScale(Vector3.zero, 0.125f)
                .SetEase(Ease.InSine)
                .SetDelay((int)contentsList[i].number * 0.125f)
                .OnComplete(() =>
                {
                    contentsList[i].contentObject.SetActive(false);
                })
                .SetLink(gameObject);
        }
    }


    public void ViewOutLine(StageSelectManager.StageID stageID)
    {
        for(int i = 0; i < contentsList.Count; i++)
        {
            // ステージIDを計算
            int id = (int)contentsList[i].grade * 3 + (int)contentsList[i].number;
            if ((int)stageID == id)
            {
                contentsList[i].contentObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                contentsList[i].contentObject.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void HideOutLine()
    {
        for (int i = 0; i < contentsList.Count; i++)
        {
            contentsList[i].contentObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

}
