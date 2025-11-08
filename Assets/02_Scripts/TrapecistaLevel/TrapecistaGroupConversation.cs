using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapecistaGroupConversation : MonoBehaviour, IInteractable
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
    public string groupName = "Trapecista"; // Nombre del grupo
    public Sprite groupIcon; // Icono del grupo
    public GameObject interactUI;

    [Header("Líneas")]

    [Header("Inicial")]
    public List<DialogueLine> initialConversation = new List<DialogueLine>();

    [Header("Después de Misión")]
    public string missionRequired; // Misión necesaria
    public List<DialogueLine> afterMissionConversation = new List<DialogueLine>();

    [Header("Final")]
    public string missionRequiredForFinalConversation; // Misión final
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
    public bool isActive = true;

    private GameManager manager;
    private bool hasInteracted = false;
    private bool hasGivenInitialMission = false; // Solo una vez

    void Start()
    {
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
        bool shouldShowFinalConversation = false;

        // Conversación final
        if (!string.IsNullOrEmpty(missionRequiredForFinalConversation) &&
            manager.HasMission(missionRequiredForFinalConversation))
        {
            if (finalConversation.Count > 0)
            {
                conversationToShow = finalConversation;
                shouldShowFinalConversation = true;
                Debug.Log($"[{gameObject.name}] Mostrando conversación FINAL del Trapecista");
            }
        }
        // Después de misión
        else if (!string.IsNullOrEmpty(missionRequired) && manager.HasMission(missionRequired))
        {
            if (afterMissionConversation.Count > 0)
            {
                conversationToShow = afterMissionConversation;
                shouldShowAfterConversation = true;
                Debug.Log($"[{gameObject.name}] Mostrando conversación DESPUÉS DE MISIÓN del Trapecista");
            }
        }
        // Inicial
        else
        {
            conversationToShow = initialConversation;
            if (!hasGivenInitialMission && !string.IsNullOrEmpty(missionToGiveAfterInitial))
            {
                shouldGiveInitialMission = true;
            }
            Debug.Log($"[{gameObject.name}] Mostrando conversación INICIAL del Trapecista");
        }

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
            List<string> singlePage = new List<string> { line.dialogue };
            AudioClip soundToUse = line.speakerSound != null ? line.speakerSound : typingSound;
            manager.NPCShowText(singlePage, line.speakerName, line.speakerSprite, soundToUse);

            yield return new WaitUntil(() => manager.DialogFinished);
            yield return new WaitForSeconds(0.3f);
        }

        if (giveInitialMission)
        {
            manager.AddMission(missionToGiveAfterInitial);
            hasGivenInitialMission = true;
            Debug.Log($"[{gameObject.name}] Misión inicial dada al Trapecista: {missionToGiveAfterInitial}");
        }

        if (isAfterMissionConversation && !string.IsNullOrEmpty(missionToGiveAfterSecond))
        {
            manager.AddMission(missionToGiveAfterSecond);
            Debug.Log($"[{gameObject.name}] Misión posterior dada al Trapecista: {missionToGiveAfterSecond}");
        }

        if (isFinalConversation && !string.IsNullOrEmpty(missionToGiveAfterFinal))
        {
            manager.AddMission(missionToGiveAfterFinal);
            Debug.Log($"[{gameObject.name}] Misión FINAL dada al Trapecista: {missionToGiveAfterFinal}");
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }

    public bool HasInteracted()
    {
        return hasInteracted;
    }

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
