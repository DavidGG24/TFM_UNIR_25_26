using UnityEngine;

public class TriggerFakeShadow : MonoBehaviour
{
    [SerializeField] private GameObject fakeShadow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool isInstantiated = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInstantiated)
        {
            var fs = Instantiate(fakeShadow, other.transform);
            fs.GetComponent<FakeShadowBehaviour>().playerTransform = other.transform;
            isInstantiated = true;
        }
    }
}
