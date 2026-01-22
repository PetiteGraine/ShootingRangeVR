using UnityEngine;

public class DestroyTarget : MonoBehaviour
{
    [Header("Destroy Target Settings")]
    [SerializeField] private float _delayTime = 0.20f;
    [SerializeField] private GameObject _spawner;
    [SerializeField] private GameObject _hitEffect;

    private AudioClip _audioClip;
    private TargetSoundManager _targetSoundManager;

    private void Awake()
    {
        _targetSoundManager = FindFirstObjectByType<TargetSoundManager>();
        _audioClip = _targetSoundManager.GetClip();
    }

    public void DestroyGameObject()
    {
        if (_audioClip == null)
        {
            _targetSoundManager = FindFirstObjectByType<TargetSoundManager>();
            _audioClip = _targetSoundManager.GetClip();
        }
        _hitEffect.SetActive(true);
        this.gameObject.SetActive(false);
        Invoke(nameof(ChangeRandomPosition), _delayTime);
        AudioSource.PlayClipAtPoint(_audioClip, this.transform.position);
    }

    private void ChangeRandomPosition()
    {
        this.gameObject.SetActive(true);
        _hitEffect.SetActive(false);
        Vector3 newPosition = Spawner.Instance.RandomPosition();
        this.transform.parent.position = newPosition;
    }
}
