using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActivatorBehaviour : MonoBehaviour
{
    [SerializeField] private InputActionReference activate;
    [SerializeField] private bool multiActivacion;
    [SerializeField] private BoxCollider cameraConfiner;
    public UnityEvent<bool> onActivate;
    public int id;
    public bool estoyActivado;
    private bool jugadorDentro;
    private Animator animator;

    private void OnEnable()
    {
        activate.action.Enable();
    }

    private void Awake()
    {
        jugadorDentro = false;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (estoyActivado)
        {
            OnActivatorActivated();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jugadorDentro && activate.action.triggered)
        {
            if (!estoyActivado)
            {
                OnActivatorActivated();
            }
            else if (multiActivacion)
            {
                estoyActivado = false;
                onActivate.Invoke(estoyActivado);
                animator.SetBool("Activated", false);
                GetComponent<AudioSource>().Play();
            }
        }
    }

    private void OnActivatorActivated()
    {
        estoyActivado = true;
        onActivate.Invoke(estoyActivado);
        animator.SetBool("Activated", true);
        Debug.Log("Estoy activado");
        GetComponent<AudioSource>().Play();

        if (cameraConfiner)
        {
            cameraConfiner.center = new Vector3(59.3644943f, 13.9754524f, 0);
            cameraConfiner.size = new Vector3(104.110855f, 18.9509048f, 1f);
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
