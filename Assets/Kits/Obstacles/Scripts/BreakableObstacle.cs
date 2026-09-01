using System.Collections;
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

    public int obstacleId;
    public bool isDestroyed;
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
        if (transform.childCount != 0)
        {
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                transform.GetChild(0).GetChild(i).GetComponent<Collider>().isTrigger = true;
                transform.GetChild(0).GetChild(i).GetComponent<Rigidbody>().useGravity = false;
            }
        }

        if (isDestroyed)
        {
            InvokeDestroy(true);
        }
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
            //Destroy(gameObject);
            GetComponent<Collider>().isTrigger = true;
            GetComponent<Collider>().enabled = false;
            if (GetComponent<CapsuleCollider>())
            {
                GetComponent<CapsuleCollider>().enabled = false;
            }

            if (transform.childCount != 0)
            {
                for (int i = 0; i < transform.GetChild(0).childCount; i++)
                {
                    transform.GetChild(0).GetChild(i).GetComponent<Collider>().isTrigger = false;
                    transform.GetChild(0).GetChild(i).GetComponent<Rigidbody>().useGravity = true;
                    StartCoroutine(DestroyFragments(transform.GetChild(0).GetChild(i).gameObject));
                }
            }

            isDestroyed = true;
            GetComponent<AudioSource>().Play();
        }
    }

    private IEnumerator DestroyFragments(GameObject fragment)
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(fragment);
    }
}
