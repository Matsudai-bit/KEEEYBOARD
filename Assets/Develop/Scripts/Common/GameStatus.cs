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

    [SerializeField]
    private ID m_startId;

    private ID m_currentID;

    public ID CurrentStateID
    {
        get { return m_currentID; }
        set { m_currentID = value; }
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

    #endregion

    /// <summary>
    /// Awakeメソッドでシングルトンインスタンスを設定する
    /// </summary>
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);

            // 初期化
            m_currentID = m_startId;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
