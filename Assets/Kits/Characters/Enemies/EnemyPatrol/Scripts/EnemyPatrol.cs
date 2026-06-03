using UnityEngine;

public class EnemyPatrol : EnemyBase
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private float velocity = 3f;

    private Transform currentTarget;
    void Start()
    {
        if (Random.Range(0, 2) > 0)
        {
            currentTarget = point1;
        } else
        {
            currentTarget = point2;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemy.transform.position.x < currentTarget.position.x)
        {
            enemy.transform.position += Vector3.right * velocity * Time.deltaTime;
        } else if (enemy.transform.position.x > currentTarget.position.x)
        {
            enemy.transform.position += Vector3.left * velocity * Time.deltaTime;
        }

        if (currentTarget == point1 && enemy.transform.position.x <= currentTarget.position.x)
        {
            currentTarget = point2;
        } else if (currentTarget == point2 && enemy.transform.position.x >= currentTarget.position.x)
        {
            currentTarget = point1;
        }
    }
}
