using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
[CreateAssetMenu(fileName = "GradeData", menuName = "ScriptableObjects/GradeData")]

public class GradeData : ScriptableObject
{
    [SerializeField]
    private SoundID soundID;


    public SoundID BgmID
    {
        get { return soundID; }
        set { soundID = value; }
    }
}
