using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Runtime.CompilerServices.RuntimeHelpers;

/// <summary>
/// 使用するキーデータ
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "KeyboardGuideSpriteData", menuName = "ScriptableObjects/KeyboardGuideSpriteData")]
public class KeyboardGuideSpriteData : ScriptableObject
{



    [Serializable]
    public struct KeyAndSpritePair
    {
        public string keyName;
        public Key key;
        public Sprite sprite;
    }

    [SerializeField]
    private Sprite[] guideSprites;

    [SerializeField]
    private UsingKeyData m_usingKeyData ; // 使用できるキーデータ

    [SerializeField]
    private List<KeyAndSpritePair> m_keyAndSpritePair = new();

    private Dictionary<Key, Sprite> m_keyboardGuideSpriteDict;

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
        m_keyboardGuideSpriteDict = new Dictionary<Key, Sprite>();

        if (m_keyAndSpritePair == null) return;

        foreach (var keyPair in m_keyAndSpritePair)
        {
            // キーが未設定の場合は飛ばす
            if (keyPair.key == Key.None) continue;

            // そのキー用の「状態→画像」の辞書を作成
            var stateDict = new Dictionary<Key, Sprite>();
     
            // メイン辞書に追加（キーの重複エラー防止）
            if (!m_keyboardGuideSpriteDict.ContainsKey(keyPair.key))
            {
                m_keyboardGuideSpriteDict.Add(keyPair.key, keyPair.sprite);
            }
        }

        Debug.Log($"Dictionary initialized: {m_keyboardGuideSpriteDict.Count} keys registered.");
    }

    /// <summary>
    /// 外部から「キー」と「状態」を指定してスプライトを取り出す関数
    /// </summary>
    public Sprite GetSprite(Key key)
    {
        if (m_keyboardGuideSpriteDict == null) InitializeDictionary();

        if (m_keyboardGuideSpriteDict.TryGetValue(key, out var sprite))
        {
            return sprite;
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

        var states = (Key[])Enum.GetValues(typeof(Key));

        int spriteNum = states.Length * m_usingKeyData.KeyCodes.Count;
        for (int x = 0; x < m_usingKeyData.KeyCodes.Count; x++)
        {
            Key keyCode = m_usingKeyData.KeyCodes[x];

            KeyAndSpritePair keyAndSprite = new KeyAndSpritePair();
            keyAndSprite.keyName = keyCode.ToString();

            keyAndSprite.keyName = keyCode.ToString();
            keyAndSprite.key = keyCode;
            keyAndSprite.sprite = guideSprites[x];
            m_keyAndSpritePair.Add(keyAndSprite);
        }

   

        Debug.Log($"KeyboardGuideSpriteData: {m_keyAndSpritePair.Count}件のキーデータを生成しました。");
    }



}
