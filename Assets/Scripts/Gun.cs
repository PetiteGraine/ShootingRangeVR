using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Gun : MonoBehaviour
{
    [Header("Gun Setup")]
    [SerializeField] private GameObject _gunController;
    [SerializeField] private BulletPoolManager _bulletManager;
    [SerializeField] private GameObject _defaultGun;

    [Header("Settings")]
    [SerializeField] private float _fireSpeed = 50f;
    [SerializeField] private string _firePointKeyBase = "Guns/FirePoint";
    [SerializeField] private BulletSound _bulletSound;

    private Transform _firePoint;
    private ParticleSystem _muzzleFlash;

    private void Awake()
    {
        GameplayManager.Instance.PlayerGuns.Add(this);
    }

    private IEnumerator Start()
    {
        string suffix =
#if UNITY_ANDROID
        "_Quest";
#else
        "_PCVR";
#endif

        string fullKey = _firePointKeyBase + suffix;

        var handle = Addressables.LoadAssetAsync<GameObject>(fullKey);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject firePointObject = Instantiate(handle.Result, this._gunController.transform);
            _firePoint = firePointObject.transform;
            _muzzleFlash = _firePoint.GetComponentInChildren<ParticleSystem>();
            SetupGunModel();
        }
        else
        {
            Debug.LogError($"Failed to load FirePoint Addressable: {fullKey}");
        }

        Addressables.Release(handle);
    }

    private void SetupGunModel()
    {
        if (this._gunController.transform.childCount == 1)
        {
            GameObject currentGun = Instantiate(_defaultGun, this._gunController.transform);

            foreach (Transform t in currentGun.GetComponentsInChildren<Transform>(true))
            {
                if (t.CompareTag("FirePoint"))
                {
                    _firePoint.gameObject.transform.position = t.position;
                    _firePoint.gameObject.transform.rotation = t.rotation;
                    break;
                }
            }
        }
    }

    public void FireBullet(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireBulletFromPool();
            MuzzleFlash();
            AudioSource.PlayClipAtPoint(_bulletSound.GetRandomClip(), _firePoint.position);
        }
    }

    private void FireBulletFromPool()
    {
        if (_bulletManager == null)
        {
            Debug.LogError("Bullet Manager non assigné ou manquant.");
            return;
        }

        GameObject spawnedBullet = _bulletManager.GetBullet();

        if (spawnedBullet == null)
        {
            Debug.LogWarning("Pool Manager n'est pas prêt ou n'a pas pu fournir de balle.");
            return;
        }

        Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        spawnedBullet.transform.position = _firePoint.position;
        spawnedBullet.transform.rotation = _firePoint.rotation;
        spawnedBullet.SetActive(true);

        if (rb != null)
        {
            rb.linearVelocity = _firePoint.forward * _fireSpeed;
        }
    }

    private void MuzzleFlash()
    {
        if (_muzzleFlash != null)
            _muzzleFlash.Play();
    }

    public void ChangeGun(GameObject newGunPrefab)
    {
        if (newGunPrefab == null) return;

        if (this._gunController.transform.childCount > 1)
            Destroy(this._gunController.transform.GetChild(1).gameObject);

        GameObject newGun = Instantiate(newGunPrefab, this._gunController.transform);

        foreach (Transform t in newGun.GetComponentsInChildren<Transform>(true))
        {
            if (t.CompareTag("FirePoint"))
            {
                _firePoint.gameObject.transform.position = t.position;
                _firePoint.gameObject.transform.rotation = t.rotation;
                break;
            }
        }
    }
}