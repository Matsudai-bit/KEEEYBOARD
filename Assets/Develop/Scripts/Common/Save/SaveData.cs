using System;
using System.Collections.Generic;

/// <summary>
/// セーブデータ用ScriptableObject
/// </summary
[Serializable]
public class SaveData 
{
    /// <summary>
    /// JSON保存用構造体
    /// </summary>
    public class JsonSaver
    {
        public GameStage.GradeID playerGrade;
        public List<GradeData> worldDataList;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SaveData()
    {
        Initialize();
    }

    /// <summary>
    /// ステージクリアデータ
    /// </summary>
    [System.Serializable]
    public class StageData
    {
        public string      stageID; // ステージID
        public StageStatus  stageStatus; // ステージステータス
    }

    /// <summary>
    /// ステージステータス
    /// </summary>
    [System.Serializable]
    public class StageStatus
    {
        public bool isClear;   // クリアしたかどうか
        public bool isLocked = true;  // アンロックされているかどうか
        public float clearTime; // クリアタイム
    }

    /// <summary>
    /// プレイヤーのグレード
    /// </summary>
    public GameStage.GradeID playerGrade;


    /// <summary>
    /// グレードデータ
    /// </summary>
    [System.Serializable]
    public class GradeData
    {
        public string gradeID;      // グレードID
        public bool isLocked = true;              // グレードがロックされているかどうか
        public List<StageData> stageDataList; // JSON保存用リスト
    }

    // グレードデータ辞書
    
    public Dictionary<string, GradeData> gradeDataDict = new Dictionary<string, GradeData>();


    /// <summary>
    /// JSON保存用構造体に変換
    /// </summary>
    /// <returns></returns>
    public JsonSaver ConvertJsonSaver()
    {
        JsonSaver jsonSaver = new();

        jsonSaver.playerGrade = playerGrade;

        jsonSaver.worldDataList = new List<GradeData>();

        jsonSaver.worldDataList.Clear();
        foreach (var worldData in gradeDataDict)
        {
            
            jsonSaver.worldDataList.Add(worldData.Value);
        }
        return jsonSaver;
    }

    /// <summary>
    /// JSON保存用構造体から読み込み
    /// </summary>
    /// <param name="jsonSaver"></param>
    public void LoadFromJsonSaver(JsonSaver jsonSaver)
    {
        playerGrade = jsonSaver.playerGrade;
        foreach (var worldData in jsonSaver.worldDataList)
        {
            gradeDataDict[worldData.gradeID] = worldData;
        }
    }


    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        // グレードデータとステージクリアデータの初期化
        for (int i = 0; i < Enum.GetValues(typeof(GameStage.GradeID)).Length; i++)
        {
            // グレードデータの初期化
            GameStage.GradeID gradeID = (GameStage.GradeID)i;
            GradeData worldData = new GradeData();
            worldData.gradeID = gradeID.ToString();
            worldData.stageDataList = new();
            worldData.isLocked = true;

            // ステージクリアデータの初期化
            for (int j = 0; j < Enum.GetValues(typeof(GameStage.StageID)).Length; j++)
            {
                GameStage.StageID stageID = (GameStage.StageID)j;
                StageData stageClearData = new StageData();
                stageClearData.stageID = stageID.ToString();
                stageClearData.stageStatus = new StageStatus();
                stageClearData.stageStatus.isClear = false;
                stageClearData.stageStatus.isLocked = true;
                stageClearData.stageStatus.clearTime = 0.0f;
                worldData.stageDataList.Add(stageClearData);
            }
            gradeDataDict[gradeID.ToString()] = worldData;
        }

        gradeDataDict[GameStage.GradeID.GRADE_1.ToString()].isLocked = false;
        GetStageStatus(GameStage.GradeID.GRADE_1, GameStage.StageID.STAGE_1).isLocked = false;
    }

    /// <summary>
    /// ステージのステータスを取得
    /// </summary>
    /// <param name="worldID"></param>
    /// <param name="stageID"></param>
    /// <returns></returns>
    public StageStatus GetStageStatus(GameStage.GradeID worldID, GameStage.StageID stageID)
    {
        return gradeDataDict[worldID.ToString()].stageDataList[(int)stageID].stageStatus;
    }
}
