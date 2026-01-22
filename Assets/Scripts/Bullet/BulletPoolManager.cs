using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using System.Collections.Generic;

public class BulletPoolManager : MonoBehaviour
{
    [Header("Bullet Pool Settings")]
    [SerializeField] private string _bulletKeyBase = "Bullets/Bullet_Pistol_A";
    [SerializeField] private int _initialPoolSize = 10;
    [SerializeField] private float _cleanupInterval = 0.5f;
    [SerializeField] private float _maxDistance = 50f;

    private Vector3 _cleanupOrigin;
    private List<GameObject> _pool = new List<GameObject>();
    private GameObject _loadedPrefab;
    public bool IsReady { get; private set; } = false;

    private IEnumerator Start()
    {
        string suffix =
#if UNITY_ANDROID
            "_Quest";
#else
            "_PCVR";
#endif
        string fullKey = _bulletKeyBase + suffix;

        var handle = Addressables.LoadAssetAsync<GameObject>(fullKey);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _loadedPrefab = handle.Result;
            InitializePool();
            IsReady = true;
            _cleanupOrigin = transform.position;
            StartCoroutine(CleanupLoop());
        }
        else
        {
            Debug.LogError($"[BulletPoolManager] Failed to load: {fullKey}");
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            CreateNewBullet(false);
        }
    }

    private GameObject CreateNewBullet(bool setActive = true)
    {
        GameObject obj = Instantiate(_loadedPrefab, this.gameObject.transform);
        obj.SetActive(setActive);
        _pool.Add(obj);
        return obj;
    }

    public GameObject GetBullet()
    {
        if (!IsReady) return null;

        foreach (var bullet in _pool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        return CreateNewBullet();
    }

    private IEnumerator CleanupLoop()
    {
        while (true)
        {
            for (int i = _pool.Count - 1; i >= 0; i--)
            {
                GameObject bullet = _pool[i];

                if (bullet.activeInHierarchy)
                {
                    if (Vector3.Distance(bullet.transform.position, _cleanupOrigin) > _maxDistance)
                    {
                        bullet.SetActive(false);
                    }
                }
            }
            yield return new WaitForSeconds(_cleanupInterval);
        }
    }
}