using System;
using UnityEngine;

public class MechanismPlatformBehaviour : MonoBehaviour
{
    [SerializeField] ActivatorBehaviour activator;

    private void OnEnable()
    {
        activator.onActivate.AddListener(ActivatePlatform);
    }

    private void Awake()
    {
        if (transform.childCount == 1)
        {
            GetComponent<MeshRenderer>().enabled = false;
        } else if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
            }
        }
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Collider>().enabled = false;
    }

    private void ActivatePlatform(bool activado)
    {
        if (activado)
        {
            if (transform.childCount == 1)
            {
                GetComponent<MeshRenderer>().enabled = true;
            }
            else if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                }
            }
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Collider>().enabled = true;
        } else
        {
            if (transform.childCount == 1)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            else if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                }
            }
            GetComponent<Collider>().isTrigger = true;
            GetComponent<Collider>().enabled = false;
        }
    }
}
