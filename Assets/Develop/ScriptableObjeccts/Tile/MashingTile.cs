using UnityEngine;
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

  

}
