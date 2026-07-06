using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;

    public UnityEvent MakeRespawn;

    Vector2 rawMove;
    [SerializeField] float acceleration = 0.5f;
    [SerializeField] float maxVelocityX = 10.0f;

    bool canJump = true;
    bool isJumping = false;
    private bool isJumpingCanceled;
    [SerializeField] float jumpVelocity = 27f;
    [SerializeField] float fallMultiplier = 3f;
    [SerializeField] float lowJumpMultiplier = 2.5f;
    public LayerMask GroundLayer;


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
            CheckFalling();
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (canJump && CheckIsOnGround())
        {
            isJumping = true;
            canJump = false;
            animator.SetTrigger("IsJumping");
            animator.SetBool("IsLanded", false);
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

        if (!characterActive)
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
        }
    }

    float secondsInTurning = 0;
    float secondsInJump = 0;
    void FixedUpdate()
    {
        if (characterActive)
        {
            Move(new Vector3(rawMove.x, rawMove.y, 0));

            Debug.DrawRay(transform.GetComponentInChildren<BoxCollider>().transform.position, Vector3.down * 0.5f, Color.red);
            Debug.DrawRay(transform.GetComponentInChildren<BoxCollider>().transform.position + new Vector3(0.3f, 0f, 0f), Vector3.down * 0.5f, Color.red);
            Debug.DrawRay(transform.GetComponentInChildren<BoxCollider>().transform.position - new Vector3(0.3f, 0f, 0f), Vector3.down * 0.5f, Color.red);

            /*if (Mathf.Abs(rb.linearVelocity.x) > maxVelocityX) // Si el jugador pasa la velocidad máxima, se setea de nuevo a esta
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x > 0 ? maxVelocityX : -maxVelocityX, rb.linearVelocity.y, 0f);
            }*/

            //if (jump.action.triggered && canJump)
            //{
            //    //rb.AddForce(new Vector3(0f, jumpVelocity, 0f), ForceMode.Impulse);
            //    isJumping = true;
            //    canJump = false;
            //}

            if (isJumping && secondsInJump < 0.5f && !isJumpingCanceled)
            {
                secondsInJump += Time.deltaTime;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, 0f);
            } else if (isJumping && secondsInJump >= 0.5f)
            {
                //isJumping = false;
                //secondsInJump = 0f;
                //isJumpingCanceled = false;
                CheckFalling();
                //rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime, 0f);
                //rb.AddForce(new Vector3(0f, Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime, 0f), ForceMode.Impulse);
            } else if ((isJumping && isJumpingCanceled) || !CheckIsOnGround())
            {
                //isJumping = false;
                //secondsInJump = 0f;
                //isJumpingCanceled = false;
                CheckFalling();
                //rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime, 0f);
                //rb.AddForce(new Vector3(0f, Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime, 0f), ForceMode.VelocityChange);
            }


            //if (isJumpingCanceled)
            //{
            //    rb.AddForce(new Vector3(0f, -rb.linearVelocity.y, 0f), ForceMode.Impulse);
            //    secondsInJump = 0f;
            //    isJumpingCanceled = false;
            //}

            if (shouldTurnBack)
            {
                transform.rotation = Quaternion.Inverse(transform.rotation);
                shouldTurnBack = false;
            }
        }
    }

    Vector3 lastMoveDirection;
    bool shouldTurnBack = false;
    protected void Move(Vector3 direction)
    {
        //rb.AddForce(direction * acceleration * Time.deltaTime, ForceMode.VelocityChange);
        rb.linearVelocity = new Vector3(direction.normalized.x * maxVelocityX, rb.linearVelocity.y, 0f);
        //rb.linearVelocity = direction.normalized * maxVelocityX;

        if (direction.magnitude > 0f)
        {
            if (direction.normalized.x != lastMoveDirection.x)
            {
                //transform.rotation = Quaternion.Inverse(transform.rotation);
                //transform.localPosition = direction.x > 0 ? new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z) : new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
                //animator.SetTrigger("TurnBack");
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
        if (GroundLayer == (1 << other.gameObject.layer))
        {
            canJump = true;
            isJumping = false;
            isJumpingCanceled = false;
            secondsInJump = 0f;
            animator.SetBool("IsLanded", true);
            isAlreadyFalling = false;
            GetComponent<ConstantForce>().enabled = false;
        }
    }

    public void OnDeath()
    {
        Debug.Log("AUCH!");
        MakeRespawn.Invoke();
    }

    private bool CheckIsOnGround()
    {
        return Physics.Raycast(transform.GetComponentInChildren<BoxCollider>().transform.position, Vector3.down, 0.5f, GroundLayer)
            || Physics.Raycast(transform.GetComponentInChildren<BoxCollider>().transform.position + new Vector3(0.3f, 0f, 0f), Vector3.down, 0.5f, GroundLayer)
            || Physics.Raycast(transform.GetComponentInChildren<BoxCollider>().transform.position - new Vector3(0.3f, 0f, 0f), Vector3.down, 0.5f, GroundLayer);
    }

    private bool isAlreadyFalling = false;
    private void CheckFalling()
    {
        if (!isAlreadyFalling)
        {
            animator.SetTrigger("IsFalling");
            isAlreadyFalling = true;
            GetComponent<ConstantForce>().enabled = true;
        }
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
