using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;

    [Header("Events")]
    public UnityEvent MakeRespawn;

    [Header("Movement")]
    [SerializeField] float maxVelocityX = 10.0f;
    private Vector2 rawMove;

    [Header("Jump")]
    [SerializeField] float jumpVelocity = 27f;
    public LayerMask GroundLayer;
    bool canJump = true;
    bool isJumping = false;
    private bool isJumpingCanceled;

    [Header("Character Control")]
    public bool characterActive;

    [Header("Audio Clips")]
    [SerializeField] AudioClip[] footstepsClips;
    [SerializeField] AudioClip[] jumpClips;
    [SerializeField] AudioClip[] landingClips;
    [SerializeField] AudioClip[] damageClips;
    private bool canPlayLanding = false;

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
            canPlayLanding = true;
        }
    }

    Rigidbody rb;
    Animator animator;
    private void Awake()
    {
        AudioListener.volume = 1f;
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

    float secondsInJump = 0;
    void FixedUpdate()
    {
        if (characterActive)
        {
            Move(new Vector3(rawMove.x, rawMove.y, 0));

            Debug.DrawRay(transform.GetComponentInChildren<BoxCollider>().transform.position, Vector3.down * 0.5f, Color.red);
            Debug.DrawRay(transform.GetComponentInChildren<BoxCollider>().transform.position + new Vector3(0.3f, 0f, 0f), Vector3.down * 0.5f, Color.red);
            Debug.DrawRay(transform.GetComponentInChildren<BoxCollider>().transform.position - new Vector3(0.3f, 0f, 0f), Vector3.down * 0.5f, Color.red);

            if (isJumping && secondsInJump < 0.5f && !isJumpingCanceled)
            {
                secondsInJump += Time.deltaTime;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, 0f);
            } else if (isJumping && secondsInJump >= 0.5f || (isJumping && isJumpingCanceled) || !CheckIsOnGround())
            {
                CheckFalling();
            } 
            if (shouldTurnBack)
            {
                transform.GetChild(0).rotation = Quaternion.Inverse(transform.GetChild(0).rotation);
                shouldTurnBack = false;
            }
        }
    }

    Vector3 lastMoveDirection;
    bool shouldTurnBack = false;
    protected void Move(Vector3 direction)
    {
        rb.linearVelocity = new Vector3(direction.normalized.x * maxVelocityX, rb.linearVelocity.y, 0f);

        if (direction.magnitude > 0f)
        {
            if (direction.normalized.x != lastMoveDirection.x)
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
                shouldTurnBack = true;
                lastMoveDirection = direction;
            }

            if (!shouldTurnBack)
            {
                if (rb.linearVelocity.x == 0f)
                {
                    rb.linearVelocity = new Vector3(5f * direction.normalized.x, rb.linearVelocity.y, 0f);
                }
                else
                {
                    animator.SetBool("IsMoving", true);
                }
            }
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
            if (canPlayLanding)
            {
                canPlayLanding = false;
                PlayLanding();
            }
        }
    }

    public void OnDeath()
    {
        Debug.Log("AUCH!");

        int selectedClip = Mathf.RoundToInt(UnityEngine.Random.Range(0f, damageClips.Length - 1));

        GetComponent<AudioSource>().clip = damageClips[selectedClip];
        GetComponent<AudioSource>().volume = 0.4f;
        GetComponent<AudioSource>().Play();

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
            canPlayLanding = true;
        }
    }

    private void PlayFootStep()
    {
        int selectedClip = Mathf.RoundToInt(UnityEngine.Random.Range(0f, footstepsClips.Length-1));

        GetComponent<AudioSource>().clip = footstepsClips[selectedClip];
        GetComponent<AudioSource>().volume = 0.3f;
        GetComponent<AudioSource>().Play();
    }

    private void PlayJump()
    {
        int selectedClip = Mathf.RoundToInt(UnityEngine.Random.Range(0f, jumpClips.Length - 1));

        GetComponent<AudioSource>().clip = jumpClips[selectedClip];
        GetComponent<AudioSource>().volume = 0.35f;
        GetComponent<AudioSource>().Play();
    }

    private void PlayLanding()
    {
        int selectedClip = Mathf.RoundToInt(UnityEngine.Random.Range(0f, landingClips.Length - 1));

        GetComponent<AudioSource>().clip = landingClips[selectedClip];
        GetComponent<AudioSource>().volume = 0.35f;
        GetComponent<AudioSource>().Play();
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
