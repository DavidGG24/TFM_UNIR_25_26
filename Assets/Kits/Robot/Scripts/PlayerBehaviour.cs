using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;

    Vector2 rawMove;
    [SerializeField] float acceleration = 0.5f;
    [SerializeField] float maxVelocityX = 10.0f;

    bool canJump = true;
    bool isJumping = false;
    private bool isJumpingCanceled;
    [SerializeField] float jumpVelocity = 50f;

    public bool characterActive;

    private void OnEnable()
    {
        move.action.Enable();
        move.action.started += OnMove;
        move.action.performed += OnMove;
        move.action.canceled += OnMove;

        jump.action.Enable();
        jump.action.started += OnJump;
        jump.action.performed += OnJump;
        jump.action.canceled += OnStopJump;
    }

    private void OnStopJump(InputAction.CallbackContext context)
    {
        if (isJumping)
        {
            isJumpingCanceled = true;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            isJumping = true;
            canJump = false;
        }
    }

    Rigidbody rb;
    Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); 
        characterActive = gameObject.layer == 6;
    }

    void Start()
    {
        lastMoveDirection = Vector3.right;
    }

    int framesInTurning = 0;
    float secondsInJump = 0;
    void Update()
    {
        if (characterActive)
        {
            Move(new Vector3(rawMove.x, rawMove.y, 0));

            if (Mathf.Abs(rb.linearVelocity.x) > maxVelocityX)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x > 0 ? maxVelocityX : -maxVelocityX, rb.linearVelocity.y, 0f);
            }

            //if (jump.action.triggered && canJump)
            //{
            //    //rb.AddForce(new Vector3(0f, jumpVelocity, 0f), ForceMode.Impulse);
            //    isJumping = true;
            //    canJump = false;
            //}

            if (isJumping && secondsInJump < 0.5f && !isJumpingCanceled)
            {
                secondsInJump += Time.deltaTime;
                rb.AddForce(new Vector3(0f, jumpVelocity * Time.deltaTime, 0f), ForceMode.Impulse);
            } else if (isJumping && secondsInJump >= 0.5f || isJumping && isJumpingCanceled)
            {
                isJumping = false;
                secondsInJump = 0f;
                isJumpingCanceled = false;
                rb.AddForce(new Vector3(0f, -rb.linearVelocity.y, 0f), ForceMode.Impulse);
            }

            //if (isJumpingCanceled)
            //{
            //    rb.AddForce(new Vector3(0f, -rb.linearVelocity.y, 0f), ForceMode.Impulse);
            //    secondsInJump = 0f;
            //    isJumpingCanceled = false;
            //}

            if (shouldTurnBack)
            {
                if (framesInTurning < 60)
                {
                    framesInTurning++;
                }
                else
                {
                    framesInTurning = 0;
                    transform.rotation = Quaternion.Inverse(transform.rotation);
                    shouldTurnBack = false;
                }
            }
        }
    }

    Vector3 lastMoveDirection;
    bool shouldTurnBack = false;
    protected void Move(Vector3 direction)
    {
        rb.AddForce(direction * acceleration * Time.deltaTime, ForceMode.VelocityChange);

        if (direction.magnitude > 0f)
        {
            if (direction.normalized.x != lastMoveDirection.x)
            {
                //transform.rotation = Quaternion.Inverse(transform.rotation);
                //transform.localPosition = direction.x > 0 ? new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z) : new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
                animator.SetTrigger("TurnBack");
                //transform.rotation = Quaternion.Inverse(transform.rotation);
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
                shouldTurnBack = true;
                lastMoveDirection = direction;
            }

            if (shouldTurnBack)
            {
                //animator.SetTrigger("TurnBack");
                //rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            } else if (rb.linearVelocity.x == 0f)
            {
                rb.linearVelocity = new Vector3(5f * direction.normalized.x, rb.linearVelocity.y, 0f);
            } else
            {

                animator.SetBool("IsMoving", true);
            }
            //shouldTurnBack = false;
        }
        else
        {
            animator.SetBool("IsMoving", false);
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        rawMove = context.action.ReadValue<Vector2>();  //Lee le valor de la acción que lo ha llamado, indicando que esperamos leer un Vector2
    }

    private void OnCollisionEnter(Collision collision)
    {
        //canJump = true;
    }

    private void OnTriggerEnter(Collider other)
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
