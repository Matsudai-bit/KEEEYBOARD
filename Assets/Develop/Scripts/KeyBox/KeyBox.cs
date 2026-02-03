using UnityEngine;

public class KeyBox 
    : MonoBehaviour
{
    [SerializeField]
    int m_keyCount; // Œ®‚Ì”


    public int KeyCount
    {
        get { return m_keyCount; }
    }

    public void StoreKey()
    {
        m_keyCount++;
    }

    public void RestoreKey()
    {
        m_keyCount--;
    }

}
