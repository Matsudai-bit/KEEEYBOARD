using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーのコントローラ
/// </summary>
public class PlayerController : MonoBehaviour
{
    private GameTile m_gameTile;
    private TileMovement m_tileMovement;
    

    private PlayerMovableTileSelector m_movableTileSelector; // 移動可能タイル選択器

    bool isPressed = false;
    Vector3Int m_pressedStartPosition;
    List<Key> m_pushedKeys = new();
    List<TileDirectionData.Direction> m_moveDirections = new();
    private void Awake()
    {
        m_gameTile = gameObject?.GetComponent<GameTile>();
        if (!m_gameTile)
        {
            Debug.LogError("m_gameTileがnullです");
        }
        m_tileMovement = gameObject?.GetComponent<TileMovement>();
        if (!m_tileMovement)
        {
            Debug.LogError("m_tileMovementがnullです");
        }

        m_movableTileSelector = new(m_gameTile);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindCandidates();

    }

    // Update is called once per frame
    void Update()
    {

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // 現在押されているすべてのキーをチェック
        // keyboard.allKeysを回すのではなく、登録されているキーだけをチェック
        foreach (var key in keyboard.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                OnPressedKey(key.keyCode);
            }
            if (key.wasReleasedThisFrame)
            {
                OnReleaseKey(key.keyCode);
            }
        }

    }

    private void OnPressedKey(Key pressedKey)
    {
        if (m_movableTileSelector.IsFoundTile(pressedKey))
        {
            m_pushedKeys.Add(pressedKey);
            var directionID = m_movableTileSelector.GetDirection(pressedKey);
            m_tileMovement.MoveTile(directionID);
            m_moveDirections.Add(directionID);
            FindCandidates();
        }
        Debug.Log("pressed" + pressedKey.ToString());
    }
    private void OnReleaseKey(Key releasedKey)
    {
        if (m_pushedKeys.Contains(releasedKey))
        {
            // リストの末尾から先頭に向かってループを回す
            for (int i = m_pushedKeys.Count - 1; i >= 0; i--)
            {
                // 処理対象のキーを取得
                Key currentKey = m_pushedKeys[i];

                // タイルの移動処理
                FindCandidates();
                m_tileMovement.MoveTile(-TileDirectionData.GetMoveDirection(m_moveDirections[i]));

                // リストから削除（現在のインデックスを消す）
                m_pushedKeys.RemoveAt(i);
                m_moveDirections.RemoveAt(i);

                // 離されたキーに到達したらループ終了
                if (currentKey == releasedKey)
                {
                    break;
                }
            }
        }
        FindCandidates();

        Debug.Log("released" + releasedKey.ToString());

    }

    void FindCandidates()
    {
        ChangeDefaultStateOfCandidatesTile();

        m_movableTileSelector.FindCandidates();

        ChangeMovableStateOfCandidatesTile();

    }

    void ChangeDefaultStateOfCandidatesTile()
    {
        foreach (var candidate in m_movableTileSelector.GetCandidates())
        {
            candidate.SetState(CommandTile.State.DEFAULT);
        }
    }

    void ChangeMovableStateOfCandidatesTile()
    {
        foreach (var candidate in m_movableTileSelector.GetCandidates())
        {
            candidate.SetState(CommandTile.State.MOVABLE);
        }
    }
}
