using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static CommandSpriteData;

/// <summary>
/// プレイヤーの移動可能タイルのセレクター
/// </summary>
public class PlayerMovableTileSelector 
{
    Dictionary<TileDirectionData.Direction, CommandTile> m_movementCandidates; // 移動候補

    GameTile m_playerTile; // プレイヤータイル

    public PlayerMovableTileSelector(GameTile playerTile)
    {
        m_movementCandidates = new();
        this.m_playerTile = playerTile;

    }

    /// <summary>
    /// 指定したキーを見つけたかどうか
    /// </summary>
    /// <param name="keyCode"></param>
    /// <returns></returns>
    public bool IsFoundTile(Key keyCode)
    {
        return m_movementCandidates.Values.Any(item => item.Key == keyCode);
    }

    public TileDirectionData.Direction GetDirection(Key keyCode)
    {
        return m_movementCandidates.ToList().Find(item => item.Value.Key == keyCode).Key;
    }

    public CommandTile GetCommandTile(TileDirectionData.Direction direction)
    {
        return m_movementCandidates[direction];
    }

    public void FindCandidates()
    {
        var directions = (TileDirectionData.Direction[])Enum.GetValues(typeof(TileDirectionData.Direction));
        m_movementCandidates.Clear();
        foreach (var directionID in directions)
        {
            var candidatePosition = m_playerTile.CellPosition + TileDirectionData.GetMoveDirection(directionID);

            var parentObject = m_playerTile.TilemapDatta.baseTilemap.GetInstantiatedObject(candidatePosition);
            if (!parentObject) { continue; }
            var candidateGameObject = parentObject.GetComponent<GameTileParent>().Child;
            if (!candidateGameObject) { continue; }
            var commandTile = candidateGameObject?.GetComponent<CommandTile>();
            if (!commandTile) { continue; }
                   
            m_movementCandidates[directionID] = commandTile;
        }
    }
}
