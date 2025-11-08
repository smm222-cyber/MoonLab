using UnityEngine;
using System.Collections;

public class EndDayOnPickup : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnDestroy()
    {
        // Evitar que se ejecute varias veces
        if (hasTriggered) return;
        hasTriggered = true;

        // Inicia una corrutina para esperar un frame antes de cambiar de escena
        if (GameManager.Instance != null)
        {
            Debug.Log("Item que termina el día recogido, preparando cambio de nivel...");
            GameManager.Instance.StartCoroutine(WaitAndChangeScene());
        }
        else
        {
            Debug.LogWarning("GameManager no encontrado al intentar avanzar el día.");
        }
    }

    private IEnumerator WaitAndChangeScene()
    {
        // Espera un frame (para dejar que el objeto se destruya sin error)
        yield return null;

        // Espera opcional de medio segundo para dar un pequeño respiro visual
        yield return new WaitForSeconds(0.2f);

        GameManager.Instance.EndDayAndLoadNext();
    }
}
