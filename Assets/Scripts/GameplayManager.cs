using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private static GameplayManager _instance;
    private int _currentScore;
    private int _highScore = 0;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    public static GameplayManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameplayManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameplayManager>();
                    singletonObject.name = typeof(GameplayManager).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    public void AddScore(int points)
    {
        _currentScore += points;
        _scoreText.text = "Score: " + _currentScore.ToString("D6");
        UpdateHighScore();
    }

    public int GetScore()
    {
        return _currentScore;
    }

    public void ResetScore()
    {
        if (_currentScore == 0) return;
        _currentScore = 0;
        _scoreText.text = "Score: " + _currentScore.ToString("D6");
    }

    public void UpdateHighScore()
    {
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            _highScoreText.text = "High Score: " + _highScore.ToString("D6");
        }
    }
}
