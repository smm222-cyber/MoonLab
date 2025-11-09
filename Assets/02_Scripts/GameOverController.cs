using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverController : MonoBehaviour
{
    [Tooltip("Nombre de la escena del menú principal a cargar (vacío = no cargar)")]
    public string mainMenuScene = "Menu";

    [Tooltip("Si está activado, recarga la escena actual en vez de cargar otra")]
    public bool reloadCurrentOnRetry = false;

    /// <summary>
    /// Método público para asignar al botón "Volver al menú".
    /// </summary>
    public void OnBackToMenu()
    {
        if (string.IsNullOrEmpty(mainMenuScene))
        {
            Debug.LogWarning("GameOverController: mainMenuScene vacío, no se cargará ninguna escena.");
            return;
        }

        // Restaurar timeScale por si el juego estaba pausado
        Time.timeScale = 1f;

        // Cargar la escena del menú de forma segura
        SceneManager.LoadScene(mainMenuScene);
    }


    public void OnRetry()
    {
        // Restaurar timeScale por si el juego estaba pausado
        Time.timeScale = 1f;

        if (reloadCurrentOnRetry)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Por defecto, volver al menú
            OnBackToMenu();
        }
    }


    public void GoToMainMenu()
    {
        OnBackToMenu();
    }
}
