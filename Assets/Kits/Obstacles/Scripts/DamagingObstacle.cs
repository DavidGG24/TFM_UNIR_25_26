using UnityEngine;

public class DamagingObstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().useGravity = true;
        } else if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.enabled = false;
        }
    }
}
