using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

[Serializable]
public class GameTile : MonoBehaviour
{
    [SerializeField]
    Vector3Int m_position; // 座標

    [SerializeField]
    Tilemap m_tilemap;

    [SerializeField]
    TilemapData m_tilemapData;

    [SerializeField]
    TileType m_tileType;

    [SerializeField]
    public enum TileType
    {
        NONE,
        COMMAND,        // コマンドタイル
        KEY,            // 鍵タイル
        LOCKED_DOOR,    // 鍵付き扉
        PLAYER

    }

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

    public TileType GetTileType()
    { return m_tileType; }


    public void ChangeSprite(Sprite newSprite)
    {
        // 1. 現在の座標にあるタイルとGameTileParentを取得
        Tile currentTileAsset = Tilemap.GetTile<Tile>(CellPosition);
        GameObject oldContainer = Tilemap.GetInstantiatedObject(CellPosition);

        if (currentTileAsset == null || oldContainer == null || newSprite == null) return;

        var oldParent = oldContainer.GetComponent<GameTileParent>();

        // 2. 新しいタイルアセットを作成（コピー）してスプライトを差し替える
        Tile newTileAsset = Instantiate(currentTileAsset);
        newTileAsset.sprite = newSprite;

        // 3. 【重要】「中身（Child）」を一旦親子関係から切り離して保護する
        // これにより、後で Tilemap.SetTile しても一緒に消えなくなる
        GameObject body = oldParent.Child;
        body.transform.SetParent(null);

        // 4. タイルを更新（ここで新しいコンテナGameObjectが自動生成される）
        Tilemap.SetTile(CellPosition, newTileAsset);

        // 5. 新しく生成されたコンテナを取得し、中身を移し替える
        GameObject newContainer = Tilemap.GetInstantiatedObject(CellPosition);
        if (newContainer != null)
        {
            var newParent = newContainer.GetComponent<GameTileParent>();

            // 新しく自動生成された方の「中身」は不要なので消す
            if (newParent.Child != null) Destroy(newParent.Child);

            // 保護していた元の「中身」を新しい親にセット
            newParent.SetChild(oldParent.GameTile, body);

            // 座標データを同期
            oldParent.GameTile.CellPosition = CellPosition;
        }

        // 古いコンテナ（枠）はもう不要（SetTileで消えているはずだが念のため）
        if (oldContainer != null && oldContainer != newContainer) Destroy(oldContainer);
    }
}
