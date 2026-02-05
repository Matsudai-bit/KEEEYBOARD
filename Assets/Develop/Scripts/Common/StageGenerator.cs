using UnityEngine;

/// <summary>
/// ステージ生成器
/// </summary>
public class StageGenerator : MonoBehaviour
{
    [Header("グリッド")]
    [SerializeField]
    GameObject m_grid;

    [Header("最下レイヤータイルマップ")]
    [SerializeField]
    GameObject m_baseTilemap;

    [Header("最上レイヤータイルマップ")]
    [SerializeField]
    GameObject m_topTilemap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(m_baseTilemap, m_grid.transform);
        Instantiate(m_topTilemap, m_grid.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
