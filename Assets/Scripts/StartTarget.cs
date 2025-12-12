using UnityEngine;

public class StartTarget : MonoBehaviour
{
    [SerializeField] private Countimer _countimer;
    [SerializeField] private Spawner _spawner;
    private AudioClip _audioClip;
    private TargetSoundManager _targetSoundManager;

    private void Awake()
    {
        _targetSoundManager = FindFirstObjectByType<TargetSoundManager>();
        _audioClip = _targetSoundManager.GetClip();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (_audioClip == null)
            {
                _targetSoundManager = FindFirstObjectByType<TargetSoundManager>();
                _audioClip = _targetSoundManager.GetClip();
            }
            AudioSource.PlayClipAtPoint(_audioClip, this.transform.position);

            _spawner.StartNewGame();
            StartGame();
            collision.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            return;
        }
    }

    private void StartGame()
    {
        GameplayManager.Instance.ResetScore();
        _countimer.BeginCountimer();
    }
}
