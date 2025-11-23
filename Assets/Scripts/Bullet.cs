using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.CompareTag("Target"))
        // {
        //     Destroy(collision.gameObject);
        //     Destroy(gameObject);
        // }
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }
}
