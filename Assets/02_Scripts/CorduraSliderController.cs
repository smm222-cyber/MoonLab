using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CorduraSliderController : MonoBehaviour
{
    [Header("Referencias")]
    public Slider corduraSlider;               // Referencia al slider
    public CorduraController playerController; // Referencia al CorduraController

    [Header("Parámetros")]
    public float drainSpeed = 5f;              // Cantidad de cordura que se pierde por segundo
    public float recoverSpeed = 15f;           // Cantidad de cordura que se recupera por segundo
    public string gameOverSceneName = "Menu";  // Escena que se cargará al perder toda la cordura

    private bool gameOverTriggered = false;    // Evitar múltiples llamadas

    void Update()
    {
        if (playerController == null || corduraSlider == null)
            return;

        // Verificar si la cordura llegó a cero
        if (playerController.cordura <= 0 && !gameOverTriggered)
        {
            TriggerGameOver();
            return;
        }

        // Si est� durmiendo (animaci�n Sleeped activa), recargar gradualmente
        if (playerController.IsSleeping())
        {
            playerController.cordura += recoverSpeed * Time.deltaTime;
        }
        else // Si no est� durmiendo, decrementar gradualmente
        {
            playerController.cordura -= drainSpeed * Time.deltaTime;
        }

        // Limitar entre 0 y 100
        playerController.cordura = Mathf.Clamp(playerController.cordura, 0f, 100f);

        // Actualizar valor del slider
        corduraSlider.value = playerController.cordura;
    }

    private void TriggerGameOver()
    {
        gameOverTriggered = true;
        Debug.Log("Cordura agotada - Game Over");
        
        // Asegurar que el tiempo esté normal antes de cargar la escena
        Time.timeScale = 1f;
        
        // Cargar escena de Game Over
        SceneManager.LoadScene(gameOverSceneName);
    }
}
