using UnityEngine;

public class Rock : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("RealObstacle") || collision.gameObject.layer == LayerMask.NameToLayer("ShadowObstacle") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.parent.GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
    }
}
