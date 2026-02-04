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
        public Vector2 initialPinPos; // ピンのRectTransform

        public void SetPosition(Vector2 pos)
        {
            initialPinPos = pos;
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
                contentsList[i].contentObject.transform.DOScale(Vector3.one, 0.125f)
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

            contentsList[i].contentObject.SetActive(false);
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
                //pinImage.transform.position = new Vector3(pinImage.transform.position.x, pinImage.transform.position.y + 100.0f, pinImage.transform.position.z);
                pinImage.GetComponent<RectTransform>().anchoredPosition = contentsList[i].initialPinPos;
                //pinImage.GetComponent<RectTransform>().DOAnchorPos(contentsList[i].initialPinPos, 0.25f)
                //    .SetEase(Ease.InOutSine)
                //    .SetDelay((int)contentsList[i].number * 0.125f);
            }
        }
    }

}
