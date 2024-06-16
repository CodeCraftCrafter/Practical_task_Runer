using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce = 300f;
    public float moveSpeed = 5f;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundMask;
    public Transform groundCheck;

    private Rigidbody rb;
    private Animator animator;
    private Weapon weapon;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();
    }

    void Update()
    {
        GroundCheck();
        HandleJump();
        HandleMovementAnimation();
        HandleShooting();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, Color.red);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetBool("Jump", true);
        StartCoroutine(ResetJumpBool());
    }

    private IEnumerator ResetJumpBool()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Jump", false);
    }

    private void HandleMovementAnimation()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        bool isMoving = moveHorizontal != 0f || moveVertical != 0f;
        animator.SetBool("Speed", isMoving);
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) // Ctrl
        {
            weapon.Shoot();
        }
    }
}