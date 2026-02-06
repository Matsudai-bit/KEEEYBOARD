using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour
{
    [Header("グリッド")]
    [SerializeField]
    GameObject m_grid;

    [Header("ステージ生成デ―タ")]
    [SerializeField]
    private StageGenerateData m_stageGenerateData;

    [Header("ステージのタイルマップペア設定")]
    [SerializeField]
    StageData m_tilemapPair;


    [Header("ステージグレードデータ")]
    [SerializeField]
    GradeData m_gradeData;


    [Header("時間")]
    [SerializeField]
    SpriteMultiLineTimer m_timer;

    [Header("ゲームタイル管理")]
    [SerializeField]
    GameTileManager m_gameTileManager;

 

    [SerializeField]
    List<StageGenerateData> m_stageGenerateDatabase = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        m_stageGenerateData = m_stageGenerateDatabase[GetIndex(GameStatus.GetInstance.CurrentPlayingStage.gradeID, GameStatus.GetInstance.CurrentPlayingStage.stageID)];

        m_tilemapPair = m_stageGenerateData.GenerationStageData;
        m_gradeData = m_stageGenerateData.GenerationGradeData;
        // タイマーに時間の設定
        m_timer.SetDuration(m_tilemapPair.TimeLimit);
       var baseTilemap  = Instantiate(m_tilemapPair.BaseTilemap   , m_grid.transform);
       var topTilemap = Instantiate(m_tilemapPair.TopTilemap    , m_grid.transform);

        m_gameTileManager.SetUpTile(baseTilemap.GetComponent<Tilemap>(), topTilemap.GetComponent<Tilemap>());


    }

    private void Start()
    {
        SoundManager.GetInstance.RequestAllStopping(true);
        SoundManager.GetInstance.PlayBGM(m_gradeData.BgmID);
        //        m_gameTileManager.SetUpTile();

    }

    public StageData TilemapPair
    {
        get { return m_tilemapPair; }
    }
    
    int GetIndex(GameStage.GradeID gradeID, GameStage.StageID stageID)
    {
        return (int)gradeID * Enum.GetNames(typeof(GameStage.StageID)).Length + (int)stageID ;
    }
}
