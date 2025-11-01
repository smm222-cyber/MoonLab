using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script para NPCs que ofrecen opciones de diálogo al jugador.
/// El jugador puede elegir entre diferentes respuestas y obtener diferentes diálogos según su elección.
/// </summary>
public class NPCDialogueWithChoices : MonoBehaviour, IInteractable
{
    [System.Serializable]
    public class DialogueChoice
    {
        [Tooltip("Texto de la opción que verá el jugador")]
        public string choiceText;
        
        [Tooltip("Diálogo que dice el NPC después de elegir esta opción")]
        [TextArea(3, 10)]
        public string responseDialogue;
    }

    [Header("Información del NPC")]
    public string npcName = "NPC";
    public Sprite npcImage;
    public GameObject interactUI;

    [Header("Misión Requerida")]
    [Tooltip("Misión que debe estar activa para mostrar este diálogo con opciones")]
    public string missionRequired = "";
    
    [Tooltip("Si es true, completa la misión cuando se inicia el diálogo")]
    public bool completeMissionOnStart = true;

    [Header("Diálogo Inicial")]
    [Tooltip("Texto que dice el NPC antes de mostrar las opciones")]
    [TextArea(3, 10)]
    public string initialDialogue;

    [Header("Opciones de Diálogo")]
    [Tooltip("Lista de opciones que el jugador puede elegir (mínimo 2)")]
    public List<DialogueChoice> choices = new List<DialogueChoice>();

    [Header("Diálogo Por Defecto")]
    [Tooltip("Diálogo que se muestra si no tiene la misión requerida")]
    [TextArea(3, 10)]
    public string defaultDialogue = "Hola.";

    [Header("Audio")]
    public AudioClip typingSound;

    [Header("Configuración Avanzada")]
    public int maxCharactersPerPage = 40;

    private GameManager manager;
    private bool hasShownChoices = false; // Para mostrar las opciones solo una vez

    void Start()
    {
        manager = GameManager.Instance;
        
        if (manager == null)
        {
            Debug.LogError($"GameManager.Instance es null para {gameObject.name}");
        }

        // Validar configuración
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

        // Verificar si tiene la misión requerida
        bool hasMission = string.IsNullOrEmpty(missionRequired) || manager.HasMission(missionRequired);

        if (hasMission && !hasShownChoices)
        {
            // Mostrar diálogo con opciones
            StartCoroutine(ShowDialogueWithChoices());
        }
        else if (hasMission && hasShownChoices)
        {
            // Ya eligió una opción antes, mostrar un diálogo genérico
            ShowSimpleDialogue("Ya hemos hablado de esto. ¡Gracias por tu ayuda!");
        }
        else
        {
            // No tiene la misión requerida
            ShowSimpleDialogue(defaultDialogue);
        }
    }

    private IEnumerator ShowDialogueWithChoices()
    {
        // Completar la misión si está configurado
        if (completeMissionOnStart && !string.IsNullOrEmpty(missionRequired))
        {
            if (manager.HasMission(missionRequired))
            {
                manager.CompleteMission(missionRequired);
                Debug.Log($"[{gameObject.name}] Misión completada: {missionRequired}");
            }
        }

        // Mostrar diálogo inicial
        List<string> initialPages = SplitTextIntoPages(initialDialogue, maxCharactersPerPage);
        manager.NPCShowText(initialPages, npcName, npcImage, typingSound);

        // Esperar a que termine el diálogo inicial
        yield return new WaitUntil(() => manager.DialogFinished);

        // Pequeña pausa
        yield return new WaitForSeconds(0.5f);

        // Aquí deberías llamar a tu sistema de UI para mostrar las opciones
        // Como no conozco tu sistema de UI, voy a crear un método que puedes adaptar
        ShowChoicesUI();

        hasShownChoices = true;
    }

    private void ShowChoicesUI()
    {
        // TODO: Implementar tu sistema de UI de opciones
        // Por ahora, solo loggeamos las opciones disponibles
        Debug.Log($"[{gameObject.name}] Mostrando {choices.Count} opciones:");
        for (int i = 0; i < choices.Count; i++)
        {
            Debug.Log($"  Opción {i + 1}: {choices[i].choiceText}");
        }

        // TEMPORAL: Para testing, simular que el jugador elige la primera opción
        // Borra esto cuando implementes tu UI de opciones
        if (choices.Count > 0)
        {
            Debug.LogWarning($"[{gameObject.name}] Sistema de UI de opciones no implementado. Usando opción 1 por defecto.");
            OnChoiceSelected(0);
        }
    }

    /// <summary>
    /// Llama a este método desde tu sistema de UI cuando el jugador elija una opción
    /// </summary>
    /// <param name="choiceIndex">Índice de la opción elegida (0-based)</param>
    public void OnChoiceSelected(int choiceIndex)
    {
        if (choiceIndex < 0 || choiceIndex >= choices.Count)
        {
            Debug.LogError($"[{gameObject.name}] Índice de opción inválido: {choiceIndex}");
            return;
        }

        DialogueChoice selectedChoice = choices[choiceIndex];
        Debug.Log($"[{gameObject.name}] Jugador eligió: {selectedChoice.choiceText}");

        // Mostrar el diálogo de respuesta
        StartCoroutine(ShowResponseDialogue(selectedChoice.responseDialogue));
    }

    private IEnumerator ShowResponseDialogue(string responseText)
    {
        // Pequeña pausa antes de mostrar la respuesta
        yield return new WaitForSeconds(0.3f);

        // Mostrar el diálogo de respuesta
        List<string> responsePages = SplitTextIntoPages(responseText, maxCharactersPerPage);
        manager.NPCShowText(responsePages, npcName, npcImage, typingSound);
    }

    private void ShowSimpleDialogue(string text)
    {
        List<string> pages = SplitTextIntoPages(text, maxCharactersPerPage);
        manager.NPCShowText(pages, npcName, npcImage, typingSound);
    }

    // Divide el texto largo en páginas más pequeñas
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
