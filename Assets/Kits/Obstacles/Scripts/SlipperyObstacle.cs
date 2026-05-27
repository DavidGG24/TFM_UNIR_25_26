using UnityEngine;

public class SlipperyObstacle : MonoBehaviour
{
    Rigidbody playerRb = null;
    bool isPlayerOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPlayerOver)
        {
            playerRb.AddForce(new Vector3(0f, -1f), ForceMode.Impulse);
        } else if (playerRb)
        {
            playerRb.AddForce(new Vector3(0f, -playerRb.linearVelocity.y), ForceMode.Impulse);
            playerRb = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerOver = true;
            playerRb = collision.collider.GetComponent<Rigidbody>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerOver = false;
        }
    }
}
