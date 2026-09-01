using System;
using UnityEngine;
using UnityEngine.Events;

public class SavePoint : MonoBehaviour
{
    protected Vector3 playerPosition; // Posición a guardar del jugador
    public UnityEvent<Vector3> UpdateEverySave; // Evento para guardar la posición en todos los puntos de guardado 

    protected virtual void OnTriggerEnter(Collider other) // Al entrar en contacto, guarda la posición del player y actualiza todos los SavePoints
    {
        if (other.CompareTag("Player"))
        {
            playerPosition = other.transform.position;
            UpdateEverySave.Invoke(playerPosition);
            Debug.Log("Cogida la posición " + playerPosition);
        }
    }
}
