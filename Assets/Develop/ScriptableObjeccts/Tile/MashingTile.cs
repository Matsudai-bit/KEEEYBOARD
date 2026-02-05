using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "2D/Tiles/Mash Tile")]

public class MashingTile : Tile
{
    [SerializeField]
    int m_mashingCount = 0;

    public int MashingCount
    {
        get { return m_mashingCount; }
        set { m_mashingCount = value; }
    }

    [SerializeField]
    Key m_key;

    public Key KeyCode
    {
        get { return m_key; }
        set { m_key = value; }
    }



}
