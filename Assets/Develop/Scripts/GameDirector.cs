using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector 
    : MonoBehaviour
    , IPlayerEventObserver
{
    void Awake()
    {
        PlayerEventMessenger.GetInstance.RegisterObserver(this);

    }

    void OnDestroy()
    {
        PlayerEventMessenger.GetInstance.RemoveObserver(this);
    }


    [SerializeField]
    private int m_lockNum; // ÉçÉbÉN

    public void OnEvent(PlayerEventID eventID, GameTile playerTile)
    {
        switch (eventID)
        {
            case PlayerEventID.UNLOCK:
                UnlockDoor();
                break;
        }
    }

    void UnlockDoor()
    {
        m_lockNum--;

        if (m_lockNum <= 0)
        {
            SceneManager.LoadScene("Result");
        }
    }
   public void AddLockDoor()
    {
        m_lockNum++;
    }
}
