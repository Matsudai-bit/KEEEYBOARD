using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MahTileController : MonoBehaviour
{

    [Header("通常色")]
    [SerializeField]
    Color m_normalColor;
    [Header("終わりが近いときの色")]
    [SerializeField]
    Color m_lastColor;
    [Header("終わりが近い(これ以下はもうすぐ)")]
    int m_lastCount = 5;

    [Header ("表示時間")]
    [SerializeField]
    float m_viewTime;

    [Header("消えるのにかかる時間")]
    [SerializeField]
    float m_fadeOutTime;

    [Header("目標連打数")]
    [SerializeField]
    int m_mashingStartCount = 0;

    [Header("現在のヒット数")]
    [SerializeField]
    int m_hitCurrentCount = 0;


    [SerializeField]
    GameTile m_gameTile;

    [SerializeField]
    CommandTile m_commandTile;

    [SerializeField]
    GameObject m_mashingPunchTextUI;

    [SerializeField]
    Canvas m_canvas;

    [SerializeField]
    float m_timeCounter;


    private TextMeshProUGUI mashingPunchText;

    private void Awake()
    {
        Debug.Log("cerate mashing tile");

        m_gameTile = gameObject?.GetComponent<GameTile>();
        m_commandTile = gameObject?.GetComponent<CommandTile>();

        if (!m_gameTile)
            Debug.LogError("m_gameTile null");

        if (!m_commandTile)
            Debug.LogError("m_commandTile null");

        m_mashingPunchTextUI = GameObject.Find("MashingPunchTextUI");
        mashingPunchText = m_mashingPunchTextUI.GetComponent<TextMeshProUGUI>();

        m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void Start()
    {
        m_mashingStartCount = m_gameTile.Tilemap.GetTile<MashingTile>(m_gameTile.CellPosition).MashingCount;
    }

    private void Update()
    {
    }

    public int MashingCount
    {
        get { return m_mashingStartCount; }
        set { m_mashingStartCount = value; }
    }

    public void PunchWall()
    {
        m_hitCurrentCount++;

        m_timeCounter = 0.0f;

        DrawCounterText(GetRemainingCount());
        m_mashingPunchTextUI.SetActive(true);


        if (GetRemainingCount() <= 0 && !DOTween.IsTweening(m_mashingPunchTextUI.GetComponent<TextMeshProUGUI>()))
        {
            // 普通のコマンドタイルにする
            m_gameTile.SetTileType(GameTile.TileType.COMMAND);
            m_commandTile.SetState(CommandTile.State.DEFAULT);
            m_mashingPunchTextUI.SetActive(false);
          //  Destroy(this);
            return;
        }
        StartFadeOut();
    }

    void DrawCounterText(int count)
    {
        m_mashingPunchTextUI.GetComponent<TextMeshProUGUI>().DOKill();

        mashingPunchText.text = count.ToString();

        if (m_canvas == null || m_mashingPunchTextUI == null) return;

        // 1. ワールド座標をスクリーン座標に変換 (カメラはnullならOverlay用)
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        // 2. スクリーン座標をCanvasのローカル座標に変換
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_canvas.GetComponent<RectTransform>(),
            screenPoint,
            null,
            out localPoint
        );
        m_mashingPunchTextUI.transform.localPosition = localPoint;

        m_mashingPunchTextUI.GetComponent<TextMeshProUGUI>().color = (GetRemainingCount() > m_lastCount) ? m_normalColor : m_lastColor;

    }

    /// <summary>
    /// 残りの数の取得
    /// </summary>
    /// <returns></returns>
    int GetRemainingCount()
    {
        return m_mashingStartCount - m_hitCurrentCount;
    }

    void StartFadeOut()
    {
        m_mashingPunchTextUI.GetComponent<TextMeshProUGUI>().DOFade(0.0f, m_fadeOutTime).OnComplete(()=> {
            m_hitCurrentCount = 0;
        });

    }

    

}
