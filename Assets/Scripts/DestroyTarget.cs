using UnityEngine;

public class DestroyTarget : MonoBehaviour
{
    [SerializeField] private float _delayTime = 0.25f;
    [SerializeField] private GameObject _spawner;
    [SerializeField] private GameObject _hitEffect;

    public void DestroyGameObject()
    {
        GameObject parent = transform.parent.gameObject;
        _hitEffect.SetActive(true);
        this.gameObject.SetActive(false);
        _spawner.GetComponent<Spawner>().SpawnTarget(true);
        Destroy(parent, _delayTime);
    }
}
