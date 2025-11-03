using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReturnToTrapecistaLevel : MonoBehaviour
{
    [Header("Configuración")]
    public string previousScene = "Trapecista_Level"; // Nombre exacto de la escena principal
    public float fadeDelay = 1f;                      // Tiempo antes de cambiar de escena

    private FadeController fadeController;

    void Start()
    {
        // Busca automáticamente el FadeController en la escena
        fadeController = FindObjectOfType<FadeController>();
    }

    public void GoBack()
    {
        // Evita múltiples clics mientras se ejecuta
        StartCoroutine(ReturnRoutine());
    }

    private IEnumerator ReturnRoutine()
    {
        if (fadeController != null)
        {
            fadeController.FadeOut();
        }
        else
        {
            Debug.LogWarning("FadeController no encontrado. Volviendo sin fade.");
        }

        yield return new WaitForSeconds(fadeDelay);

        SceneManager.LoadScene(previousScene);
    }
}
