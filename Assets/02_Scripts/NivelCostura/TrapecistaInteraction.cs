using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapecistaInteraction : MonoBehaviour
{
    [Header("Configuración de diálogo")]
    [TextArea]
    public string dialogueLine = "Cose mi tela, por favor.";
    public KeyCode interactionKey = KeyCode.O;
    public KeyCode goToSewingKey = KeyCode.L;

    [Header("Escena de costura")]
    public string sewingSceneName = "SewingScene";
    public float delayBeforeLoad = 1.5f;

    private bool playerInRange = false;
    private bool dialogueShown = false;

    private NPCBasicDialog npcDialog;

    void Start()
    {
        npcDialog = FindObjectOfType<NPCBasicDialog>();
    }

    void Update()
    {
        // Interactuar con el trapecista
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            ShowDialogue();
        }

        // Ir al minijuego solo si ya se mostró el diálogo
        if (dialogueShown && Input.GetKeyDown(goToSewingKey))
        {
            StartCoroutine(LoadSewingSceneWithDelay());
        }
    }

    private void ShowDialogue()
    {
        if (npcDialog != null)
        {
            npcDialog.Interact();
            dialogueShown = true;
        }
        else
        {
            Debug.LogWarning("NPCBasicDialog no encontrado en la escena.");
        }
    }

    private IEnumerator LoadSewingSceneWithDelay()
    {
        // Espera un tiempo antes de cambiar de escena
        yield return new WaitForSeconds(delayBeforeLoad);

        // Carga segura de la escena
        SceneManager.LoadScene(sewingSceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Presiona O para hablar con la Trapecista.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
