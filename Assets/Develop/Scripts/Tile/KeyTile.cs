using UnityEngine;
using UnityEngine.UIElements;

public class KeyTile : MonoBehaviour
{
    [SerializeField]
    GameTile m_gameTile;

    float m_angle;
    private void Awake()
    {
        m_gameTile = gameObject?.GetComponent<GameTile>();
        if (m_gameTile)
        {
            Debug.LogError("m_gameTileがnullです");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_angle += Time.deltaTime * 100.0f;

        // 指定した角度（度数法）をクォータニオンに変換し、行列を作る
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, m_angle), Vector3.one);

        // 特定の座標のタイルの表示行列を上書きする
        m_gameTile.Tilemap.SetTransformMatrix(m_gameTile.CellPosition, matrix);
    }
}
