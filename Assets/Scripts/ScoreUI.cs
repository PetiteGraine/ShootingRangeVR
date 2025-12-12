using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("Score UI Setup")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private float _refreshInterval = 0.15f;
    private int _displayedScore;
    private int _targetScore;

    private void Start()
    {
        GameplayManager.Instance.OnScoreChanged += UpdateTargetScore;
        _displayedScore = GameplayManager.Instance.GetScore();
        _targetScore = _displayedScore;
        StartCoroutine(RefreshScoreDisplay());
    }

    private void OnDestroy()
    {
        if (GameplayManager.Instance != null)
            GameplayManager.Instance.OnScoreChanged -= UpdateTargetScore;
    }

    private void UpdateTargetScore(int newScore)
    {
        _targetScore = newScore;
    }

    private IEnumerator RefreshScoreDisplay()
    {
        while (true)
        {
            if (_displayedScore != _targetScore)
            {
                _displayedScore = _targetScore;
                _scoreText.text = "Score: " + _displayedScore.ToString("D6");
            }
            yield return new WaitForSeconds(_refreshInterval);
        }
    }
}