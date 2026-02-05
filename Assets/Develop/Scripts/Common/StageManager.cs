using UnityEngine;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour
{
    [Header("グリッド")]
    [SerializeField]
    GameObject m_grid;

    [Header("ステージのタイルマップペア設定")]
    [SerializeField]
    StageData m_tilemapPair;


    [Header("時間")]
    [SerializeField]
    SpriteMultiLineTimer m_timer;

    [Header("ゲームタイル管理")]
    [SerializeField]
    GameTileManager m_gameTileManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // タイマーに時間の設定
        m_timer.SetDuration(m_tilemapPair.TimeLimit);
       var baseTilemap  = Instantiate(m_tilemapPair.BaseTilemap   , m_grid.transform);
       var topTilemap = Instantiate(m_tilemapPair.TopTilemap    , m_grid.transform);

        m_gameTileManager.SetUpTile(baseTilemap.GetComponent<Tilemap>(), topTilemap.GetComponent<Tilemap>());

    }

    private void Start()
    {

//        m_gameTileManager.SetUpTile();

    }

    public StageData TilemapPair
    {
        get { return m_tilemapPair; }
    }
    

}
