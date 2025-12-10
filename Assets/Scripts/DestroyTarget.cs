using UnityEngine;

public class DestroyTarget : MonoBehaviour
{
    [SerializeField] private float _delayTime = 0.25f;
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
        _hitEffect.GetComponent<FractureEffect>().BreakObject();
        this.gameObject.SetActive(false);
        Invoke(nameof(ChangeRandomPosition), _delayTime);
        AudioSource.PlayClipAtPoint(_audioClip, this.transform.position);
    }

    private void ChangeRandomPosition()
    {
        this.gameObject.SetActive(true);
        _hitEffect.SetActive(false);
        _hitEffect.GetComponent<FractureEffect>().Reset();
        Vector3 newPosition = _spawner.GetComponent<Spawner>().RandomPosition();
        this.transform.parent.position = newPosition;
    }
}
