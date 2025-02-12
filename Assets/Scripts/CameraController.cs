using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Cinemachine Settings")]
    public CinemachineVirtualCamera virtualCamera;
    public float rotationSpeed = 100f;
    public float mouseSensitivity = 1f;

    private Vector3 initialFollowOffset; // Начальное смещение камеры
    private bool isRotating = false;
    private float currentRotationAngle = 0f; // Текущий угол вращения

    private void Awake()
    {
        if (virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        // Сохраняем начальное смещение камеры
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            initialFollowOffset = transposer.m_FollowOffset;
        }
        else
        {
            Debug.LogError("Добавьте Transposer компонент к виртуальной камере!");
        }
    }

    public void OnRotateCamera(InputAction.CallbackContext context)
    {
        isRotating = context.performed;
    }

    private void Update()
    {
        if (isRotating)
        {
            RotateCamera();
        }
    }

    private void RotateCamera()
    {
        // Получаем ввод мыши
        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity;
        currentRotationAngle += mouseX * rotationSpeed * Time.deltaTime;

        // Обновляем позицию камеры
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer == null) return;

        // Поворачиваем начальное смещение камеры
        Quaternion rotation = Quaternion.Euler(0, currentRotationAngle, 0);
        transposer.m_FollowOffset = rotation * initialFollowOffset;

        // Гарантируем, что камера смотрит на цель
        virtualCamera.LookAt = virtualCamera.Follow;
    }
}