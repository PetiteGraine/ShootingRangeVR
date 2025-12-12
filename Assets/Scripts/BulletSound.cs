using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BulletSound : MonoBehaviour
{
    [Header("Bullet Sounds Settings")]
    [SerializeField] private int _numberOfSounds = 3;

    private AudioClip[] _bulletSounds;

    private IEnumerator Start()
    {
        _bulletSounds = new AudioClip[_numberOfSounds];

        string suffix =
#if UNITY_ANDROID
        "_Quest";
#else
        "_PCVR";
#endif

        for (int i = 1; i <= _numberOfSounds; i++)
        {
            string soundPath = $"Audios/shot-{i}{suffix}";

            var handle = Addressables.LoadAssetAsync<AudioClip>(soundPath);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _bulletSounds[i - 1] = handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load BulletSound Addressable: {soundPath}");
            }
        }
    }

    public AudioClip GetRandomClip()
    {
        if (_bulletSounds == null || _bulletSounds.Length == 0)
            return null;

        return _bulletSounds[Random.Range(0, _bulletSounds.Length)];
    }
}
