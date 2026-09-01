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
            GameObject rockInstance = Instantiate(rock, transform.position, Quaternion.identity);
            rockInstance.transform.parent = this.transform;
            timer = 0f;
        }
    }
}
