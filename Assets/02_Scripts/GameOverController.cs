using UnityEngine;
using UnityEngine.SceneManagement;

// Simple controller para la pantalla de Game Over
// Añade este componente a un GameObject en la escena GameOver
// y conecta su método GoToMainMenu() al OnClick del botón.
public class GameOverController : MonoBehaviour
{
    [Tooltip("Nombre de la escena del menú principal (asegúrate de añadirla a Build Settings)")]
    public string mainMenuSceneName = "Menu";

    // Método público para conectar al botón OnClick
    public void GoToMainMenu()
    {
        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.LogWarning("GameOverController: mainMenuSceneName no está configurado.");
            return;
        }

        // Opcional: restaurar timeScale por si el juego estaba pausado
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
