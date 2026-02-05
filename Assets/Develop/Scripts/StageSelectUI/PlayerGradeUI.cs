using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGradeUI : MonoBehaviour
{
    [SerializeField]
    private Image m_myImage;

    [SerializeField]
    private List<Sprite> m_gradeUI;

    private void Awake()
    {
        m_myImage = GetComponent<Image>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int index = (int)GameContext.GetInstance.GetSaveData().playerGrade;
        m_myImage.sprite = m_gradeUI[index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
