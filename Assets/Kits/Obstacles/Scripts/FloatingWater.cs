using System.Collections.Generic;
using UnityEngine;

public class FloatingWater : MonoBehaviour
{
    [SerializeField] float indiceFlotacion = 1f;
    private List<GameObject> objectsFloating = new List<GameObject>();
    private List<float> objectsCurrentPosition = new List<float>();
    private float ySuperficie;

    void Start()
    {
        ySuperficie = transform.position.y + (transform.localScale.y / 2);
    }

    void FixedUpdate()
    {
        ySuperficie = transform.position.y + (transform.localScale.y / 2);

        if (objectsFloating.Count > 0)
        {
            for (int i = 0; i < objectsFloating.Count; i++)
            {
                Rigidbody rb = objectsFloating[i].GetComponent<Rigidbody>();
                if (objectsFloating[i].transform.position.y < ySuperficie)
                {
                    float impulsoFlote = Mathf.Clamp01(ySuperficie - objectsFloating[i].transform.position.y / objectsCurrentPosition[i]) * indiceFlotacion;
                    rb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * impulsoFlote, 0f), ForceMode.Acceleration);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FloatableObject"))
        {
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            objectsFloating.Add(other.gameObject);
            objectsCurrentPosition.Add(other.transform.position.y);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FloatableObject"))
        {
            objectsCurrentPosition.RemoveAt(objectsFloating.IndexOf(other.gameObject));
            objectsFloating.Remove(other.gameObject);
        }
    }
}
