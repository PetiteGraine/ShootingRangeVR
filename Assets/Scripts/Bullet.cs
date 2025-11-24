using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            collision.gameObject.GetComponent<DestroyTarget>().DestroyGameObject();
            Destroy(this.gameObject);
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }
}
