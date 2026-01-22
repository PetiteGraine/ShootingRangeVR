using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour
{
    [Header("Gameplay Settings")]
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private int _maxTargets = 3;
    public int MaxTargets => _maxTargets;
    public int CurrentTargets { get; private set; } = 0;
    public List<Gun> PlayerGuns = new List<Gun>();

    private int _currentScore;
    private int _highScore = 0;
    public event Action<int> OnScoreChanged;
    private static GameplayManager _instance;

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
        OnScoreChanged?.Invoke(_currentScore);
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
        OnScoreChanged?.Invoke(_currentScore);
    }

    public void UpdateHighScore()
    {
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            if (_highScoreText != null)
            {
                _highScoreText.text = "High Score: " + _highScore.ToString("D6");
            }
        }
    }

    public void RegisterTargetSpawn()
    {
        CurrentTargets++;
    }

    public void RegisterTargetDespawn()
    {
        CurrentTargets--;
        if (CurrentTargets < 0) CurrentTargets = 0;
    }
}