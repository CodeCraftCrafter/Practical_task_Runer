using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce = 300f;
    public float moveSpeed = 5f;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundMask; // Для проверки слоя земли
    public Transform groundCheck; // Точка для Raycast

    private Rigidbody rb;
    private Animator animator;
    private Weapon weapon;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Получаем компонент Animator
        weapon = GetComponentInChildren<Weapon>(); // Получаем компонент Weapon
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
        animator.SetBool("Jump", true); // Устанавливаем булеву переменную в аниматоре в true
        StartCoroutine(ResetJumpBool()); // Запускаем корутину для сброса булевой переменной
    }

    // Корутин для сброса булевой переменной после прыжка
    private IEnumerator ResetJumpBool()
    {
        yield return new WaitForSeconds(0.1f); // Ждем короткое время
        animator.SetBool("Jump", false); // Сбрасываем булеву переменную в false
    }

    // Метод для управления анимацией движения
    private void HandleMovementAnimation()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        bool isMoving = moveHorizontal != 0f || moveVertical != 0f;
        animator.SetBool("Speed", isMoving); // Устанавливаем булеву переменную "Speed" в зависимости от движения
    }

    // Метод для обработки стрельбы
    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) // Стреляем при нажатии на Ctrl
        {
            weapon.Shoot();
        }
    }
}