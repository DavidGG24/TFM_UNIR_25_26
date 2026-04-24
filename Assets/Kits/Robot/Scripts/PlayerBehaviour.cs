using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings.SplashScreen;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;

    Vector2 rawMove;
    [SerializeField] float acceleration = 0.5f;
    [SerializeField] float maxVelocityX = 10.0f;

    bool canJump = true;
    [SerializeField] float jumpVelocity = 0.5f;

    private void OnEnable()
    {
        move.action.Enable();
        move.action.started += OnMove;
        move.action.performed += OnMove;
        move.action.canceled += OnMove;

        jump.action.Enable();
    }

    Rigidbody rb;
    Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastMoveDirection = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        Move(new Vector3(rawMove.x, rawMove.y, 0));

        if (rb.linearVelocity.x > maxVelocityX)
        {
            rb.linearVelocity = new Vector3(maxVelocityX, rb.linearVelocity.y, rb.linearVelocity.z);
        }

        if (jump.action.triggered)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + jumpVelocity * Time.deltaTime, 0f);
        }
    }

    Vector3 lastMoveDirection;
    bool shouldTurnBack = false;
    protected void Move(Vector3 direction)
    {
        rb.linearVelocity += new Vector3(direction.x * acceleration * Time.deltaTime, rb.linearVelocity.y, rb.linearVelocity.z);

        if (direction.magnitude > 0f)
        {
            if (direction.normalized.x != lastMoveDirection.x)
            {
                shouldTurnBack = true;
                lastMoveDirection = direction;
            }

            if (shouldTurnBack)
            {
                animator.SetTrigger("TurnBack");
                rb.linearVelocity = Vector3.zero;
            } else if (rb.linearVelocity.x == 0f)
            {
                rb.linearVelocity = new Vector3(5f * direction.normalized.x, rb.linearVelocity.y, 0f);
            }
            animator.SetBool("IsMoving", true);
            shouldTurnBack = false;
        }

        else
        {
            animator.SetBool("IsMoving", false);
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        rawMove = context.action.ReadValue<Vector2>();  //Lee le valor de la acción que lo ha llamado, indicando que esperamos leer un Vector2
    }

    private void OnCollisionExit(Collision collision)
    {
        canJump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
    }

    private void OnDisable()
    {
        move.action.Disable();
        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;

        jump.action.Disable();
    }
}
