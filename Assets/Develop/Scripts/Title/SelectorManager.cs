using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SelectorManager : MonoBehaviour
{
    [Header("Selectoers")]
    [SerializeField]
    private GameObject startSelecter;
    [SerializeField]
    private GameObject endSelecter;

    // 現在の選択状態
    private bool m_isStartSelecting = true;

    // 点滅アニメーション用シーケンス
    private Sequence flickerSequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 規則的な点滅パターンを作成
        flickerSequence = DOTween.Sequence();
        flickerSequence.Append(startSelecter.GetComponent<UnityEngine.UI.Image>().DOFade(0.2f, 0.02f));
        flickerSequence.Append(endSelecter.GetComponent<UnityEngine.UI.Image>().DOFade(0.2f, 0.02f));
        flickerSequence.AppendInterval(0.5f);
        flickerSequence.Append(startSelecter.GetComponent<UnityEngine.UI.Image>().DOFade(1.0f, 0.02f));
        flickerSequence.Append(endSelecter.GetComponent<UnityEngine.UI.Image>().DOFade(1.0f, 0.02f));
        flickerSequence.AppendInterval(0.5f);
        flickerSequence.SetLoops(-1); // 無限ループ

    }

    // Update is called once per frame
    void Update()
    {
        // 入力受け付け
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_isStartSelecting = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_isStartSelecting = false;
        }

        // セレクターの表示切り替え
        if (m_isStartSelecting)
        {
            startSelecter.SetActive(true);
            endSelecter.SetActive(false);
        }
        else
        {
            startSelecter.SetActive(false);
            endSelecter.SetActive(true);
        }

    }
}
