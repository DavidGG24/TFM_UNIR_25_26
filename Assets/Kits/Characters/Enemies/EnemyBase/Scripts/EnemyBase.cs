using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerBehaviour>().OnDeath();
        }
    }
}
