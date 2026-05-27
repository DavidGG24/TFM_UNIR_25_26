using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TemporalActivatorBehaviour : MonoBehaviour
{
    [SerializeField] private InputActionReference activate;
    [SerializeField] private float timeInActive = 12f;
    public UnityEvent<float, float> onTemporalActivated;

    private bool estoyActivado;
    private bool jugadorDentro;

    private void OnEnable()
    {
        activate.action.Enable();
    }

    private void Awake()
    {
        estoyActivado = false;
        jugadorDentro = false;
    }

    private float timeActivated;
    void Update()
    {
        if (jugadorDentro && activate.action.triggered && !estoyActivado)
        {
            timeActivated = Time.time;
            onTemporalActivated.Invoke(timeActivated, timeInActive);
            estoyActivado = true;
        }

        if (Time.time >= timeActivated + timeInActive)
        {
            estoyActivado = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            Debug.Log("Estoy dentro");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
            Debug.Log("Estoy fuera");
        }
    }

    private void OnDisable()
    {
        activate.action.Disable();
    }
}
