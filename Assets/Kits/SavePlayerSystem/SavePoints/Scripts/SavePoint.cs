using System;
using UnityEngine;
using UnityEngine.Events;

public class SavePoint : MonoBehaviour
{
    private Vector3 playerPosition;
    public UnityEvent<Vector3> UpdateEverySave;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPosition = other.transform.position;
            UpdateEverySave.Invoke(playerPosition);
            Debug.Log("Cogida la posición " + playerPosition);
        }
    }
}
