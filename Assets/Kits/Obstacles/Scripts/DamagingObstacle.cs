using UnityEngine;

public class DamagingObstacle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
