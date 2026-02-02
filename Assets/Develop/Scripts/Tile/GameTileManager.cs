using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTileManager : MonoBehaviour
{
    [SerializeField]
    private string m_commandTiletipBaseName;
    [SerializeField]
    private Tilemap m_tilemap;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 1. タイルが存在する「枠（境界線）」を取得
        BoundsInt bounds = m_tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (m_tilemap.HasTile(pos))
            {
                // 1. タイルアセットを取得
                TileBase tile = m_tilemap.GetTile(pos);

                // 2. その座標に生成されているGameObjectを取得
                // ※タイルアセットの「GameObject」欄にPrefabが設定されている場合のみ取得できます
                GameObject tileObj = m_tilemap.GetInstantiatedObject(pos);

                if (tileObj != null)
                {
                    // 3. コンポーネントを取得して座標をセット
                    GameTile gameTile = tileObj.GetComponent<GameTile>();
                    if (gameTile != null)
                    {
                        if (tile.name.Contains(m_commandTiletipBaseName))
                        {
                           var commandTile =  tileObj.AddComponent<CommandTile>();
                            
                        }
                        gameTile.Position = pos;
                    }
                }
                else
                {
                    // ここでnullになる場合、タイルアセットにPrefabが設定されていないか、
                    // TilemapRendererのModeが「Individual」になっていない可能性があります
                    Debug.LogWarning($"{pos} のタイルにGameObjectが見つかりません。");
                }

                Vector3 worldPos = m_tilemap.CellToWorld(pos);
                Debug.Log($"タイル名: {tile.name}, グリッド: {pos}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
