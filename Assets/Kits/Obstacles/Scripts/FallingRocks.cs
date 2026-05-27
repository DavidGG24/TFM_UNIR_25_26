using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    [SerializeField] private GameObject rock;
    [SerializeField] private float frequency = 3f;

    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0f;
    }
    
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= frequency)
        {
            Instantiate(rock, transform.position, Quaternion.identity);
            timer = 0f;
        }
    }
}
