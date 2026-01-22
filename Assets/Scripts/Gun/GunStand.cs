using System.Collections;
using UnityEngine;

public class GunStand : MonoBehaviour
{

    [SerializeField] private GameObject gunStandSpawnPoint;
    private IEnumerator Start()
    {
        string suffix =
#if UNITY_ANDROID
        "_Quest";
#else
        "_PCVR";
#endif

        string fullKey = "Prefabs/Gun_Stand" + suffix;
        var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(fullKey);
        yield return handle;
        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            if (gunStandSpawnPoint == null)
            {
                Instantiate(handle.Result);
                yield break;
            }
            Instantiate(handle.Result, gunStandSpawnPoint.transform.position, gunStandSpawnPoint.transform.rotation);
        }
        else
        {
            Debug.LogError($"Failed to load GunStand Addressable: {fullKey}");
        }
    }
}
