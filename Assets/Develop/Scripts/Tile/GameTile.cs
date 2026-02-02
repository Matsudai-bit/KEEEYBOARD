using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField]
    Vector3Int m_position; // 座標

    /// <summary>
    /// 座標のゲッターセッター
    /// </summary>
    public Vector3Int Position
    {
        get { return m_position; }
        set { m_position = value; }
    }
}
