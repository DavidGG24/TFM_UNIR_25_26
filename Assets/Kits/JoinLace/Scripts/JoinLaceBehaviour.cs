using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinLaceBehaviour : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private InputActionReference makeTeleport;
    [SerializeField] private Transform[] puntos;
    [SerializeField] private float maxLaceDistance = 10f;

    private float teleportForce = 10f;

    private bool realControls; // Define si el jugador está controlando al personaje de la chica
    public bool onlyRealControls; // Define si sólo puede usar la habilidad la chica o también la sombra

    private ChangeReality cr;

    private void OnEnable()
    {
        makeTeleport.action.Enable();

        cr = FindFirstObjectByType<ChangeReality>();
        cr.onChangeReality.AddListener(OnChangeReality);
    }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        realControls = true;
        onlyRealControls = true;
    }

    /*    public void SetUpLine(Transform[] points)
        {
            lr.positionCount = points.Length;
            this.puntos = points;
        }*/

    private bool canEnlace;
    private void Update()
    {
        if (!onlyRealControls)
        {
            realControls = true;
        }

        if (Vector3.Distance(puntos[0].position, puntos[1].position) <= maxLaceDistance) // Comprobamos que la sombra está dentro del rango
        {
            lr.enabled = true;
            canEnlace = true;
            for (int i = 0; i < puntos.Length; i++)
            {
                lr.SetPosition(i, puntos[i].position); // Renderiza la línea que los une
            }
        } else
        {
            lr.enabled = false;
            canEnlace = false;
        }

        if (realControls && canEnlace && makeTeleport.action.triggered) // Activa Enlace cuando hace click y está dentro del rango
        {
            Enlace();
        }

        if (enlaceActivated)
        {
            var shadow = puntos[1].parent;
            var shadowRigidbody = shadow.GetComponent<Rigidbody>();
            //shadowRigidbody.useGravity = false;
            //shadowRigidbody.AddForce(director * teleportForce, ForceMode.Impulse);
            shadow.position += director * teleportForce * Time.deltaTime;

            if (Vector3.Distance(destination, puntos[1].position) < 0.5f)
            {
                //shadowRigidbody.AddForce(shadowRigidbody.linearVelocity * -1, ForceMode.Impulse);
                //shadowRigidbody.useGravity = true;
                enlaceActivated = false;
            }
        }
    }

    Vector3 director;
    Vector3 destination;
    bool enlaceActivated;
    public void Enlace()
    {
        director = (puntos[0].position - puntos[1].position).normalized;
        destination = puntos[0].position;
        enlaceActivated = true;
    }

    private void OnChangeReality(int newReality)
    {
        realControls = newReality == 6;
    }

    private void OnDisable()
    {
        makeTeleport.action.Disable();
    }
}
