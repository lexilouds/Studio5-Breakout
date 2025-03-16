using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class ScoreCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI current;
    [SerializeField] private TextMeshProUGUI toUpdate;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Transform scoreTextContainer;
    [SerializeField] private float durationCount = 0.4f;
    [SerializeField] private float pulseCount = 0.2f;
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private Ease numberCurve;
    [SerializeField] private Ease textCurve;

    private float containerInitPosition;
    private float moveAmount;

    void Start()
    {
        Canvas.ForceUpdateCanvases();
        current.SetText("0");
        toUpdate.SetText("0");
        containerInitPosition = scoreTextContainer.localPosition.y;
        moveAmount = current.rectTransform.rect.height;
    }


    public void UpdateScore(int score)
    {
        toUpdate.SetText($"{score}");
        scoreTextContainer.DOLocalMoveY(containerInitPosition + moveAmount, durationCount).SetEase(numberCurve);
        Sequence pulseSequence = DOTween.Sequence();
        pulseSequence.Append(scoreText.transform.DOScale(Vector3.one*scaleFactor, pulseCount).SetEase(textCurve));
        pulseSequence.Append(scoreText.transform.DOScale(Vector3.one, pulseCount).SetEase(textCurve));
        StartCoroutine(ResetScoreContainer(score));
    }

    private IEnumerator ResetScoreContainer(int score)
    {
        yield return new WaitForSeconds(durationCount);
        current.SetText($"{score}");
        Vector3 localPosition = scoreTextContainer.localPosition;
        scoreTextContainer.localPosition = new Vector3(localPosition.x, containerInitPosition, localPosition.z);
    }
}
