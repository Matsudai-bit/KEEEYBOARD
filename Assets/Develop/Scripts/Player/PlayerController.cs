using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Tilemaps;

/// <summary>
/// プレイヤーのコントローラ
/// </summary>
public class PlayerController : MonoBehaviour
{
    private GameTile m_gameTile;
    private TileMovement m_tileMovement;
    private KeyBox m_keyBox;

    private PlayerMovableTileSelector m_movableTileSelector; // 移動可能タイル選択器

    bool isPressed = false;
    Vector3Int m_pressedStartPosition;
    List<VisitedTileData> m_visitedTileData = new();

    struct VisitedTileData
    {
        public CommandTile commandTile;
        public TileDirectionData.Direction directionID;

        public VisitedTileData(CommandTile commandTile, TileDirectionData.Direction directionID)
        {
            this.commandTile = commandTile;
            this.directionID = directionID;
        }
    }
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
        m_keyBox = gameObject?.GetComponent<KeyBox>();
        if (!m_keyBox)
        {
            Debug.LogError("m_keyBoxがnullです");
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
            var directionID = m_movableTileSelector.GetDirection(pressedKey);
            var visitObject = m_gameTile.TilemapDatta.baseTilemap.GetInstantiatedObject(m_gameTile.CellPosition + TileDirectionData.GetMoveDirection(directionID)).GetComponent<GameTileParent>().Child;
            if (visitObject.TryGetComponent<MahTileController>(out var mashTileController))
            {
                mashTileController.PunchWall();
                if (mashTileController.MashingCount > 0)
                return;
            }
            if (TryMoveTile(directionID))
            {

                var visitedTile = visitObject.GetComponent<CommandTile>();
                m_visitedTileData.Add(new VisitedTileData(visitedTile, directionID));

                VisitTile(m_gameTile.CellPosition);
                SoundManager.GetInstance.RequestPlaying(SoundID.SE_INGAME_PLAYER_MOVE);
                if (visitedTile.GameTile.GetTileType() == GameTile.TileType.SAFE)
                {
                    StayTile();
                }
            }

            FindCandidates();
        }
        Debug.Log("pressed" + pressedKey.ToString());
    }
    private void OnReleaseKey(Key releasedKey)
    {
        if (m_visitedTileData.Any(item => item.commandTile.Key == releasedKey))
        {
            // リストの末尾から先頭に向かってループを回す
            for (int i = m_visitedTileData.Count - 1; i >= 0; i--)
            {
                CancelVisitTile(m_gameTile.CellPosition);

                // 処理対象のキーを取得
                Key currentKey = m_visitedTileData[i].commandTile.Key;

                // タイルの移動処理
                FindCandidates();

                var oldPosition = m_gameTile.CellPosition;
                if (TryMoveTile(m_visitedTileData[i].directionID, true))
                {
                    CancelVisitTile(m_gameTile.CellPosition);

                    // リストから削除（現在のインデックスを消す）
                    m_visitedTileData.RemoveAt(i);

                }
                VisitTile(m_gameTile.CellPosition);

                // 離されたキーに到達したらループ終了
                if (currentKey == releasedKey)
                {
                    break;
                }
            }
        }

        Debug.Log("released" + releasedKey.ToString());

    }

    /// <summary>
    /// タイルを移動する
    /// </summary>
    /// <param name="directionID"></param>
    /// <param name="reverseVector"></param>
    bool TryMoveTile(TileDirectionData.Direction directionID, bool reverseVector = false)
    {
        // 移動地点にアイテムがあるか確認する
        var toPosition = m_gameTile.CellPosition + TileDirectionData.GetMoveDirection(directionID, reverseVector);
        var toGameObject = m_gameTile.Tilemap.GetInstantiatedObject(toPosition);

        if (toGameObject && toGameObject.GetComponent<GameTileParent>() && toGameObject.GetComponent<GameTileParent>().GameTile)
        {
            if (toGameObject.GetComponent<GameTileParent>().GameTile.GetTileType() == GameTile.TileType.KEY)
            {
                m_keyBox.StoreKey();
                PlayerEventMessenger.GetInstance.Notify(PlayerEventID.GET_KEY, m_gameTile);

            }

            if (toGameObject.GetComponent<GameTileParent>().GameTile.GetTileType() == GameTile.TileType.LOCKED_DOOR)
            {
                if (m_keyBox.KeyCount > 0)
                {
                    m_keyBox.RestoreKey();
                    PlayerEventMessenger.GetInstance.Notify(PlayerEventID.UNLOCK, m_gameTile);
                }
                else
                {
                    SoundManager.GetInstance.RequestPlaying(SoundID.SE_INGAME_PLAYER_NOT_VISIT);
                    return false;
                }
            }
        }

        // タイルの移動
        m_tileMovement.MoveTile(directionID, reverseVector);

        FindCandidates();

        return true;

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
            if (candidate.GetState() != CommandTile.State.VISITED)
                candidate.SetState(CommandTile.State.DEFAULT);
        }
    }

    void ChangeMovableStateOfCandidatesTile()
    {
        foreach (var candidate in m_movableTileSelector.GetCandidates())
        {
            if (candidate.GetState() != CommandTile.State.VISITED)
                candidate.SetState(CommandTile.State.MOVABLE);
        }
    }


    /// <summary>
    /// タイルを訪れる
    /// </summary>
    /// <param name="visitedTilePosition"></param>
    void VisitTile(Vector3Int visitedTilePosition)
    {
        var visitedTileObject = m_gameTile.TilemapDatta.baseTilemap.GetInstantiatedObject(visitedTilePosition);
        if (visitedTileObject && visitedTileObject.GetComponent<GameTileParent>() && visitedTileObject.GetComponent<GameTileParent>().Child.GetComponent<CommandTile>())
        {
            visitedTileObject.GetComponent<GameTileParent>().Child.GetComponent<CommandTile>().SetState(CommandTile.State.VISITED);
        }
    }

    void CancelVisitTile(Vector3Int visitedTilePosition)
    {
        var visitedTileObject = m_gameTile.TilemapDatta.baseTilemap.GetInstantiatedObject(visitedTilePosition);
        if (visitedTileObject && visitedTileObject.GetComponent<GameTileParent>() && visitedTileObject.GetComponent<GameTileParent>().Child.GetComponent<CommandTile>())
        {
            visitedTileObject.GetComponent<GameTileParent>().Child.GetComponent<CommandTile>().SetState(CommandTile.State.DEFAULT);
        }
    }

    void StayTile()
    {
        SoundManager.GetInstance.RequestPlaying(SoundID.SE_INGAME_PLAYER_VISIT_SAFETILE);

        foreach (var visitedTile in m_visitedTileData)
        {
            visitedTile.commandTile.SetState(CommandTile.State.DEFAULT);
        }
        m_visitedTileData.Clear();

        

    }

  
}
