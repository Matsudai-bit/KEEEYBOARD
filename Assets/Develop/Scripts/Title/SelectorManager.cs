using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class SelectorManager : MonoBehaviour
{
    [Header("Selectoers")]
    [SerializeField]
    private GameObject startSelecter;
    [SerializeField]
    private GameObject endSelecter;

    [Header("シーンのフェードインエフェクト")]
    [SerializeField]
    SceneTransitionEffect m_sceneFadeInEffect;
    [Header("シーンのフェードアウトエフェクト")]
    [SerializeField]
    SceneTransitionEffect m_sceneFadeOutEffect;

    // 現在の選択状態
    private bool m_isStartSelecting = true;

    // 点滅アニメーション用シーケンス
    private Sequence flickerSequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_sceneFadeInEffect.StartTransition(() => { });

        // 規則的な点滅パターンを作成
        flickerSequence = DOTween.Sequence();
        flickerSequence.Append(startSelecter.GetComponent<UnityEngine.UI.Image>().DOFade(0.2f, 0.02f));
        flickerSequence.Append(endSelecter.GetComponent<UnityEngine.UI.Image>().DOFade(0.2f, 0.02f));
        flickerSequence.AppendInterval(0.5f);
        flickerSequence.Append(startSelecter.GetComponent<UnityEngine.UI.Image>().DOFade(1.0f, 0.02f));
        flickerSequence.Append(endSelecter.GetComponent<UnityEngine.UI.Image>().DOFade(1.0f, 0.02f));
        flickerSequence.AppendInterval(0.5f);
        flickerSequence.SetLoops(-1); // 無限ループ
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

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        
        m_sceneFadeOutEffect.StartTransition(() => {
            if (m_isStartSelecting)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelect");
            }
            else
            {

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                Application.Quit();//ゲームプレイ終了
#endif
            }
        });

   
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        var axis = context.ReadValue<Vector2>();
        if (Mathf.Approximately(axis.magnitude, 0.0f)) { return; }
        bool right = (axis.x > 0.0f) ? true : false;

        if (right)
        {
            m_isStartSelecting = false;

        }
        else
        {
            m_isStartSelecting = true;

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
