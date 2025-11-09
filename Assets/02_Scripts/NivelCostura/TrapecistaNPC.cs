using UnityEngine;
using System.Collections;

public class TrapecistaNPC : MonoBehaviour
{
    [Header("Dialogue Configuration")]
    public string initialDialogue = "¿Podrías ayudarme a coser esta tela?";
    public string afterSewingDialogue = "¡Gracias por ayudarme! La tela quedó perfecta.";
    
    [Header("Scene Transition")]
    public string sewingSceneName = "SewingScene";
    public float transitionDelay = 1.5f;
    public KeyCode interactionKey = KeyCode.O;
    
    private NPCBasicDialog npcDialog;
    private bool playerInRange = false;
    private bool dialogueShown = false;
    private FadeController fadeController;

    void Start()
    {
        npcDialog = GetComponent<NPCBasicDialog>();
        fadeController = FindObjectOfType<FadeController>();
        
        if (npcDialog != null)
        {
            npcDialog.dialogueText = initialDialogue;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            ShowDialogue();
        }

        // After showing dialogue, pressing L will start the sewing minigame
        if (dialogueShown && Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(TransitionToSewingScene());
        }
    }

    void ShowDialogue()
    {
        if (npcDialog != null)
        {
            npcDialog.Interact();
            dialogueShown = true;
            Debug.Log("Presiona L para comenzar a coser");
        }
    }

    IEnumerator TransitionToSewingScene()
    {
        if (fadeController != null)
        {
            fadeController.FadeOut();
        }
        
        yield return new WaitForSeconds(transitionDelay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sewingSceneName);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Presiona O para hablar con la Trapecista");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueShown = false;
        }
    }
}