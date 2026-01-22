using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float _lifeTime = 5f;
    private Rigidbody _rigidbody;
    private TrailRenderer _particleSystem;
    private Coroutine _disableCoroutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _particleSystem = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        _disableCoroutine = StartCoroutine(DisableAfterDelay());
        _particleSystem.enabled = true;
    }

    private void OnDisable()
    {
        if (_disableCoroutine != null)
            StopCoroutine(_disableCoroutine);
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            collision.gameObject.GetComponent<DestroyTarget>().DestroyGameObject();
            this.gameObject.SetActive(false);
            GameplayManager.Instance.AddScore(1);
            return;
        }

        _rigidbody.useGravity = true;
        _particleSystem.enabled = false;
    }
}
