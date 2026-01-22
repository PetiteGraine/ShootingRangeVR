using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TargetSoundManager : MonoBehaviour
{
    private AudioClip _audioClip;

    private IEnumerator Start()
    {
        string suffix =
#if UNITY_ANDROID
        "_Quest";
#else
        "_PCVR";
#endif
        string soundPath = $"Audios/hit-impact{suffix}";

        var handle = Addressables.LoadAssetAsync<AudioClip>(soundPath);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _audioClip = handle.Result;
        }
        else
        {
            Debug.LogError($"Failed to load TargetSoundManager Addressable: {soundPath}");
        }
    }

    public AudioClip GetClip()
    {
        return _audioClip;
    }
}
