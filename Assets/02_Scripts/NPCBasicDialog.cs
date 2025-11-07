using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    [Tooltip("Texto del botón de opción")]
    public string choiceText;
    
    [Tooltip("Respuesta del NPC cuando eliges esta opción")]
    [TextArea(3, 10)]
    public string responseDialogue;
    
    [Tooltip("Misión a dar después de elegir esta opción (opcional)")]
    public string missionToGive;
    
    [Tooltip("Misión a completar después de elegir esta opción (opcional)")]
    public string missionToComplete;
}

[System.Serializable]
public class NPCMission
{
    [TextArea(3, 10)]
    public string dialogueText;
    public string missionToGive;      // Misión que se da en este diálogo
    public string missionRequired;     // Misión que debe completarse para ver este diálogo
    public string missionToComplete;   // Misión que se completa con este diálogo
    
    [Header("Opciones de Diálogo (opcional)")]
    [Tooltip("Si tiene opciones, se mostrarán botones después del diálogo")]
    public bool hasChoices = false;
    public List<DialogueChoice> choices = new List<DialogueChoice>();
}

public class NPCBasicDialog : MonoBehaviour, IInteractable
{
    public string npcName;
    public Sprite npcImage;
    GameManager manager;
    public GameObject interactUI;

    [Header("Sistema de Misiones")]
    public bool usesMissionSystem = false;
    public List<NPCMission> missions = new List<NPCMission>();
    private int currentMissionIndex = 0; // Índice de la misión actual
    private List<string> missionsAlreadyGiven = new List<string>(); // Misiones que ya se dieron
    private NPCMission currentMissionWithChoices; // Guardar la misión actual si tiene opciones

    [Header("Diálogo Simple (sin misiones)")]
    [TextArea(3, 10)]
    public string dialogueText;

    [Header("Sistema de UI de Opciones (Opcional)")]
    [Tooltip("Arrastra aquí el DialogueChoicesUIFixed para mostrar botones de opciones")]
    public DialogueChoicesUIFixed choicesUI;

    //Max caracteres por página
    public int maxCharactersPerPage = 40;
    //Audio
    public AudioClip typingSound;


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
        if (manager == null)
        {
            Debug.LogError($"No se puede interactuar con {gameObject.name}: GameManager es null");
            return;
        }
        
        string textToShow = dialogueText;
        string missionToAdd = "";
        string missionToCompleteNow = "";
        NPCMission missionToUse = null; // Declarar aquí para que esté disponible en todo el método

        // Si usa misiones, decidir cuál mostrar
        if (usesMissionSystem && missions.Count > 0)
        {
            
            // PRIORIDAD 1: misiones que requieren otra misión activa
            for (int i = 0; i < missions.Count; i++)
            {
                NPCMission mission = missions[i];
                
                if (!string.IsNullOrEmpty(mission.missionRequired) && manager.HasMission(mission.missionRequired))
                {
                    missionToUse = mission;
                    break;
                }
            }
            
            // PRIORIDAD 2: misiones que dan una misión nueva
            if (missionToUse == null)
            {
                for (int i = 0; i < missions.Count; i++)
                {
                    NPCMission mission = missions[i];
                    
                    // Si este diálogo da una misión, verificar que:
                    // 1. El jugador NO la tiene actualmente
                    // 2. Este NPC NO la ha dado antes
                    // 3. NO tiene requisitos (o los cumple si los tiene)
                    if (!string.IsNullOrEmpty(mission.missionToGive) && 
                        !manager.HasMission(mission.missionToGive) &&
                        !missionsAlreadyGiven.Contains(mission.missionToGive))
                    {
                        // Verificar requisitos: si no tiene requisitos se puede mostrar
                        bool canShow = string.IsNullOrEmpty(mission.missionRequired);
                        if (canShow)
                        {
                            missionToUse = mission;
                            break;
                        }
                    }
                }
            }
            
            // PRIORIDAD 3: diálogos simples (sin requisitos ni misiones)
            if (missionToUse == null)
            {
                for (int i = 0; i < missions.Count; i++)
                {
                    NPCMission mission = missions[i];
                    
                    // Diálogo sin requisitos, sin dar misiones, sin completar misiones
                    if (string.IsNullOrEmpty(mission.missionRequired) && 
                        string.IsNullOrEmpty(mission.missionToGive) && 
                        string.IsNullOrEmpty(mission.missionToComplete))
                    {
                        missionToUse = mission;
                        break;
                    }
                }
            }
            
            // Si hay misión válida, usarla
            if (missionToUse != null)
            {
                textToShow = missionToUse.dialogueText;
                missionToCompleteNow = missionToUse.missionToComplete;
                missionToAdd = missionToUse.missionToGive;
            }
        }

