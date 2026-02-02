using DG.Tweening.Core.Easing;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEditor.U2D.ScriptablePacker;
using UnityEngine.UIElements;

public class InputTest : MonoBehaviour
{
    public Tilemap m_tilemap;
    public UsingKeyData m_usingKey;
    public CommandSpriteData m_commandSpriteData;

    private List<Vector3Int> m_Aobject = new List<Vector3Int>();

  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 1. タイルが存在する「枠（境界線）」を取得
        BoundsInt bounds = m_tilemap.cellBounds;


        // 2. その範囲内の座標をすべてループで回す
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            // 3. その座標にタイルがあるか確認
            if (m_tilemap.HasTile(pos))
            {
                TileBase tile = m_tilemap.GetTile(pos);
                if (tile.name == "ui_keyTile_0")
                {
                        m_Aobject.Add(pos);
                }
                // 座標をワールド座標（実際のゲーム内位置）に変換
                Vector3 worldPos = m_tilemap.CellToWorld(pos);
                Debug.Log($"タイル発見！ 名前: {tile.name}, グリッド座標: {pos}, ワールド座標: {worldPos}");
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
      

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // 現在押されているすべてのキーをチェック
        foreach (var key in keyboard.allKeys)
        {
            if (key.wasPressedThisFrame && key.isPressed)
            {
                // System.Array.Exists や LINQ を使うとループを二重に書かなくて済みます
                if (m_usingKey.KeyCodes.Exists( k => k == key.keyCode))
                {
                    if (key.keyCode == Key.A)
                    {
                        // m_Aobject が配列やリストの場合
                        for (int i = 0; i < m_Aobject.Count; i++)
                        {
                            // 1. 辞書からスプライトを取得
                            Sprite newSprite = m_commandSpriteData.GetSprite(key.keyCode, CommandSpriteData.TileState.MOVABLE);
                            if (newSprite == null) return;

                            // 2. 新しいタイルインスタンスを作成（または既存のタイルをコピー）
                            Tile newTile = ScriptableObject.CreateInstance<Tile>();
                            newTile.sprite = newSprite;

                            // 3. 指定した座標のタイルを差し替える
                            m_tilemap.SetTile(m_Aobject[i], newTile);

                        }
                    }
                    
                    Debug.Log($"当たり！押されたキー: {key.name}");
                }
            }
        }
    }
}
