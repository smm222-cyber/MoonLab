using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCGroupConversation : MonoBehaviour, IInteractable
{
    [System.Serializable]
    public class DialogueLine
    {
    public string speakerName; // Nombre
    public Sprite speakerSprite; // Imagen
        
    [Tooltip("Sonido específico (opcional)")]
    public AudioClip speakerSound; // Sonido
        
    [TextArea(3, 10)]
    public string dialogue; // Diálogo
    }

    [Header("Grupo")]
    public string groupName = "Conversación"; // Nombre
    public Sprite groupIcon; // Icono
    public GameObject interactUI;

    [Header("Líneas")]

    [Header("Inicial")]
    public List<DialogueLine> initialConversation = new List<DialogueLine>();
    
    [Header("Después de Misión")]
    public string missionRequired; // Misión necesaria
    public List<DialogueLine> afterMissionConversation = new List<DialogueLine>();

    [Header("Final")]
    public string missionRequiredForFinalConversation; // Misión para final
    public List<DialogueLine> finalConversation = new List<DialogueLine>();

    [Header("Misiones")]
    [Tooltip("Misión tras inicial")]
    public string missionToGiveAfterInitial;
    [Tooltip("Completa tras después")]
    public string missionToCompleteWhenShowingAfter;
    [Tooltip("Misión tras después")]
    public string missionToGiveAfterSecond;
    [Tooltip("Completa tras final")]
    public string missionToCompleteInFinalConversation;
    [Tooltip("Misión tras final")]
    public string missionToGiveAfterFinal;

    [Header("Audio")]
    public AudioClip typingSound;

    [Header("Activación")]
    public bool isActive = true; // Activo

    private GameManager manager;
    private bool hasInteracted = false;
    private bool hasGivenInitialMission = false; // Solo una vez

    void Start()
    {
    // Usar GameManager
        manager = GameManager.Instance;
        
        if (manager == null)
        {
            Debug.LogError($"GameManager.Instance es null para {gameObject.name}. Asegúrate de que el GameManager existe y se inicializa primero.");
        }
    }

    public void Interact()
    {
        if (!isActive) return;
        
        if (manager == null)
        {
            Debug.LogError($"No se puede interactuar con {gameObject.name}: GameManager es null");
            return;
        }

        hasInteracted = true;

    // Qué conversación mostrar
        List<DialogueLine> conversationToShow = initialConversation;
        bool shouldGiveInitialMission = false;
        bool shouldShowAfterConversation = false;
        bool shouldShowFinalConversation = false;

    // Final
        if (!string.IsNullOrEmpty(missionRequiredForFinalConversation) && 
            manager.HasMission(missionRequiredForFinalConversation))
        {
            if (finalConversation.Count > 0)
            {
                conversationToShow = finalConversation;
                shouldShowFinalConversation = true;
                Debug.Log($"[{gameObject.name}] Mostrando conversación FINAL");
            }
        }
    // Después de misión
        else if (!string.IsNullOrEmpty(missionRequired) && manager.HasMission(missionRequired))
        {
            if (afterMissionConversation.Count > 0)
            {
                conversationToShow = afterMissionConversation;
                shouldShowAfterConversation = true;
                Debug.Log($"[{gameObject.name}] Mostrando conversación DESPUÉS DE MISIÓN");
            }
        }
    // Inicial
        else
        {
            conversationToShow = initialConversation;
            // Solo una vez
            if (!hasGivenInitialMission && !string.IsNullOrEmpty(missionToGiveAfterInitial))
            {
                shouldGiveInitialMission = true;
            }
            Debug.Log($"[{gameObject.name}] Mostrando conversación INICIAL");
        }

    // Mostrar diálogos
        StartCoroutine(ShowGroupConversation(conversationToShow, shouldGiveInitialMission, shouldShowAfterConversation, shouldShowFinalConversation));
    }

    private IEnumerator ShowGroupConversation(List<DialogueLine> conversation, bool giveInitialMission, bool isAfterMissionConversation, bool isFinalConversation)
    {
    // Completar misión después
        if (isAfterMissionConversation && !string.IsNullOrEmpty(missionToCompleteWhenShowingAfter))
        {
            manager.CompleteMission(missionToCompleteWhenShowingAfter);
            Debug.Log($"[{gameObject.name}] Misión completada ANTES del diálogo: {missionToCompleteWhenShowingAfter}");
        }

    // Completar misión final
        if (isFinalConversation && !string.IsNullOrEmpty(missionToCompleteInFinalConversation))
        {
            manager.CompleteMission(missionToCompleteInFinalConversation);
            Debug.Log($"[{gameObject.name}] Misión FINAL completada: {missionToCompleteInFinalConversation}");
        }

        foreach (DialogueLine line in conversation)
        {
            // Una página
            List<string> singlePage = new List<string> { line.dialogue };
            
            // Sonido específico o general
            AudioClip soundToUse = line.speakerSound != null ? line.speakerSound : typingSound;
            
            // Mostrar diálogo
            manager.NPCShowText(singlePage, line.speakerName, line.speakerSprite, soundToUse);

            // Esperar diálogo
            yield return new WaitUntil(() => manager.DialogFinished);
            
            // Pausa
            yield return new WaitForSeconds(0.3f);
        }

    // Dar misión inicial
        if (giveInitialMission)
        {
            manager.AddMission(missionToGiveAfterInitial);
            hasGivenInitialMission = true; // Marcar que ya se dio la misión
            Debug.Log($"[{gameObject.name}] Misión inicial dada: {missionToGiveAfterInitial}");
        }
        
    // Dar misión después
        if (isAfterMissionConversation && !string.IsNullOrEmpty(missionToGiveAfterSecond))
        {
            manager.AddMission(missionToGiveAfterSecond);
            Debug.Log($"[{gameObject.name}] Misión posterior dada: {missionToGiveAfterSecond}");
        }
        
    // Dar misión final
        if (isFinalConversation && !string.IsNullOrEmpty(missionToGiveAfterFinal))
        {
            manager.AddMission(missionToGiveAfterFinal);
            Debug.Log($"[{gameObject.name}] Misión FINAL dada: {missionToGiveAfterFinal}");
        }
    }

    // Activar/desactivar
    public void SetActive(bool active)
    {
        isActive = active;
    }

    // ¿Ya interactuó?
    public bool HasInteracted()
    {
        return hasInteracted;
    }

    // IInteractable
    public void ShowIndicator(bool state)
    {
        if (interactUI != null)
        {
            interactUI.SetActive(state && isActive);
        }
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
