using UnityEngine;

public class Bullet : MonoBehaviour
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            collision.gameObject.GetComponent<DestroyTarget>().DestroyGameObject();
            this.gameObject.SetActive(false);
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }
}
