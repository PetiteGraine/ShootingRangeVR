using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject _defaultGun;
    [SerializeField] private GameObject _bullets_container;
    private List<GameObject> _pool = new List<GameObject>();
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _firePoint;
    private ParticleSystem _muzzleFlash;
    [SerializeField] private float _fireSpeed = 20f;

    private void Awake()
    {
        if (this.transform.childCount == 0)
        {
            GameObject currentGun = Instantiate(_defaultGun, this.transform);
            currentGun.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            foreach (Transform t in currentGun.GetComponentsInChildren<Transform>(true))
            {
                if (t.CompareTag("FirePoint"))
                {
                    _firePoint = t;
                    _muzzleFlash = t.GetComponentInChildren<ParticleSystem>();
                    break;
                }
            }
        }

        foreach (Transform t in _bullets_container.GetComponentsInChildren<Transform>(true))
        {
            _pool.Add(t.gameObject);
        }
    }

    public void FireBullet(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            BulletPoolingAndFire();
            MuzzleFlash();
        }
    }

    private void BulletPoolingAndFire()
    {
        GameObject spawnedBullet = null;

        foreach (var bullet in _pool)
        {
            if (!bullet.activeInHierarchy)
            {
                spawnedBullet = bullet;
                spawnedBullet.SetActive(true);
                break;
            }
        }

        if (spawnedBullet == null)
        {
            spawnedBullet = Instantiate(_bullet, _bullets_container.transform);
            _pool.Add(spawnedBullet);
        }

        spawnedBullet.transform.position = _firePoint.position;
        spawnedBullet.transform.rotation = _firePoint.rotation;

        var rb = spawnedBullet.GetComponent<Rigidbody>();
        rb.linearVelocity = _firePoint.forward * _fireSpeed;
    }

    private void MuzzleFlash()
    {
        if (_muzzleFlash != null)
        {
            _muzzleFlash.Play();
        }
    }

    public void ChangeGun(GameObject newGunPrefab)
    {
        if (newGunPrefab == null) return;

        if (this.transform.childCount > 0)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }

        GameObject newGun = Instantiate(newGunPrefab, this.transform);
        newGun.transform.localPosition = Vector3.zero;

        foreach (Transform t in newGun.GetComponentsInChildren<Transform>(true))
        {
            if (t.CompareTag("FirePoint"))
            {
                _firePoint = t;
                _muzzleFlash = t.GetComponentInChildren<ParticleSystem>();
                break;
            }
        }
    }
}
