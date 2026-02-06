using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector 
    : MonoBehaviour
    , IPlayerEventObserver
{
    [Header("シーンのフェードインエフェクト")]
    [SerializeField]
    SceneTransitionEffect m_sceneFadeInEffect;
    [Header("シーンのフェードアウトエフェクト")]
    [SerializeField]
    SceneTransitionEffect m_sceneFadeOutEffect;
    [Header("シーンのフェードアウト(爆発)エフェクト")]
    [SerializeField]
    SceneTransitionEffect m_sceneFadeGameOVer;

    [SerializeField]
    private SpriteMultiLineTimer m_timer;
    void Awake()
    {
        PlayerEventMessenger.GetInstance.RegisterObserver(this);
    }

    void OnDestroy()
    {
        PlayerEventMessenger.GetInstance.RemoveObserver(this);
  

    }

    void Start()
    {
        m_sceneFadeInEffect.StartTransition(() => {
            m_timer.StartTimer();
        });
    }


    [SerializeField]
    private int m_lockNum; // ロック

    public void OnEvent(PlayerEventID eventID, GameTile playerTile)
    {
        switch (eventID)
        {
            case PlayerEventID.UNLOCK:
                UnlockDoor();
                SoundManager.GetInstance.RequestPlaying(SoundID.SE_INGAME_PLAYER_UNLOCK);
                break;
            case PlayerEventID.GET_KEY:
                SoundManager.GetInstance.RequestPlaying(SoundID.SE_INGAME_PLAYER_GET_KEY);
                break;
        }
    }

    void Update()
    {
        if (m_timer.GetTime() <= 0.0f && !m_sceneFadeGameOVer.IsTransitioning())
        {
            SceneTransitionManager.GetInstance.TransitionToScene("StageSelect", m_sceneFadeGameOVer);

        }
    }

    void UnlockDoor()
    {
        m_lockNum--;

        if (m_lockNum <= 0)
        {
            var gradeID = GameStatus.GetInstance.CurrentPlayingStage.gradeID;
            var stageID = GameStatus.GetInstance.CurrentPlayingStage.stageID;
            GameStatus.GetInstance.CurrentStateID = GameStatus.ID.RESULT_MODE;
            GameContext.GetInstance.TryUnlockGrade(gradeID);
            GameContext.GetInstance.GetSaveData().GetStageStatus(gradeID, stageID).isClear = true;
            GameContext.GetInstance.GetSaveData().GetStageStatus(gradeID, stageID).clearTime = m_timer.GetDuration() - m_timer.GetTime();
            GameContext.GetInstance.SaveGame();
            SceneTransitionManager.GetInstance.TransitionToScene("StageSelect", m_sceneFadeOutEffect);
        }
    }
   public void AddLockDoor()
    {
        m_lockNum++;
    }
}
