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

    [Header("Audio")]
    public AudioClip typingSound;

    [Header("Control de Activación")]
    public bool isActive = true; // Si es false, esta conversación no se puede activar

    private GameManager manager;
    private bool hasInteracted = false;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public void Interact()
    {
        if (!isActive) return;

        hasInteracted = true;

        // Determinar qué conversación mostrar
        List<DialogueLine> conversationToShow = initialConversation;

        // Si hay una misión requerida Y está activa (completada), mostrar la conversación alternativa
        if (!string.IsNullOrEmpty(missionRequired) && manager.HasMission(missionRequired))
        {
            if (afterMissionConversation.Count > 0)
            {
                conversationToShow = afterMissionConversation;
            }
        }

        // Convertir la conversación en diálogos para el sistema
        StartCoroutine(ShowGroupConversation(conversationToShow));
    }

    private IEnumerator ShowGroupConversation(List<DialogueLine> conversation)
    {
        bool isInitialConversation = (conversation == initialConversation);

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

        // Si era la conversación inicial y hay una misión para dar, agregarla
        if (isInitialConversation && !string.IsNullOrEmpty(missionToGiveAfterInitial))
        {
            manager.AddMission(missionToGiveAfterInitial);
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
