using UnityEngine;
using UnityEngine.UI;

public class CorduraSliderController : MonoBehaviour
{
    [Header("Referencias")]
    public Slider corduraSlider;               // Referencia al slider
    public CorduraController playerController; // Referencia al CorduraController

    [Header("Parámetros")]
    public float drainSpeed = 5f;              // Cantidad de cordura que se pierde por segundo
    public float recoverSpeed = 15f;           // Cantidad de cordura que se recupera por segundo

    void Update()
    {
        if (playerController == null || corduraSlider == null)
            return;

        // Si está durmiendo (animación Sleeped activa), recargar gradualmente
        if (playerController.IsSleeping())
        {
            playerController.cordura += recoverSpeed * Time.deltaTime;
        }
        else // Si no está durmiendo, decrementar gradualmente
        {
            playerController.cordura -= drainSpeed * Time.deltaTime;
        }

        // Limitar entre 0 y 100
        playerController.cordura = Mathf.Clamp(playerController.cordura, 0f, 100f);

        // Actualizar valor del slider
        corduraSlider.value = playerController.cordura;
    }
}
