using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script para manejar conversaciones entre múltiples NPCs que están juntos.
/// El jugador puede presionar E para escuchar su conversación.
/// </summary>
public class NPCGroupConversation : MonoBehaviour, IInteractable
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName; // Nombre del NPC que habla
        public Sprite speakerSprite; // Imagen del NPC que habla
        [TextArea(3, 10)]
        public string dialogue; // Lo que dice
    }

    [Header("Configuración de la Conversación Grupal")]
    public string groupName = "Conversación"; // Nombre que aparece en el diálogo
    public Sprite groupIcon; // Icono que representa al grupo
    public GameObject interactUI;

    [Header("Líneas de Diálogo")]

    [Header("Conversación Inicial (antes de completar misión)")]
    public List<DialogueLine> initialConversation = new List<DialogueLine>();
    
    [Header("Conversación Después de Misión")]
    public string missionRequired; // Misión que debe completarse para cambiar el diálogo
    public List<DialogueLine> afterMissionConversation = new List<DialogueLine>();

    [Header("Sistema de Misiones")]
    [Tooltip("Misión que se agrega al completar la conversación inicial")]
    public string missionToGiveAfterInitial;
    [Tooltip("Misión que se completa al iniciar esta conversación después de tener missionRequired")]
    public string missionToCompleteWhenShowingAfter;
    [Tooltip("Misión que se da después de la conversación posterior")]
    public string missionToGiveAfterSecond;

    [Header("Audio")]
    public AudioClip typingSound;

    [Header("Control de Activación")]
    public bool isActive = true; // Si es false, esta conversación no se puede activar

    private GameManager manager;
    private bool hasInteracted = false;
    private bool hasGivenInitialMission = false; // Para evitar dar la misión inicial múltiples veces

    void Start()
    {
        // Usar la instancia singleton del GameManager
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

        // Determinar qué conversación mostrar
        List<DialogueLine> conversationToShow = initialConversation;
        bool shouldGiveInitialMission = false;
        bool shouldShowAfterConversation = false;

        // Si hay una misión requerida Y está activa, mostrar la conversación alternativa
        if (!string.IsNullOrEmpty(missionRequired) && manager.HasMission(missionRequired))
        {
            if (afterMissionConversation.Count > 0)
            {
                conversationToShow = afterMissionConversation;
                shouldShowAfterConversation = true;
            }
        }
        else
        {
            // Mostrar conversación inicial
            conversationToShow = initialConversation;
            // Solo dar la misión inicial si no se ha dado antes
            if (!hasGivenInitialMission && !string.IsNullOrEmpty(missionToGiveAfterInitial))
            {
                shouldGiveInitialMission = true;
            }
        }

        // Convertir la conversación en diálogos para el sistema
        StartCoroutine(ShowGroupConversation(conversationToShow, shouldGiveInitialMission, shouldShowAfterConversation));
    }

    private IEnumerator ShowGroupConversation(List<DialogueLine> conversation, bool giveInitialMission, bool isAfterMissionConversation)
    {
        // Si es la conversación posterior y hay misión para completar, completarla ANTES del diálogo
        if (isAfterMissionConversation && !string.IsNullOrEmpty(missionToCompleteWhenShowingAfter))
        {
            manager.CompleteMission(missionToCompleteWhenShowingAfter);
        }

        foreach (DialogueLine line in conversation)
        {
            // Crear una lista con una sola línea para cada personaje
            List<string> singlePage = new List<string> { line.dialogue };
            
            // Mostrar el diálogo
            manager.NPCShowText(singlePage, line.speakerName, line.speakerSprite, typingSound);

            // Esperar hasta que el diálogo termine
            yield return new WaitUntil(() => manager.DialogFinished);
            
            // Pequeña pausa entre diálogos
            yield return new WaitForSeconds(0.3f);
        }

        // Si debemos dar la misión inicial (solo la primera vez)
        if (giveInitialMission)
        {
            manager.AddMission(missionToGiveAfterInitial);
            hasGivenInitialMission = true; // Marcar que ya se dio la misión
            Debug.Log($"[{gameObject.name}] Misión inicial dada: {missionToGiveAfterInitial}");
        }
        
        // Si era la conversación posterior y hay una misión para dar, agregarla
        if (isAfterMissionConversation && !string.IsNullOrEmpty(missionToGiveAfterSecond))
        {
            manager.AddMission(missionToGiveAfterSecond);
            Debug.Log($"[{gameObject.name}] Misión posterior dada: {missionToGiveAfterSecond}");
        }
    }

    // Para activar/desactivar esta conversación desde otros scripts
    public void SetActive(bool active)
    {
        isActive = active;
    }

    // Verificar si ya se interactuó con este grupo
    public bool HasInteracted()
    {
        return hasInteracted;
    }

    // Implementación de IInteractable
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
