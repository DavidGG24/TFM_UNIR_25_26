using UnityEngine;
using UnityEngine.InputSystem;

public class JoinLaceBehaviour : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private InputActionReference makeTeleport;
    [SerializeField] private Transform[] puntos;
    [SerializeField] private float maxLaceDistance = 10f;

    private float teleportForce = 10f;

    private void OnEnable()
    {
        makeTeleport.action.Enable();
    }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    /*    public void SetUpLine(Transform[] points)
        {
            lr.positionCount = points.Length;
            this.puntos = points;
        }*/

    private bool canEnlace;
    private void Update()
    {
        if ((puntos[0].position - puntos[1].position).magnitude <= maxLaceDistance) // Comprobamos que la sombra está dentro del rango
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

        if (canEnlace && makeTeleport.action.triggered) // Activa Enlace cuando hace click y está dentro del rango
        {
            Enlace();
        }

        if (enlaceActivated)
        {
            var shadow = puntos[1].parent;
            var shadowRigidbody = shadow.GetComponent<Rigidbody>();
            shadow.GetComponent<CapsuleCollider>().enabled = false;
            shadowRigidbody.useGravity = false;
            //shadowRigidbody.AddForce(director * teleportForce, ForceMode.Impulse);
            shadow.position += director * teleportForce * Time.deltaTime;

            if (Mathf.Abs(puntos[0].position.x - puntos[1].position.x) < 0.5f && Mathf.Abs(puntos[0].position.y - puntos[1].position.y) < 0.5f)
            {
                //shadowRigidbody.AddForce(shadowRigidbody.linearVelocity * -1, ForceMode.Impulse);
                shadow.GetComponent<CapsuleCollider>().enabled = true;
                shadowRigidbody.useGravity = true;
                enlaceActivated = false;
            }
        }
    }

    Vector3 director;
    bool enlaceActivated;
    public void Enlace()
    {
        director = (puntos[0].position - puntos[1].position).normalized;
        enlaceActivated = true;
    }

    private void OnDisable()
    {
        makeTeleport.action.Disable();
    }
}
