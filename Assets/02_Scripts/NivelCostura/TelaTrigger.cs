using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaTrigger : MonoBehaviour
{
    [SerializeField] private string sewingSceneName = "SewingScene"; // nombre de la escena de costura

    //Cuando el jugador se acerca a la tela, puede presionar E para pasar al minijuego de costura(SewingScene).
    private bool canInteract = false;

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.O))
        {
            // Cambia a la escena del minijuego
            SceneManager.LoadScene(sewingSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            Debug.Log("Presiona O para comenzar a coser");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}
