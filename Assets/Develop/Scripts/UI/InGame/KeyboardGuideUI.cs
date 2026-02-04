using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyboardGuideUI : MonoBehaviour
{
    [SerializeField]
    private KeyboardGuideSpriteData m_KeyboardGuideSpriteData;

    [SerializeField]
    private GameObject m_guideUIPrefab;

    private Dictionary<Key, Image>  m_usedKey = new();
    private List<Image>             m_ImagePool = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
 

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        foreach (var key in keyboard.allKeys)
        {
            if (key.wasPressedThisFrame && !m_usedKey.Any(item => item.Key == key.keyCode))
            {
                var guideSprite = m_KeyboardGuideSpriteData.GetSprite(key.keyCode);
                if (guideSprite)
                {
                    var image = m_ImagePool.Find(item => !item.gameObject.activeSelf);
                    if (!image)
                    {
                        var spriteObject = Instantiate(m_guideUIPrefab, transform);
                        image = spriteObject.GetComponent<Image>();
                        m_ImagePool.Add(image);

                    }
                    image.gameObject.SetActive(true);
                    image.sprite = guideSprite;

                    m_usedKey.Add(key.keyCode, image);
                    
                }

            }
            if (key.wasReleasedThisFrame && m_usedKey.Any(item => item.Key == key.keyCode))
            {
                m_usedKey[key.keyCode].gameObject.SetActive(false);
                m_usedKey.Remove(key.keyCode);
            }

        }
    }
}
