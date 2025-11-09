using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

    [Header("Cambio de Escena (opcional)")]
    [Tooltip("Si está activado, cambia de escena después de mostrar la respuesta de esta opción")]
    public bool loadSceneAfterChoice = false;
    
    [Tooltip("Nombre de la escena a cargar (debe estar en Build Settings)")]
    public string sceneToLoad = "";
    
    [Tooltip("Segundos de espera antes de cambiar de escena")]
    public float delayBeforeChange = 1.5f;
    
    [Header("Contador global (opcional)")]
    [Tooltip("Clave del contador global que se incrementará al elegir esta opción. Si está vacío, se usará el mapeo por índice (legacy). Ej: 'SaberSobreElCirco' o 'SaberSobreMi'")]
    public string globalChoiceID = "";
    
    [Tooltip("Si está activado, esta opción disparará la lógica de final (decidir y cargar el final según contadores) después de ejecutarse.")]
    public bool triggersFinal = false;
    
    [Tooltip("Si está activado, y esta opción da una misión, el sistema esperará a que GameManager registre la misión antes de decidir el final (evita race conditions). Conserva compatibilidad con la misión 'Abrir portal secreto'.")]
    public bool waitForMissionRegistration = false;
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

    [Header("Cambio de Escena (opcional)")]
    [Tooltip("Si está activado, cambia de escena después de este diálogo (solo si NO tiene opciones)")]
    public bool loadSceneAfterDialog = false;
    
    [Tooltip("Nombre de la escena a cargar (debe estar en Build Settings)")]
    public string sceneToLoad = "";
    
    [Tooltip("Segundos de espera antes de cambiar de escena")]
    public float delayBeforeChange = 1.5f;
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

    [Header("Cambio de Escena para Diálogo Simple")]
    [Tooltip("Si está activado, cambia de escena después del diálogo simple (solo funciona si NO usas sistema de misiones)")]
    public bool loadSceneAfterSimpleDialog = false;
    
    [Tooltip("Nombre de la escena a cargar (debe estar en Build Settings)")]
    public string simpleDialogSceneToLoad = "";
    
    [Tooltip("Segundos de espera antes de cambiar de escena")]
    public float simpleDialogDelayBeforeChange = 1.5f;

    [Header("Sistema de UI de Opciones (Opcional)")]
    [Tooltip("Arrastra aquí el DialogueChoicesUIFixed para mostrar botones de opciones")]
    public DialogueChoicesUIFixed choicesUI;

    [Header("Eventos")]
    [Tooltip("Se invoca cuando el diálogo (y las acciones de misión asociadas) han terminado. Úsalo para dar items automáticamente u otras acciones.")]
    public UnityEvent onDialogFinished;

    [Tooltip("Si está activado, decidir y cargar el final inmediatamente después de que termine el diálogo con este NPC (útil para el Maestro).")]
    public bool triggerFinalAfterDialog = false;

    // Evitar disparos múltiples del final desde este NPC
    private bool finalTriggered = false;

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

    // Esperar hasta que GameManager registre la misión (o timeout) y luego decidir el final
    IEnumerator WaitForMissionAndDecide(string missionName)
    {
        float timeout = 1.0f; // segundos
        float waited = 0f;

        // esperar hasta que GameManager exista
        while (GameManager.Instance == null && waited < timeout)
        {
            yield return null;
            waited += Time.unscaledDeltaTime;
        }

        // esperar hasta que la misión esté registrada o timeout
        waited = 0f;
        while ((GameManager.Instance == null || !GameManager.Instance.HasMission(missionName)) && waited < timeout)
        {
            yield return null;
            waited += Time.unscaledDeltaTime;
        }

        Debug.Log($"[NPCBasicDialog] WaitForMissionAndDecide: comprobado misión '{missionName}' (registered={GameManager.Instance != null && GameManager.Instance.HasMission(missionName)})");

        // Llamar al helper para decidir y cargar el final
        MissionToEndingChooserHelper.DecideAndLoad();
        yield break;
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
            StartCoroutine(HandleMissionsAfterDialog(missionToCompleteNow, missionToAdd, missionToUse));
        }
        // Si no hay misiones pero hay missionToUse (para cambio de escena sin misiones)
        else if (missionToUse != null)
        {
            StartCoroutine(HandleMissionsAfterDialog("", "", missionToUse));
        }
        // Si usa diálogo simple (sin sistema de misiones), manejar cambio de escena
        else if (!usesMissionSystem)
        {
            StartCoroutine(HandleSimpleDialogEnd());
        }
    }

    IEnumerator HandleMissionsAfterDialog(string missionToComplete, string missionToAdd, NPCMission mission)
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
        // Invocar eventos adicionales (por ejemplo: dar un item automáticamente)
        try
        {
            onDialogFinished?.Invoke();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error invoking onDialogFinished for {gameObject.name}: {ex}");
        }

        // Si este NPC está configurado para disparar el final tras el diálogo, hacerlo aquí.
        if (triggerFinalAfterDialog && !finalTriggered)
        {
            finalTriggered = true;
            // Si acabamos de añadir una misión, esperar a que GameManager la registre antes de decidir
            if (!string.IsNullOrEmpty(missionToAdd))
            {
                StartCoroutine(WaitForMissionAndDecide(missionToAdd));
                yield break;
            }
            else
            {
                MissionToEndingChooserHelper.DecideAndLoad();
                yield break;
            }
        }

        // Cambiar de escena si esta misión lo tiene configurado
        if (mission != null && mission.loadSceneAfterDialog && !string.IsNullOrEmpty(mission.sceneToLoad))
        {
            yield return new WaitForSeconds(mission.delayBeforeChange);
            Debug.Log($"[NPCBasicDialog] Cambiando a escena: {mission.sceneToLoad}");
            Time.timeScale = 1f; // Restaurar timeScale por si acaso
            SceneManager.LoadScene(mission.sceneToLoad);
        }
    }

    // Maneja el final del diálogo simple (sin sistema de misiones)
    IEnumerator HandleSimpleDialogEnd()
    {
        // Esperar a que termine el diálogo
        yield return new WaitUntil(() => manager.DialogFinished);

        // Invocar eventos
        try
        {
            onDialogFinished?.Invoke();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error invoking onDialogFinished for {gameObject.name}: {ex}");
        }

        // Si este NPC está configurado para disparar el final tras el diálogo (diálogo simple), hacerlo aquí
        if (triggerFinalAfterDialog && !finalTriggered)
        {
            finalTriggered = true;
            MissionToEndingChooserHelper.DecideAndLoad();
            yield break;
        }
        // Cambiar de escena si está configurado
        if (loadSceneAfterSimpleDialog && !string.IsNullOrEmpty(simpleDialogSceneToLoad))
        {
            yield return new WaitForSeconds(simpleDialogDelayBeforeChange);
            Debug.Log($"[NPCBasicDialog] Cambiando a escena: {simpleDialogSceneToLoad}");
            Time.timeScale = 1f;
            SceneManager.LoadScene(simpleDialogSceneToLoad);
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

    // Debug adicional: mostrar escena y índice para trazar problemas multi-escena
    Debug.Log($"[NPCBasicDialog] OnChoiceSelected - Escena: {SceneManager.GetActiveScene().name}, Índice: {choiceIndex}");
        
        // ⭐ CONTADOR GLOBAL - preferimos la clave explícita en la opción si existe,
        // si no, caemos al comportamiento legacy basado en el índice.
        ChoiceCounterManager.EnsureExists();

        string globalChoiceID = selectedChoice != null && !string.IsNullOrEmpty(selectedChoice.globalChoiceID)
            ? selectedChoice.globalChoiceID
            : null;

        // Legacy fallback: mapear por índice si no hay globalChoiceID
        if (string.IsNullOrEmpty(globalChoiceID))
        {
            string[] optionNames = { "SaberSobreElCirco", "SaberSobreMi" };
            globalChoiceID = choiceIndex < optionNames.Length
                ? optionNames[choiceIndex]
                : $"Opcion_{choiceIndex}";
        }

        if (ChoiceCounterManager.Instance != null)
        {
            ChoiceCounterManager.Instance.IncrementChoice(globalChoiceID);
        }
        else
        {
            Debug.LogWarning("[NPCBasicDialog] ChoiceCounterManager no pudo inicializarse");
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

            // Si esta opción da la misión que activa el final inmediato, esperar a que GameManager la registre y decidir el final
            // Si la opción da una misión que requiere esperar a que GameManager la registre
            // (compatibilidad: tratamos 'Abrir portal secreto' como caso especial) o si el diseñador
            // marcó waitForMissionRegistration en el inspector, esperar y luego decidir el final.
            bool shouldWaitForMission = false;
            if (!string.IsNullOrEmpty(choice.missionToGive))
            {
                if (string.Equals(choice.missionToGive.Trim(), "Abrir portal secreto", System.StringComparison.OrdinalIgnoreCase))
                    shouldWaitForMission = true;
                if (choice.waitForMissionRegistration)
                    shouldWaitForMission = true;
            }

            if (shouldWaitForMission)
            {
                Debug.Log($"[NPCBasicDialog] Opción dio '{choice.missionToGive}' -> Esperando confirmación de GameManager para decidir final");
                StartCoroutine(WaitForMissionAndDecide(choice.missionToGive));
                yield break; // salir del coroutine actual, la decisión se hará desde WaitForMissionAndDecide
            }

            // Si la opción está marcada para disparar final (trigger), decidir ahora
            if (choice.triggersFinal)
            {
                Debug.Log("[NPCBasicDialog] Opción marcada para disparar final -> Decidiendo según contadores");
                MissionToEndingChooserHelper.DecideAndLoad();
                yield break;
            }
        }
        
        // Invocar eventos (por ejemplo: dar un item automáticamente tras la opción)
        try
        {
            onDialogFinished?.Invoke();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error invoking onDialogFinished after choice for {gameObject.name}: {ex}");
        }

        currentMissionWithChoices = null;

        // Cambiar de escena si esta opción lo tiene configurado
        if (choice.loadSceneAfterChoice && !string.IsNullOrEmpty(choice.sceneToLoad))
        {
            yield return new WaitForSeconds(choice.delayBeforeChange);
            Debug.Log($"[NPCBasicDialog] Cambiando a escena: {choice.sceneToLoad}");
            Time.timeScale = 1f; // Restaurar timeScale por si acaso
            SceneManager.LoadScene(choice.sceneToLoad);
        }

        yield break;
    }
}
