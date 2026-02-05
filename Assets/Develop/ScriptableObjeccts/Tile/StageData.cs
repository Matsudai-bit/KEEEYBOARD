using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData")]

public class StageData : ScriptableObject
{
    [Header("最下レイヤータイルマップ")]
    [SerializeField]
    GameObject baseTilemap;

    [Header("最上レイヤータイルマップ")]
    [SerializeField]
    GameObject topTilemap;

    [Header("制限時間")]
    [SerializeField]
    int m_timeLimit;


    public GameObject BaseTilemap
    {
        get { return baseTilemap; }
    }

    public GameObject TopTilemap
    {
        get { return topTilemap; }
    }

    public int TimeLimit
    {
        get { return m_timeLimit; }
    }

}
