using UnityEngine;

public class StageSelectManager : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField]
    private UnityEngine.CanvasGroup Boards;

    [SerializeField]
    ContentsController contentsController;
    [SerializeField]
    StageSlideController stageSlideController;


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
    }


    private SelectionState currentState = SelectionState.GRADE_SELECTION; // 現在の選択状態

    private int currentGradeIndex = 0; // 現在の階級インデックス
    private int currentStageIndex = 0; // 現在のステージインデックス


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == SelectionState.GRADE_SELECTION)
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
        // 入力に応じてcurrentGradeIndexを更新(左右キー)
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentGradeIndex == 2) return; // 3つ目の階級以上には進めない
            ChangeGradeIndex(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentGradeIndex == 0) return; // 1つ目の階級以下には戻れない
            ChangeGradeIndex(-1);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            // ステージ選択へ移行
            currentState = SelectionState.STAGE_SELECTION;
            StageID stageID = (StageID)(currentGradeIndex * 3 + currentStageIndex);
            contentsController.ViewOutLine(stageID);
            stageSlideController.SlideIn((StageGrade)currentGradeIndex, (StageNumber)currentStageIndex);
        }

    }


    // ステージ選択の処理
    private void HandleStageSelection()
    {
        // 入力に応じてcurrentStageIndexを更新(左右キー)
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(currentStageIndex == 2) return; // 3つ目のステージ以上には進めない
            ChangeStageIndex(1);
            stageSlideController.SlideIn((StageGrade)currentGradeIndex, (StageNumber)currentStageIndex);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentStageIndex == 0) return; // 1つ目のステージ以下には戻れない
            ChangeStageIndex(-1);
            stageSlideController.SlideIn((StageGrade)currentGradeIndex, (StageNumber)currentStageIndex);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            // 階級選択へ戻る
            currentState = SelectionState.GRADE_SELECTION;
            // ステージインデックスをリセット
            currentStageIndex = 0;
            contentsController.HideOutLine();
            stageSlideController.SlideOut();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ステージ確定の処理(階級 * 3 + ステージ番号)
            StageID selectedStage = (StageID)(currentGradeIndex * 3 + currentStageIndex);
            Debug.Log("Selected Stage: " + selectedStage);
        }

    }



    // 階級インデックスの変更
    private void ChangeGradeIndex(int add)
    {
        currentGradeIndex += add;
        currentGradeIndex = Mathf.Clamp(currentGradeIndex, 0, 2); // GRADE1からGRADE3まで

        // 階級が変更された場合の処理
        Boards.GetComponent<BoardsController>().SlideBoard((StageGrade)currentGradeIndex);

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
}
