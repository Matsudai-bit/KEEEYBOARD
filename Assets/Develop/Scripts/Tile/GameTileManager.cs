using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameTileManager : MonoBehaviour
{
    [SerializeField]
    private string m_commandTiletipBaseName;
    [SerializeField]
    private Tilemap m_baseTilemap;
    [SerializeField]
    private Tilemap m_topTilemap;
    [SerializeField]
    private UsingKeyData m_usingKey;
    [SerializeField]
    private CommandSpriteData m_commandSpriteData;

    private Dictionary<Key, List<GameTile>> m_gameTileDict;

    

    private void Awake()
    {
        
        m_gameTileDict = new();
        // 辞書の作成
        foreach (var keyCode in m_usingKey.KeyCodes)
        {
            m_gameTileDict.Add(keyCode, new());
        }

        // タイルマップの座標を設定
        InitializeTilemap(m_baseTilemap, m_gameTileDict, m_commandTiletipBaseName, m_usingKey, m_baseTilemap, m_topTilemap, m_commandSpriteData);

        InitializeTilemap(m_topTilemap, m_gameTileDict, m_commandTiletipBaseName, m_usingKey, m_baseTilemap, m_topTilemap, m_commandSpriteData);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        //var keyboard = Keyboard.current;
        //if (keyboard == null) return;

        //// 現在押されているすべてのキーをチェック
        //// keyboard.allKeysを回すのではなく、登録されているキーだけをチェック
        //foreach (var keyCode in m_gameTileDict.Keys)
        //{
        //    // keyboard[Key.A] のようなインデクサで直接アクセス可能
        //    var keyControl = keyboard[keyCode];

        //    if (keyControl.wasPressedThisFrame)
        //    {
        //        // 辞書にあるリストを直接回す
        //        foreach (var gameTile in m_gameTileDict[keyCode])
        //        {
        //            Sprite newSprite = m_commandSpriteData.GetSprite(keyCode, CommandSpriteData.TileState.MOVABLE);
        //            if (newSprite == null) continue;

        //            // タイル生成部分の簡略化（後述）
        //            UpdateTileSprite(gameTile.CellPosition, newSprite);
        //        }
        //    }
        //}
    
    }

    private void UpdateTileSprite(Vector3Int position, Sprite sprite)
    {
        Tile newTile = ScriptableObject.CreateInstance<Tile>();
        newTile.sprite = sprite;
        
        m_baseTilemap.SetTile(position, newTile);
    }

    // 1つのメソッドで「座標セット」と「辞書登録」を同時に行う
    static void InitializeTilemap( Tilemap tilemap, Dictionary<Key, List<GameTile>> dict, string baseName, UsingKeyData keys, Tilemap baseTilemap, Tilemap topTilemap, CommandSpriteData commandSpriteData)
    {
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            GameObject tileObj = tilemap.GetInstantiatedObject(pos).GetComponent<GameTileParent>().Child;
            if (tileObj == null) continue;

            if (tileObj.TryGetComponent<GameTile>(out var gameTile))
            {
                gameTile.CellPosition   = pos; // 座標セット
                gameTile.Tilemap    = tilemap; // タイルマップセット
                gameTile.TilemapDatta = new(baseTilemap, topTilemap);

                // ついでにコマンドタイルの判定もここで行う
                TileBase asset = tilemap.GetTile(pos);
                if (gameTile.GetTileType() == GameTile.TileType.COMMAND)
                {
                    string keyName = asset.name.Replace(baseName, "");
                    Key code = keys.KeyCodes.Find(k => keyName == k.ToString());

                    var commandTile = tileObj.AddComponent<CommandTile>();
                    commandTile.Key = code;
                    commandTile.SpriteData = commandSpriteData;
                    if (dict.ContainsKey(code)) dict[code].Add(gameTile);
                }
            }
        }
    }
}
