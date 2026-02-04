using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoxUI 
    : MonoBehaviour
    , IPlayerEventObserver

{
    [SerializeField]
    private GameObject m_keyUIPrefab;

    List<GameObject> m_keys = new();

    private void Awake()
    {
        PlayerEventMessenger.GetInstance.RegisterObserver(this);
    }
    private void OnDestroy()
    {
        PlayerEventMessenger.GetInstance.RemoveObserver(this);
    }

    void AddKey()
    {
        var startPosition = m_keyUIPrefab.GetComponent<RectTransform>().anchoredPosition;

        var spriteObject = Instantiate(m_keyUIPrefab, transform);
        spriteObject.GetComponent<RectTransform>().anchoredPosition = new(startPosition.x + 15.0f * (float)(m_keys.Count), startPosition.y);
        m_keys.Add(spriteObject);
    }

    void RemoveKey()
    {
        Destroy(m_keys.Last());
        m_keys.RemoveAt(m_keys.Count - 1);
    }

    public void OnEvent(PlayerEventID eventID, GameTile playerTile)
    {
        switch(eventID)
        {
            case PlayerEventID.GET_KEY:
                AddKey();
                break;
            case PlayerEventID.UNLOCK:
                RemoveKey();
                break;
        }
    }
}
