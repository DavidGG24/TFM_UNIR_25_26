using UnityEngine;
using UnityEngine.Events;

public class BreakableObstacle : MonoBehaviour
{
    private enum BreakingTypes
    {
        withEvent = 0,
        withTrigger = 1,
        withCollision = 2,
    }

    [SerializeField] BreakingTypes breakingType;
    [SerializeField] private ActivatorBehaviour activator;

    private void OnEnable()
    {
        if (breakingType == BreakingTypes.withEvent)
        {
            activator.onActivate.AddListener(InvokeDestroy);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (breakingType)
        {
            case BreakingTypes.withEvent:
                break;
            case BreakingTypes.withTrigger:
                break;
            case BreakingTypes.withCollision:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (breakingType == BreakingTypes.withTrigger && other.CompareTag("Player"))
        {
            InvokeDestroy(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (breakingType == BreakingTypes.withCollision && collision.collider.CompareTag("Player"))
        {
            InvokeDestroy(true);
        }
    }

    private void InvokeDestroy(bool destroyed)
    {
        if (destroyed)
        {
            Destroy(gameObject);
        }
    }
}
