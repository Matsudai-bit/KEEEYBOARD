using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class CommandTile : MonoBehaviour
{
    private GameTile m_gameTile;

    [SerializeField]
    private Key m_key = Key.None; // ÉLÅ[

    private void Awake()
    {
        m_gameTile = gameObject?.GetComponent<GameTile>();
        if (!m_gameTile)
        {
            Debug.LogError("m_gameTileÇ™nullÇ≈Ç∑");
        }
    }

    public Key Key
    {
        get { return m_key; }
        set { m_key = value; }
    }


}
