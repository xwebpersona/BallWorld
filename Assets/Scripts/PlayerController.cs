using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость движения
    public float rotationSpeed = 10f; // Скорость вращения шара
    public Transform cameraTransform; // Ссылка на камеру
    public float cameraRotationSpeed = 100f; // Скорость поворота камеры

    private Rigidbody rb;
    private Vector2 moveInput;
    private PlayerInput playerInput; // Используем сгенерированный класс Input Action
    private float targetCameraRotationY = 0f; // Целевая ротация камеры по Y

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Получаем сгенерированный класс
        playerInput = new PlayerInput();
        playerInput.Enable();

        if (cameraTransform != null)
        {
            targetCameraRotationY = cameraTransform.eulerAngles.y; // Запоминаем стартовую ориентацию камеры
        }
    }

    private void OnEnable()
    {
        // Подписываемся на события для движения и прыжка
        playerInput.Keyboard.Move.performed += OnMove;
        playerInput.Keyboard.Move.canceled += OnMove;
        playerInput.Keyboard.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        // Отписываемся от событий
        playerInput.Keyboard.Move.performed -= OnMove;
        playerInput.Keyboard.Move.canceled -= OnMove;
        playerInput.Keyboard.Jump.performed -= OnJump;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Движение шара
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        rb.AddForce(moveDirection * moveSpeed, ForceMode.VelocityChange);

        // Вращение шара
        Vector3 rotationAxis = new Vector3(moveInput.y, 0, -moveInput.x);
        rb.AddTorque(rotationAxis * rotationSpeed, ForceMode.VelocityChange);

        // Поворот камеры
        RotateCamera(moveInput.x);
    }

    private void RotateCamera(float moveX)
    {
        if (cameraTransform == null) return;

        // Определяем целевой угол поворота камеры (например, ±15 градусов в зависимости от направления)
        float maxRotationAngle = 15f; // Максимальный угол поворота влево/вправо
        targetCameraRotationY = Mathf.Lerp(targetCameraRotationY, cameraTransform.eulerAngles.y + moveX * maxRotationAngle, Time.deltaTime * cameraRotationSpeed);

        // Применяем поворот только по Y
        cameraTransform.rotation = Quaternion.Euler(0, targetCameraRotationY, 0);
    }
}