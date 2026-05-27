using UnityEngine;
using UnityEngine.InputSystem;

public class FakeShadowBehaviour : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;

    private void OnEnable()
    {
        move.action.Enable();
        move.action.started += OnMove;
        move.action.performed += OnMove;
        move.action.canceled += OnMove;

        //jump.action.Enable();
        //jump.action.started += OnJump;
        //jump.action.performed += OnJump;
        //jump.action.canceled += OnStopJump;
    }

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(new Vector3(rawMove.x, rawMove.y, 0));
        transform.position = playerTransform.position;
        transform.rotation = playerTransform.rotation;
    }

    Vector2 rawMove;
    private void OnMove(InputAction.CallbackContext context)
    {
        rawMove = context.action.ReadValue<Vector2>();  //Lee le valor de la acción que lo ha llamado, indicando que esperamos leer un Vector2
    }

    /*bool canJump = true;
    bool isJumping = false;
    private bool isJumpingCanceled;
    private void OnStopJump(InputAction.CallbackContext context)
    {
        if (isJumping)
        {
            isJumpingCanceled = true;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (canJump && CheckIsOnGround())
        {
            isJumping = true;
            canJump = false;
        }
    }*/

    protected void Move(Vector3 direction)
    {
        if (direction.magnitude > 0f)
        {
            animator.SetBool("IsMoving", true);

        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }
}
