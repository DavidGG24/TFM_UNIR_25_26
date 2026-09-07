using UnityEngine;
using UnityEngine.AI;

public class EnemyFocus : EnemyBase
{
    [Header("Listen and Movement")]
    [SerializeField] private float listenDistance = 10f;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float velocity = 5f;

    [Header("Player")]
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject particlesPref;

    [SerializeField] bool drawGizmos;

    private Transform body;
    private Vector3 targetPosition;
    private bool targetIsPlayer = false;
    private bool isReturning = false;

    private void Awake()
    {
        body = transform.GetChild(0).transform;
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= listenDistance && !targetIsPlayer && !isReturning)
        {
            targetPosition = player.transform.position;
            targetIsPlayer = true;
        }

        if (Vector3.Distance(body.transform.position, targetPosition) > stopDistance)
        {
            FocusOnTheTarget();
        } else if (Vector3.Distance(body.transform.position, targetPosition) <= stopDistance && !isReturning)
        {
            targetPosition = transform.position;
            targetIsPlayer = false;
            isReturning = true;
        } else
        {
            isReturning = false;
        }

        //if (rb.linearVelocity.magnitude > 0.1f) //Código para spawnear partículas
        //{
        //    timer += Time.deltaTime;

        //    if (timer >= spawnInterval)
        //    {
        //        Instantiate(particlesPref, spawnPoint.position, Quaternion.identity);
        //        timer = 0f;
        //    }
        //}
    }

    Vector3 director;
    private void FocusOnTheTarget()
    {
        director = (targetPosition - body.transform.position).normalized;
        body.transform.position += director * velocity * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, listenDistance);
    }
}
