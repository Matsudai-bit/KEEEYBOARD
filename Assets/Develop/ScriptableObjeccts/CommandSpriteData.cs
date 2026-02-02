using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Runtime.CompilerServices.RuntimeHelpers;

/// <summary>
/// 使用するキーデータ
/// </summary>
[CreateAssetMenu(fileName = "CommandSpriteData", menuName = "ScriptableObjects/CommandSpriteData")]
public class CommandSpriteData : ScriptableObject
{
    [Serializable]
    public enum TileState
    {
        DEFAULT,    // 通常
        VISITED,    // 通過済み
        MOVABLE     // 移動候補
    }

    [Serializable]

    public struct SpriteAndTileStatePair
    {
        public string label;
        public TileState tileState;
        public Sprite sprite;
    }

    [Serializable]
    public struct KeyAndSpritePair
    {
        public string keyName;
        public Key key;
        public List<SpriteAndTileStatePair> spritePair;
    }

    [SerializeField]
    private Sprite[] commandSprites;

    [SerializeField]
    private UsingKeyData m_usingKeyData ; // 使用できるキーデータ

    [SerializeField]
    private List<KeyAndSpritePair> m_keyAndSpritePair = new();

    private Dictionary<Key, Dictionary<TileState, Sprite>> m_commandSpriteDict;

    public void Awake()
    {
        InitializeDictionary();
    }
    /// <summary>
    /// インスペクターのリストを検索用辞書に変換する
    /// </summary>
    public void InitializeDictionary()
    {
        // 辞書の初期化
        m_commandSpriteDict = new Dictionary<Key, Dictionary<TileState, Sprite>>();

        if (m_keyAndSpritePair == null) return;

        foreach (var keyPair in m_keyAndSpritePair)
        {
            // キーが未設定の場合は飛ばす
            if (keyPair.key == Key.None) continue;

            // そのキー用の「状態→画像」の辞書を作成
            var stateDict = new Dictionary<TileState, Sprite>();

            foreach (var spritePair in keyPair.spritePair)
            {
                // 同じ状態が重複して登録されないようにチェック
                if (!stateDict.ContainsKey(spritePair.tileState))
                {
                    stateDict.Add(spritePair.tileState, spritePair.sprite);
                }
            }

            // メイン辞書に追加（キーの重複エラー防止）
            if (!m_commandSpriteDict.ContainsKey(keyPair.key))
            {
                m_commandSpriteDict.Add(keyPair.key, stateDict);
            }
        }

        Debug.Log($"Dictionary initialized: {m_commandSpriteDict.Count} keys registered.");
    }

    /// <summary>
    /// 外部から「キー」と「状態」を指定してスプライトを取り出す関数
    /// </summary>
    public Sprite GetSprite(Key key, TileState state)
    {
        if (m_commandSpriteDict == null) InitializeDictionary();

        if (m_commandSpriteDict.TryGetValue(key, out var stateDict))
        {
            if (stateDict.TryGetValue(state, out var sprite))
            {
                return sprite;
            }
        }
        return null;
    }
    /// <summary>
    /// インスペクターを右クリックして実行できる初期化ボタン
    /// </summary>
    [ContextMenu("Generate Sprite List From Keys")]
    public void GenerateList()
    {
        if (m_usingKeyData == null)
        {
            Debug.LogError("UsingKeyDataがアサインされていません！");
            return;
        }

        // 現在のリストをクリア（上書きしたくない場合は調整が必要）
        m_keyAndSpritePair.Clear();

        var states = (TileState[])Enum.GetValues(typeof(TileState));

        int spriteNum = states.Length * m_usingKeyData.KeyCodes.Length;
        for (int x = 0; x < m_usingKeyData.KeyCodes.Length; x++)
        {
            Key keyCode = m_usingKeyData.KeyCodes[x];

            KeyAndSpritePair keyAndSprite = new KeyAndSpritePair();
            keyAndSprite.keyName = keyCode.ToString();
            keyAndSprite.spritePair = new List<SpriteAndTileStatePair>();

            for (int y = 0; y < states.Length; y++)
            {
                int index = (m_usingKeyData.KeyCodes.Length * y) + x;

                SpriteAndTileStatePair spritePair = new SpriteAndTileStatePair
                {
                    label = states[y].ToString(),
                    tileState = states[y],
                    sprite = commandSprites[index]
                };
                keyAndSprite.spritePair.Add(spritePair);
            }
            keyAndSprite.keyName = keyCode.ToString();
            keyAndSprite.key = keyCode;
            m_keyAndSpritePair.Add(keyAndSprite);
        }

        //// UsingKeyDataのKeyCodesループ
        //foreach (var keyCode in m_usingKeyData.KeyCodes)
        //{
        //    KeyAndSpritePair keyAndSprite = new KeyAndSpritePair();

        //    // KeyCode (Unity標準) から InputSystemの Key に変換が必要な場合はキャスト等を行う
        //    // ここでは仮に Key への変換として登録
        //    keyAndSprite.keyName = keyCode.ToString();
        //    // keyAndSprite.key = ... (型を合わせる)

        //    keyAndSprite.spritePair = new List<SpriteAndTileStatePair>();

        //    for (int j = 0; j < states.Length; j++)
        //    {
        //        SpriteAndTileStatePair spritePair = new SpriteAndTileStatePair
        //        {
        //            label = states[j].ToString(),
        //            tileState = states[j],
        //            sprite = null
        //        };
        //        keyAndSprite.spritePair.Add(spritePair);
        //    }
        //    keyAndSprite.keyName = keyCode.ToString();
        //    keyAndSprite.key = keyCode;
        //    m_keyAndSpritePair.Add(keyAndSprite);
        //}

        Debug.Log($"CommandSpriteData: {m_keyAndSpritePair.Count}件のキーデータを生成しました。");
    }



}
