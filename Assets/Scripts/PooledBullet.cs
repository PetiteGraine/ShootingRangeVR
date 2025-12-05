using UnityEngine;

public class PooledBullet : MonoBehaviour
{
    private float _lifeTime = 5f;

    private void OnEnable()
    {
        Invoke(nameof(Disable), _lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
