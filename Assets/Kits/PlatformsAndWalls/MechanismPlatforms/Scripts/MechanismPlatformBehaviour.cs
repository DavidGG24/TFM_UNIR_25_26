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
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().isTrigger = true;
    }

    private void ActivatePlatform(bool activado)
    {
        if (activado)
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent <Collider>().isTrigger = false;
        } else
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().isTrigger = true;
        }
    }
}
