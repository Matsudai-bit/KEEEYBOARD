using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class GameTile : MonoBehaviour
{
    [SerializeField]
    Vector3Int m_position; // 座標

    [SerializeField]
    Tilemap m_tilemap;

    [SerializeField]
    TilemapData m_tilemapData;

    [Serializable]
    public struct TilemapData
    {
        public Tilemap baseTilemap;
        public Tilemap topTilemap;

       public TilemapData(Tilemap baseTilemap, Tilemap topTilemap)
        {
            this.baseTilemap = baseTilemap;
            this.topTilemap = topTilemap;
        }
    }

    /// <summary>
    /// 座標のゲッターセッター
    /// </summary>
    public Vector3Int CellPosition
    {
        get { return m_position; }
        set { m_position = value; }
    }

    /// <summary>
    /// タイルマップのゲッターセッター
    /// </summary>
    public Tilemap Tilemap 
    {
        get { return m_tilemap; }
        set { m_tilemap = value; }
    }

    public TilemapData TilemapDatta
    {
        get { return m_tilemapData; }
        set { m_tilemapData = value; }
    }
}
