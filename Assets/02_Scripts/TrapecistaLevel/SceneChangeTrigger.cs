using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    [Tooltip("Nombre de la escena que se cargará al activarse este trigger")]
    public string targetSceneName;

    [Tooltip("¿Esperar un poco antes de cambiar de escena? (por ejemplo, para mostrar mensaje de fin de nivel)")]
    public float delayBeforeChange = 0.5f;

    private bool hasActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasActivated && collision.CompareTag("Player"))
        {
            hasActivated = true;
            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(delayBeforeChange);

        // Si hay GameManager y FadeController, usa fade
        if (GameManager.Instance != null && GameManager.Instance.fadeController != null)
        {
            yield return GameManager.Instance.fadeController.FadeOut();
        }

        // Cargar la nueva escena
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("No se ha asignado el nombre de la escena en SceneChangeTrigger.");
        }

        // Volver a hacer fade in
        if (GameManager.Instance != null && GameManager.Instance.fadeController != null)
        {
            yield return GameManager.Instance.fadeController.FadeIn();
        }
    }
}