        // Dividir el texto en páginas
        List<string> pages = SplitTextIntoPages(textToShow, maxCharactersPerPage);
        manager.NPCShowText(pages, npcName, npcImage, typingSound);

        // Si la misión tiene opciones, mostrarlas después del diálogo
        if (missionToUse != null && missionToUse.hasChoices && missionToUse.choices.Count > 0)
        {
            currentMissionWithChoices = missionToUse;
            StartCoroutine(ShowChoicesAfterDialog());
        }
        // Si no tiene opciones, completar y agregar misiones normalmente
        else if (!string.IsNullOrEmpty(missionToCompleteNow) || !string.IsNullOrEmpty(missionToAdd))
        {
            StartCoroutine(HandleMissionsAfterDialog(missionToCompleteNow, missionToAdd));
        }
    }

    IEnumerator HandleMissionsAfterDialog(string missionToComplete, string missionToAdd)
    {
        // Esperar a que termine el diálogo
        yield return new WaitUntil(() => manager.DialogFinished);
        
        // Primero completar la misión si existe
        if (!string.IsNullOrEmpty(missionToComplete))
        {
            manager.CompleteMission(missionToComplete);
        }
        
        // Luego agregar la nueva misión si existe
        if (!string.IsNullOrEmpty(missionToAdd))
        {
            manager.AddMission(missionToAdd);
            // Registrar que esta misión ya fue dada por este NPC
            if (!missionsAlreadyGiven.Contains(missionToAdd))
            {
                missionsAlreadyGiven.Add(missionToAdd);
            }
        }
    }
    

    public void ShowIndicator(bool state)
    {
        if (interactUI != null)
            interactUI.SetActive(state);
    }

    // Divide el texto largo en páginas más pequeñas
    private List<string> SplitTextIntoPages(string text, int maxChars)
    {
        List<string> pages = new List<string>();

        // Si el texto es corto, devolverlo directamente
        if (text.Length <= maxChars)
        {
            pages.Add(text);
            return pages;
        }

        string[] words = text.Split(' ');
        string currentPage = "";

        foreach (string word in words)
        {
            // Probar si agregar la palabra excede el límite
            string testLine = currentPage.Length == 0 ? word : currentPage + " " + word;

            if (testLine.Length > maxChars)
            {
                // Si la página actual tiene contenido, guardarla
                if (currentPage.Length > 0)
                {
                    pages.Add(currentPage);
                    currentPage = word; // Empezar nueva página con la palabra actual
                }
                else
                {
                    // Si la palabra sola es más larga que maxchars agregar en su propia pagina
                    pages.Add(word);
                    currentPage = "";
                }
            }
            else
            {
                currentPage = testLine;
            }
        }

        // Agregar la última página si tiene contenido
        if (currentPage.Length > 0)
        {
            pages.Add(currentPage);
        }

        return pages;
    }

    // Mostrar opciones después del diálogo
    IEnumerator ShowChoicesAfterDialog()
    {
        // Esperar a que termine el diálogo
        yield return new WaitUntil(() => manager.DialogFinished);
        
        // Completar misión si es necesario (antes de mostrar opciones)
        if (!string.IsNullOrEmpty(currentMissionWithChoices.missionToComplete))
        {
            manager.CompleteMission(currentMissionWithChoices.missionToComplete);
        }
        
        // Pausa breve
        yield return new WaitForSeconds(0.3f);
        
        Debug.Log($"[NPCBasicDialog] Mostrando {currentMissionWithChoices.choices.Count} opciones:");
        for (int i = 0; i < currentMissionWithChoices.choices.Count; i++)
        {
            Debug.Log($"  Opción {i}: {currentMissionWithChoices.choices[i].choiceText}");
        }
        
        // Si hay UI de opciones asignada, usarla
        if (choicesUI != null)
        {
            choicesUI.ShowChoices(this, currentMissionWithChoices.choices);
            Debug.Log("[NPCBasicDialog] ✓ UI de opciones mostrada");
        }
        else
        {
            // Modo testing: auto-seleccionar la primera opción después de 2 segundos
            Debug.LogWarning("[NPCBasicDialog] No hay UI de opciones asignada. Auto-seleccionando opción 0 en 2 segundos...");
            StartCoroutine(AutoSelectFirstChoice());
        }
    }
    
    IEnumerator AutoSelectFirstChoice()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("[NPCBasicDialog] Auto-seleccionando opción 0");
        OnChoiceSelected(0);
    }
    
    // Callback cuando el jugador elige una opción
    public void OnChoiceSelected(int choiceIndex)
    {
        if (currentMissionWithChoices == null || choiceIndex < 0 || choiceIndex >= currentMissionWithChoices.choices.Count)
        {
            Debug.LogError($"[NPCBasicDialog] Índice de opción inválido: {choiceIndex}");
            return;
        }
        
        DialogueChoice selectedChoice = currentMissionWithChoices.choices[choiceIndex];
        Debug.Log($"[NPCBasicDialog] Jugador eligió: {selectedChoice.choiceText}");
        
        // ⭐ CONTADOR GLOBAL - Usa solo el índice de la opción (0, 1, 2, etc.)
        // Todas las "Opción 0" de todos los NPCs suman al mismo contador
        if (ChoiceCounterManager.Instance != null)
        {
            // Nombres personalizados para cada opción
            string[] optionNames = { "SaberSobreElCirco", "SaberSobreMi" };
            
            string globalChoiceID = choiceIndex < optionNames.Length 
                ? optionNames[choiceIndex] 
                : $"Opcion_{choiceIndex}";
                
            ChoiceCounterManager.Instance.IncrementChoice(globalChoiceID);
        }
        else
        {
            Debug.LogWarning("[NPCBasicDialog] ChoiceCounterManager no encontrado en la escena");
        }
        
        // Mostrar respuesta del NPC
        StartCoroutine(ShowChoiceResponse(selectedChoice));
    }
    
    IEnumerator ShowChoiceResponse(DialogueChoice choice)
    {
        // Pausa breve
        yield return new WaitForSeconds(0.3f);
        
        // Mostrar respuesta
        List<string> pages = SplitTextIntoPages(choice.responseDialogue, maxCharactersPerPage);
        manager.NPCShowText(pages, npcName, npcImage, typingSound);
        
        // Esperar a que termine
        yield return new WaitUntil(() => manager.DialogFinished);
        
        // Completar o dar misión según la opción
        if (!string.IsNullOrEmpty(choice.missionToComplete))
        {
            manager.CompleteMission(choice.missionToComplete);
        }
        
        if (!string.IsNullOrEmpty(choice.missionToGive))
        {
            manager.AddMission(choice.missionToGive);
            if (!missionsAlreadyGiven.Contains(choice.missionToGive))
            {
                missionsAlreadyGiven.Add(choice.missionToGive);
            }
        }
        
        currentMissionWithChoices = null;
    }
}