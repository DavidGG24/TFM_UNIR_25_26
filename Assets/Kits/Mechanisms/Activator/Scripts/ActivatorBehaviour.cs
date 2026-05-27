using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActivatorBehaviour : MonoBehaviour
{
    [SerializeField] private InputActionReference activate;
    [SerializeField] private bool multiActivacion;
    public UnityEvent<bool> onActivate;
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

    // Update is called once per frame
    void Update()
    {
        if (jugadorDentro && activate.action.triggered)
        {
            if (!estoyActivado)
            {
                estoyActivado = true;
                onActivate.Invoke(estoyActivado);
                Debug.Log("Estoy activado");
            }
            else if (multiActivacion)
            {
                estoyActivado = false;
                onActivate.Invoke(estoyActivado);
            }
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
