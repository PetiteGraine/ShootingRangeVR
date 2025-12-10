using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using System.Collections.Generic;

public class BulletPoolManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string _bulletKeyBase = "Bullets/Bullet_Pistol_A";
    [SerializeField] private int _initialPoolSize = 10;

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
                bullet.SetActive(true);
                return bullet;
            }
        }

        return CreateNewBullet();
    }
}