using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // Input System (Send Messages)

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // movement speed
    public float jumpForce = 5f; // jump impulse

    // Ground check
    public Transform groundCheck;       // place at player's feet (child of player)
    public float groundDistance = 0.4f;
    public LayerMask groundMask;        // set to "Ground" layer for ground test in Inspector

    public AudioClip footStepSfx;

    private Rigidbody rb;               // player rigidbody
    private Vector2 moveInput;          // WASD/Arrows as (x,y)
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // cache Rigidbod
        StartCoroutine(PlayFootStep());
    }

    void Update()
    {
        CheckGround(); // update grounded state each frame
    }

    void FixedUpdate()
    {
        MovePlayer();   // physics-friendly movement tick
    }

    void OnJump()       // action "Jump" → OnJump (case-sensitive)
    {
        if (isGrounded) // only jump when grounded
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse); // upward impulse
        }
    }

    void OnMovement(InputValue value) // action "Movement" → OnMovement (case-sensitive)
    {
        moveInput = value.Get<Vector2>(); // read Vector2: x=A/D, y=W/S
    }

    void MovePlayer()
    {
        // convert 2D input to world-space using player right/forward
        Vector3 direction = (transform.right * moveInput.x) + (transform.forward * moveInput.y);
        direction = direction.normalized; // avoid faster diagonal movement

        // Unity 6: use linearVelocity (renamed from velocity); preserve Y (gravity/jump)
        rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);
    }

    // Ground detection using invisible sphere at feet
    void CheckGround()
    {
        if (groundCheck == null)    // safety: require groundCheck transform
        {
            isGrounded = false;     // not grounded if probe missing
            return;
        }

        // true if sphere overlaps any collider on groundMask within groundDistance
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    IEnumerator PlayFootStep()
    {
        while (true)
        {
            if (rb.linearVelocity.magnitude > 0.1f && isGrounded)
            {
                AudioManager.instance.PlaySFX(footStepSfx);
            }
            yield return new WaitForSeconds(0.5f);
        }


    }
}