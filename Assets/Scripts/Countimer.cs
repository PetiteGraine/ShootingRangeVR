using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Countimer : MonoBehaviour
{
    [Header("Countimer")]
    [SerializeField] private TextMeshProUGUI _countimerText;
    [SerializeField] private GameObject _startTarget;
    [SerializeField] private GameObject _spawner;
    private bool _isTimerOn = false;
    private TimeSpan _timePlaying;
    private float _remainingTime;
    private float _startTime = 5f;

    private void Start()
    {
        _countimerText.text = "Time: " + TimeSpan.FromSeconds(_startTime).ToString("mm':'ss'.'ff");
        _remainingTime = _startTime;
    }

    public void BeginCountimer()
    {
        _isTimerOn = true;
        _remainingTime = _startTime;
        StartCoroutine(UpdateTimer());
    }

    public void EndCountimer()
    {
        StopCoroutine(UpdateTimer());
        _isTimerOn = false;
        _startTarget.SetActive(true);
        _spawner.GetComponent<Spawner>().DisableAllTargets();
    }

    private IEnumerator UpdateTimer()
    {
        while (_isTimerOn)
        {
            _remainingTime -= Time.deltaTime;
            _timePlaying = TimeSpan.FromSeconds(_remainingTime);
            string timePlayingStr = "Time: " + _timePlaying.ToString("mm':'ss'.'ff");
            _countimerText.text = timePlayingStr;
            if (_remainingTime <= 0)
            {
                EndCountimer();
                _isTimerOn = false;
                _countimerText.text = "Time: 00:00.00";
            }
            yield return null;
        }
    }
}
