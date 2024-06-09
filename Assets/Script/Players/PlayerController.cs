using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce = 300f;
    public float moveSpeed = 5f;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundMask; // Для проверки слоя земли
    public Transform groundCheck; // Точка для Raycast

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GroundCheck();
        HandleJump();
    }

    void FixedUpdate()
    {
        Move();
    }

    // Метод для проверки нахождения на земле
    private void GroundCheck()
    {
        // Создаем сферический Raycast для лучшей проверки на землю
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, Color.red);
    }

    // Метод для обработки прыжков
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    // Метод для движения игрока
    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime); // Изменяем позицию игрока
    }

    // Метод для прыжка
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Изменяем тип силы на импульс
    }
}