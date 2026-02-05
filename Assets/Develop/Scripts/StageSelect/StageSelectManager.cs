using System;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;
using UnityEngine.InputSystem;

public class StageSelectManager : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField]
    private UnityEngine.CanvasGroup Boards;

    [SerializeField]
    ContentsController contentsController;
    [SerializeField]
    StageSlideController stageSlideController;
    [SerializeField]
    LightController lightController;

    [Header("シーンのフェードインエフェクト")]
    [SerializeField]
    SceneTransitionEffect m_sceneFadeInEffect;
    [Header("シーンのフェードアウトエフェクト")]
    [SerializeField]
    SceneTransitionEffect m_sceneFadeOutEffect;

    [SerializeField]
    GameObject m_arrowRight;

    [SerializeField]
    GameObject m_arrowLeft;

    // 階級を示す列挙
    public enum StageGrade
    {
        GRADE1,
        GRADE2,
        GRADE3,
    }

    public enum StageNumber
    {
        STAGE_1,
        STAGE_2,
        STAGE_3,
    }

    // ステージを示す列挙
    public enum StageID
    {
        // GRADE1
        STAGE1_1,
        STAGE1_2,
        STAGE1_3,
        // GRADE2
        STAGE2_1,
        STAGE2_2,
        STAGE2_3,
        // GRADE3
        STAGE3_1,
        STAGE3_2,
        STAGE3_3,
    }

    // 選択状態を示す列挙
    private enum SelectionState
    {
        GRADE_SELECTION,    // 階級を選択中
        STAGE_SELECTION,    // ステージを選択中
        RESULT_MODE         // リザルトモード
    }


    private SelectionState currentState = SelectionState.GRADE_SELECTION; // 現在の選択状態

    private int currentGradeIndex = 0; // 現在の階級インデックス
    private int currentStageIndex = 0; // 現在のステージインデックス


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_arrowLeft.SetActive(false);
        m_arrowRight.SetActive(false);
        stageSlideController.Initialize();

        m_sceneFadeInEffect.StartTransition(() => {
            if (GameStatus.GetInstance.CurrentStateID == GameStatus.ID.RESULT_MODE)
            {

                currentState = SelectionState.RESULT_MODE;

                currentGradeIndex = 0;
                currentGradeIndex = 0;

                ChangeGradeIndex((int)GameStatus.GetInstance.CurrentPlayingStage.gradeID, () => {
                    InToStageSelect(currentGradeIndex, currentStageIndex);
                    ShowStageInformation(currentGradeIndex, currentStageIndex, true);
                });
                ChangeStageIndex((int)GameStatus.GetInstance.CurrentPlayingStage.stageID);

            }
        });
       
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == SelectionState.RESULT_MODE && !stageSlideController.IsAnimation())
        {
            currentState = SelectionState.STAGE_SELECTION;
            // ステージインの処理
            contentsController.ViewOutLine(GetStageID(currentGradeIndex, currentStageIndex));
        }
        else if(currentState == SelectionState.GRADE_SELECTION)
        {
            // 階級選択中の処理
            HandleGradeSelection();
        }
        else if (currentState == SelectionState.STAGE_SELECTION)
        {
            // ステージ選択中の処理
            HandleStageSelection();
        }
    }




    // 階級選択の処理
    private void HandleGradeSelection()
    {
        m_arrowLeft.SetActive(currentGradeIndex > 0);
        m_arrowRight.SetActive(currentGradeIndex < 2);

    }

    StageID GetStageID(int gradeIndex, int stageIndex)
    {
        return (StageID)(gradeIndex * 3 + stageIndex);
    }

    /// <summary>
    /// ステージセレクトに入る
    /// </summary>
    private void InToStageSelect(int gradeIndex, int stageIndex)
    {


        // ステージ選択へ移行
        //currentState = SelectionState.STAGE_SELECTION;
        StageID stageID = GetStageID(gradeIndex, stageIndex);
        // ステージインの処理
        contentsController.ViewOutLine(stageID);
        lightController.PlayFlicker();
    }

    private void ShowStageInformation(int gradeIndex, int stageIndex, bool completeAnimation = false)
    {

        var stageStatus = GetStageStatus(gradeIndex, stageIndex);
       
        stageSlideController.SlideIn((StageGrade)gradeIndex, (StageNumber)stageIndex, stageStatus.isClear, completeAnimation);
    }

    // ステージ選択の処理
    private void HandleStageSelection()
    {

        m_arrowLeft.SetActive(false);
        m_arrowRight.SetActive(false);




    }



    // 階級インデックスの変更
    private void ChangeGradeIndex(int add, Action animationEndAction)
    {
        currentGradeIndex += add;
        currentGradeIndex = Mathf.Clamp(currentGradeIndex, 0, 2); // GRADE1からGRADE3まで

        // 階級が変更された場合の処理
        Boards.GetComponent<BoardsController>().SlideBoard((StageGrade)currentGradeIndex, animationEndAction);

    }

    // ステージインデックスの変更
    private void ChangeStageIndex(int add)
    {
        currentStageIndex += add;
        currentStageIndex = Mathf.Clamp(currentStageIndex, 0, 2); // 各階級に3つのステージ

        // ステージが変更された場合の処理
        // ステージIDを計算
        StageID stageID = (StageID)(currentGradeIndex * 3 + currentStageIndex);
        contentsController.ViewOutLine(stageID);

    }

    public static void ConvertGradeIDAndStageID(int gradeIndex, int stageIndex, out GameStage.GradeID gradeID, out GameStage.StageID stageID)
    {
        var gradeData = (GameStage.GradeID[])Enum.GetValues(typeof(GameStage.GradeID));
        var stageData = (GameStage.StageID[])Enum.GetValues(typeof(GameStage.StageID));

        gradeID = gradeData[gradeIndex];
        stageID = stageData[stageIndex];

    }

    public static void ConvertGradeIDAndStageID(StageGrade stageGrade, StageNumber stage, out GameStage.GradeID gradeID, out GameStage.StageID stageID)
    {
        var gradeData = (GameStage.GradeID[])Enum.GetValues(typeof(GameStage.GradeID));
        var stageData = (GameStage.StageID[])Enum.GetValues(typeof(StageID));

        gradeID = gradeData[(int)stageGrade];
        stageID = stageData[(int)stage];

    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed ) { return; }

        if (currentState == SelectionState.GRADE_SELECTION)
        { 
            InToStageSelect(currentGradeIndex, currentStageIndex);
            ShowStageInformation(currentGradeIndex, currentStageIndex);
            currentState = SelectionState.STAGE_SELECTION;
            
        }
        else if (currentState == SelectionState.STAGE_SELECTION)
        {
            if (GetStageStatus(currentGradeIndex, currentStageIndex).isLocked) { return; }

            // ステージ確定の処理(階級 * 3 + ステージ番号)
            StageID selectedStage = (StageID)(currentGradeIndex * 3 + currentStageIndex);
            Debug.Log("Selected Stage: " + selectedStage);

            GameStage.GradeID gradeID;
            GameStage.StageID stageID;
            ConvertGradeIDAndStageID(currentGradeIndex, currentStageIndex, out gradeID, out stageID);

            GameStatus.GetInstance.CurrentPlayingStage = new(gradeID, stageID);

            SceneTransitionManager.GetInstance.TransitionToScene("InGame", m_sceneFadeOutEffect);

        }
    }


    public void OnEscape(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        
        if (currentState == SelectionState.STAGE_SELECTION)
        {
            // 階級選択へ戻る
            currentState = SelectionState.GRADE_SELECTION;
            // ステージインデックスをリセット
            currentStageIndex = 0;
            // ステージアウトの処理
            contentsController.HideOutLine();
            stageSlideController.SlideOut();
            lightController.StopFlicker();
        }

    
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        var axis = context.ReadValue<Vector2>();
        if (Mathf.Approximately(axis.magnitude, 0.0f)) { return; }
        bool right = (axis.x > 0.0f) ? true : false;

        // 入力に応じてcurrentStageIndexを更新(左右キー)
        if (currentState == SelectionState.STAGE_SELECTION)
        {

            if (right)
            {
                if (currentStageIndex == 2) return; // 3つ目のステージ以上には進めない
                ChangeStageIndex(1);
                ShowStageInformation(currentGradeIndex, currentStageIndex);
                // ステージインの処理
                contentsController.ViewOutLine(GetStageID(currentGradeIndex, currentStageIndex));


            }

            if (!right)
            {
                if (currentStageIndex == 0) return; // 1つ目のステージ以下には戻れない
                ChangeStageIndex(-1);
                ShowStageInformation(currentGradeIndex, currentStageIndex);
                contentsController.ViewOutLine(GetStageID(currentGradeIndex, currentStageIndex));

            }
        }

        else if (currentState == SelectionState.GRADE_SELECTION)
        {
            // 入力に応じてcurrentGradeIndexを更新(左右キー)
            if (right)
            {
                if (currentGradeIndex == 2) return; // 3つ目の階級以上には進めない
                ChangeGradeIndex(1, () => { });
            }
            if (!right)
            {
                if (currentGradeIndex == 0) return; // 1つ目の階級以下には戻れない
                ChangeGradeIndex(-1, () => { });
            }

        }

    }


    SaveData.StageStatus GetStageStatus(int gradeIndex, int stageIndex)
    {
        GameStage.GradeID gradeID;
        GameStage.StageID stageID;
        ConvertGradeIDAndStageID(gradeIndex, stageIndex, out gradeID, out stageID);
        return GameContext.GetInstance.GetSaveData().GetStageStatus(gradeID, stageID);
    }
}
