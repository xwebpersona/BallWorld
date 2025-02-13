using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen; // Ссылка на UI экран смерти

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowDeathScreen();
        }
    }

    private void ShowDeathScreen()
    {
        Time.timeScale = 0f; // Останавливаем
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // Разблокируем курсор
    }

    // Метод для кнопки рестарта
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}