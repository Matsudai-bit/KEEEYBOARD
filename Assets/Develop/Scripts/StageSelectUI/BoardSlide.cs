using UnityEngine;
using DG.Tweening;

public class BoardSlideController : MonoBehaviour
{
    private Vector2 startPosition = new Vector2(999, 999);

    private void Awake()
    {
        startPosition = transform.position;
    }

    public void SlideBoard(StageSelectManager.StageGrade currentGrade)
    {

        Vector3 targetPosition = transform.position;

        targetPosition.x = startPosition.x + (-1920f * (int)currentGrade);

        // スライドアニメーションを実行
        transform.DOMoveX(targetPosition.x, 0.5f).SetEase(Ease.InOutSine);

    }
}