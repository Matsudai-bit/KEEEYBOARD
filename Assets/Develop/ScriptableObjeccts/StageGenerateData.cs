using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "StageGenerateData", menuName = "ScriptableObjects/StageGenerateData")]

public class StageGenerateData : ScriptableObject
{
    [Header("グレードデータ")]
    [SerializeField]
    private GradeData gradeData;

    [Header("ステージデータ")]
    [SerializeField]
    private StageData stageData;

   public GradeData GenerationGradeData
    {
        get { return gradeData; }
    }

   public StageData GenerationStageData
    {
        get { return stageData; }
    }
}
