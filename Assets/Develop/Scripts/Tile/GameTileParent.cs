using UnityEngine;

public class GameTileParent : MonoBehaviour
{
    [SerializeField]
    GameTile  gameTile;
    [SerializeField]
    GameObject child;

    public GameTile GameTile
    {
        get { return gameTile; }
    }

    public GameObject Child
    {
        get { return child; }
    }

    private void Awake()
    {
        if (!gameTile)
            gameTile = GetComponentInChildren<GameTile>();
        if (!child)
            child = transform.GetChild(0).gameObject;

        if (!gameTile)
            Debug.LogError("gameTile Null");
        if (!child)
            Debug.LogError("child Null");
    }

    public void SetChild(GameTile gameTile, GameObject child)
    {
        this.gameTile = gameTile;
        this.child = child;

        this.child.transform.SetParent(transform);
    }
}
