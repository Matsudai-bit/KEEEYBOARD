using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileDirectionData
{
    [Serializable]
    public enum Direction
    {
        FORWARD,    // 前
        BACKWARD,   // 後ろ
        LEFT,       // 左
        RIGHT,      // 右
    }
    static private List<Vector3Int> moveDirection = new List<Vector3Int>
    {
        new ( 0,  1),   // FORWARD
        new ( 0, -1),   // BACKWARD
        new ( 1,  0),   // LEFT
        new (-1,  0),   // RIGHT
    };

    static public Vector3Int GetMoveDirection(Direction direction)
    {
        return TileDirectionData.moveDirection[(int)(direction)];
    }
}

/// <summary>
/// 移動タイル
/// </summary>
public class TileMovement : MonoBehaviour
{
 

    private GameTile m_gameTile;


    private void Awake()
    {
        m_gameTile = gameObject?.GetComponent<GameTile>();
        if (!m_gameTile)
        {
            Debug.LogError("m_gameTileがnullです");
        }
    }

    /// <summary>
    /// タイルの移動
    /// </summary>
    /// <param name="moveDirection"></param>
    public void MoveTile(Vector3Int moveDirectionVector)
    {
        var newPosition = m_gameTile.CellPosition + moveDirectionVector;

        ApplyMove(newPosition);
    }
    public void MoveTile(TileDirectionData.Direction moveDirection)
    {
        var moveDirectionVector = TileDirectionData.GetMoveDirection(moveDirection);
        MoveTile(moveDirectionVector);

    }

    /// <summary>
    /// 移動の適用
    /// </summary>
    /// <param name="toPosition"></param>
    private void ApplyMove(Vector3Int toPosition)
    {
        var tilemap = m_gameTile.Tilemap;
        var fromPosition = m_gameTile.CellPosition;
        // 1. 元の座標にあるタイルを取得
        TileBase tile = tilemap.GetTile(fromPosition);
        if (tile == null) return;

        // 2. 新しい座標に同じタイルをセット
        tilemap.SetTile(toPosition, tile);
        var newObect = tilemap.GetInstantiatedObject(toPosition);
        var oldObect = tilemap.GetInstantiatedObject(fromPosition);

        var newGameTileParent = newObect.GetComponent<GameTileParent>();
        var oldGameTileParent = oldObect.GetComponent<GameTileParent>();


        //// 3. 元の座標を空にする（削除）
        Destroy(newGameTileParent.Child);
        newObect.GetComponent<GameTileParent>().SetChild(oldGameTileParent.GameTile, oldGameTileParent.Child);
        tilemap.SetTile(fromPosition, null);

        oldGameTileParent.GameTile.CellPosition = toPosition;
    }
}
