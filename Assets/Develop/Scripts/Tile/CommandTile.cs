using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class CommandTile : MonoBehaviour
{
    [Serializable]
    public enum State
    {
        DEFAULT,    // í èÌ
        MOVABLE,    // à⁄ìÆåÛï‚
        VISITED,    // í âﬂçœÇ›
    }

    [SerializeField]
    private State m_state;

    private GameTile m_gameTile;

    [SerializeField]
    private CommandSpriteData m_spriteData;

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

    public GameTile GameTile
    {
        get { return m_gameTile; }
    }

    public CommandSpriteData SpriteData
    {
        get { return m_spriteData; }
        set { m_spriteData = value; }
    }

    public State GetState()
    { return m_state; }

    public void SetState(State value)
    { 
      
        if (m_state != value)
        {
            m_state = value;
            m_gameTile.ChangeSprite(m_spriteData.GetSprite(Key, m_state));
        }
    
    }

}
