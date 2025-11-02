using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCDialogueWithChoices : MonoBehaviour, IInteractable
{
    [System.Serializable]
    public class DialogueChoice
    {
    [Tooltip("Texto de la opción")]
        public string choiceText;
        
    [Tooltip("Respuesta del NPC")]
        [TextArea(3, 10)]
        public string responseDialogue;
    }

    [Header("NPC")]
    public string npcName = "NPC";
    public Sprite npcImage;
    public GameObject interactUI;

    [Header("Misión")]
    [Tooltip("Misión necesaria")]
    public string missionRequired = "";
    
    [Tooltip("Completa misión al iniciar")]
    public bool completeMissionOnStart = true;

    [Header("Diálogo Inicial")]
    [Tooltip("Texto inicial")]
    [TextArea(3, 10)]
    public string initialDialogue;

    [Header("Opciones")]
    [Tooltip("Opciones para elegir")]
    public List<DialogueChoice> choices = new List<DialogueChoice>();

    [Header("Diálogo Por Defecto")]
    [Tooltip("Si no tiene misión")]
    [TextArea(3, 10)]
    public string defaultDialogue = "Hola.";

    [Header("Audio")]
    public AudioClip typingSound;

    [Header("Avanzado")]
    public int maxCharactersPerPage = 40;

    private GameManager manager;
    private bool hasShownChoices = false; // Solo una vez

    void Start()
    {
        manager = GameManager.Instance;
        
        if (manager == null)
        {
            Debug.LogError($"GameManager.Instance es null para {gameObject.name}");
        }

    // Validar opciones
        if (choices.Count < 2)
        {
            Debug.LogWarning($"[{gameObject.name}] Se necesitan al menos 2 opciones de diálogo. Actualmente hay {choices.Count}");
        }
    }

    public void Interact()
    {
        if (manager == null)
        {
            Debug.LogError($"No se puede interactuar con {gameObject.name}: GameManager es null");
            return;
        }

    // Checar misión
        bool hasMission = string.IsNullOrEmpty(missionRequired) || manager.HasMission(missionRequired);

        if (hasMission && !hasShownChoices)
        {
            // Mostrar opciones
            StartCoroutine(ShowDialogueWithChoices());
        }
        else if (hasMission && hasShownChoices)
        {
            // Ya eligió antes
            ShowSimpleDialogue("Ya hemos hablado de esto. ¡Gracias por tu ayuda!");
        }
        else
        {
            // No tiene misión
            ShowSimpleDialogue(defaultDialogue);
        }
    }

    private IEnumerator ShowDialogueWithChoices()
    {
    // Completar misión
        if (completeMissionOnStart && !string.IsNullOrEmpty(missionRequired))
        {
            if (manager.HasMission(missionRequired))
            {
                manager.CompleteMission(missionRequired);
                Debug.Log($"[{gameObject.name}] Misión completada: {missionRequired}");
            }
        }

    // Mostrar inicial
        List<string> initialPages = SplitTextIntoPages(initialDialogue, maxCharactersPerPage);
        manager.NPCShowText(initialPages, npcName, npcImage, typingSound);

    // Esperar diálogo
        yield return new WaitUntil(() => manager.DialogFinished);

    // Pausa
        yield return new WaitForSeconds(0.5f);

    // Mostrar opciones UI
        ShowChoicesUI();

        hasShownChoices = true;
    }

    private void ShowChoicesUI()
    {
       
        Debug.Log($"[{gameObject.name}] Mostrando {choices.Count} opciones:");
        for (int i = 0; i < choices.Count; i++)
        {
            Debug.Log($"  Opción {i + 1}: {choices[i].choiceText}");
        }

       
    }

     public void OnChoiceSelected(int choiceIndex)
    {
        if (choiceIndex < 0 || choiceIndex >= choices.Count)
        {
            Debug.LogError($"[{gameObject.name}] Índice de opción inválido: {choiceIndex}");
            return;
        }

        DialogueChoice selectedChoice = choices[choiceIndex];
        Debug.Log($"[{gameObject.name}] Jugador eligió: {selectedChoice.choiceText}");

    // Mostrar respuesta
        StartCoroutine(ShowResponseDialogue(selectedChoice.responseDialogue));
    }

    private IEnumerator ShowResponseDialogue(string responseText)
    {
    // Pausa
        yield return new WaitForSeconds(0.3f);

    // Mostrar respuesta
        List<string> responsePages = SplitTextIntoPages(responseText, maxCharactersPerPage);
        manager.NPCShowText(responsePages, npcName, npcImage, typingSound);
    }

    private void ShowSimpleDialogue(string text)
    {
        List<string> pages = SplitTextIntoPages(text, maxCharactersPerPage);
        manager.NPCShowText(pages, npcName, npcImage, typingSound);
    }

    // Divide texto en páginas
    private List<string> SplitTextIntoPages(string text, int maxChars)
    {
        List<string> pages = new List<string>();

        if (string.IsNullOrEmpty(text))
        {
            Debug.LogWarning("Texto vacío en SplitTextIntoPages");
            return pages;
        }

        if (text.Length <= maxChars)
        {
            pages.Add(text);
            return pages;
        }

        string[] words = text.Split(' ');
        string currentPage = "";

        foreach (string word in words)
        {
            string testLine = currentPage.Length == 0 ? word : currentPage + " " + word;

            if (testLine.Length > maxChars)
            {
                if (currentPage.Length > 0)
                {
                    pages.Add(currentPage);
                    currentPage = word;
                }
                else
                {
                    pages.Add(word);
                    currentPage = "";
                }
            }
            else
            {
                currentPage = testLine;
            }
        }

        if (currentPage.Length > 0)
        {
            pages.Add(currentPage);
        }

        return pages;
    }

    public void ShowIndicator(bool state)
    {
        if (interactUI != null)
            interactUI.SetActive(state);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowIndicator(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowIndicator(false);
        }
    }
}
