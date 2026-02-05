using System;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    [Serializable]
    public enum ID
    {
        TITLE,
        GAMEPLAY,
        RESULT_MODE,
    }
    [Serializable]
    public struct PlayingStage
    {
        public GameStage.GradeID gradeID;
        public GameStage.StageID stageID;

        public PlayingStage(GameStage.GradeID gradeID, GameStage.StageID stageID)
        {
            this.gradeID = gradeID;
            this.stageID = stageID;
        }
    }

    [SerializeField]
    private ID m_startId;

    [Header("プレイするステージ情報")]
    [SerializeField]
    private PlayingStage m_playingStage;

    private ID m_currentID;

    public ID CurrentStateID
    {
        get { return m_currentID; }
        set { m_currentID = value; }
    }


    public PlayingStage CurrentPlayingStage
    {
        get { return m_playingStage; }
        set { m_playingStage = value; }
    }

    #region シングルトンの実装
    private static GameStatus m_instance;
    /// <summary>
    /// GameStatusのインスタンスを取得する
    /// </summary>
    public static GameStatus GetInstance
    {
        get
        {
            if (m_instance == null)
            {
                var obj = new GameObject("GameStatus");
                m_instance = obj.AddComponent<GameStatus>();
            }
            return m_instance;
        }
    }


    /// <summary>
    /// Awakeメソッドでシングルトンインスタンスを設定する
    /// </summary>
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }

    }
    #endregion


    private void Initialize()
    {
        // 初期化
        m_currentID = m_startId;
    }
}
