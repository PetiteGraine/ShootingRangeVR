using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject _defaultGun;
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
    }

    public void FireBullet(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SpawnAndFireBullet();
            MuzzleFlash();
        }
    }

    private void SpawnAndFireBullet()
    {
        GameObject spawnedBullet = Instantiate(_bullet);
        spawnedBullet.transform.position = _firePoint.position;
        spawnedBullet.transform.rotation = _firePoint.rotation;

        spawnedBullet.GetComponent<Rigidbody>().linearVelocity =
            _firePoint.forward * _fireSpeed;

        Destroy(spawnedBullet, 5f);
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
