using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MahTileController : MonoBehaviour
{
    [SerializeField]
    int m_mashingCount = 0;

    [SerializeField]
    GameTile m_gameTile;

    [SerializeField]
    CommandTile m_commandTile;

    private void Awake()
    {
        Debug.Log("cerate mashing tile");

        m_gameTile = gameObject?.GetComponent<GameTile>();
        m_commandTile = gameObject?.GetComponent<CommandTile>();

        if (!m_gameTile)
            Debug.LogError("m_gameTile null");

        if (!m_commandTile)
            Debug.LogError("m_commandTile null");

    }

    private void Start()
    {
        m_mashingCount = m_gameTile.Tilemap.GetTile<MashingTile>(m_gameTile.CellPosition).MashingCount;
    }

    public int MashingCount
    {
        get { return m_mashingCount; }
        set { m_mashingCount = value; }
    }

    public void PunchWall()
    {
        m_mashingCount--;

        if (m_mashingCount <= 0)
        {
            // 普通のコマンドタイルにする
            m_gameTile.SetTileType(GameTile.TileType.COMMAND);
            m_commandTile.SetState(CommandTile.State.DEFAULT);
        }
    }

}
