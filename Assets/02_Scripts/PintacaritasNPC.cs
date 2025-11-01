using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script específico para las Pintacaritas que cambia su diálogo basado en el progreso del jugador.
/// </summary>
public class PintacaritasNPC : MonoBehaviour, IInteractable
{
    [Header("Información del NPC")]
    public string npcName = "Pintacaritas";
    public Sprite npcImage;
    public GameObject interactUI;

    [Header("FASE 1: Diálogo Inicial")]
    [Tooltip("Diálogo corto inicial - cuando NO tiene la misión requerida")]
    [TextArea(3, 10)]
    public string dialogoSinHablarConPintacaritas1;

    [Header("FASE 2: Primer Diálogo Largo")]
    [Tooltip("Misión requerida para la FASE 2")]
    public string misionRequeridaFase2 = "Escuchar la historia completa";
    
    [Tooltip("Diálogo largo FASE 2 - cuando tiene la misión requerida")]
    [TextArea(3, 10)]
    public string dialogoFase2;
    
    [Tooltip("Misión que se completa en FASE 2")]
    public string misionACompletarFase2 = "Escuchar la historia completa";
    
    [Tooltip("Misión que se da en FASE 2 (buscar primer objeto)")]
    public string misionADarFase2 = "Buscar el pincel de la pintacaritas";

    [Header("FASE 3: Después del Primer Objeto")]
    [Tooltip("Misión requerida para la FASE 3 (cuando ya NO tiene la misión del primer objeto = ya lo recogió)")]
    public string misionRequeridaFase3 = "Buscar el pincel de la pintacaritas";
    
    [Tooltip("Diálogo FASE 3 - cuando ya recogió el primer objeto")]
    [TextArea(3, 10)]
    public string dialogoFase3;
    
    [Tooltip("Misión que se da en FASE 3 (buscar segundo objeto)")]
    public string misionADarFase3 = "Buscar las pinturas de la pintacaritas";

    [Header("FASE 4: Después del Segundo Objeto")]
    [Tooltip("Misión requerida para la FASE 4 (cuando ya NO tiene la misión del segundo objeto = ya lo recogió)")]
    public string misionRequeridaFase4 = "Buscar las pinturas de la pintacaritas";
    
    [Tooltip("Diálogo FASE 4 - cuando ya recogió el segundo objeto")]
    [TextArea(3, 10)]
    public string dialogoFase4;
    
    [Tooltip("Misión que se da en FASE 4 (hablar con Pintacaritas1)")]
    public string misionADarFase4 = "Hablar con Héctor el Pintacaritas";

    [Header("FASE 5: Diálogo con Opciones")]
    [Tooltip("Misión requerida para la FASE 5 (volver después de hablar con Héctor)")]
    public string misionRequeridaFase5 = "Volver con Pintacaritas2";
    
    [Tooltip("Diálogo FASE 5 - Antes de mostrar opciones")]
    [TextArea(3, 10)]
    public string dialogoFase5Inicial;
    
    [System.Serializable]
    public class DialogueChoice
    {
        [Tooltip("Texto de la opción que verá el jugador")]
        public string choiceText;
        
        [Tooltip("Diálogo que dice el NPC después de elegir esta opción")]
        [TextArea(3, 10)]
        public string responseDialogue;
    }
    
    [Tooltip("Opciones de diálogo para la FASE 5")]
    public List<DialogueChoice> opcionesFase5 = new List<DialogueChoice>();
    
    [Tooltip("Misión que se completa al iniciar la FASE 5")]
    public string misionACompletarFase5 = "Volver con Pintacaritas2";

    [Header("FASE 6: Diálogo Final")]
    [Tooltip("Diálogo final después de elegir una opción")]
    [TextArea(3, 10)]
    public string dialogoFinal;

    [Header("Audio")]
    public AudioClip typingSound;

    [Header("Configuración Avanzada")]
    public int maxCharactersPerPage = 40;
    
    [Header("Sistema de UI (Opcional)")]
    [Tooltip("Referencia al sistema de UI de opciones. Si no está asignado, usará el sistema por defecto de testing")]
    public DialogueChoicesUI choicesUISystem;
    
    [Tooltip("Versión alternativa del sistema de UI (más robusta). Usa esta si la otra no funciona")]
    public DialogueChoicesUIFixed choicesUISystemFixed;
    
    private GameManager manager;
    private bool misionFase2Dada = false;
    private bool misionFase3Dada = false;
    private bool misionFase4Dada = false;
    private bool opcionesFase5Mostradas = false;
    private int opcionSeleccionada = -1;

    void Start()
    {
        // Intentar obtener el GameManager
        GetManagerReference();
    }

    private void GetManagerReference()
    {
        if (manager == null)
        {
            manager = GameManager.Instance;
            
            if (manager == null)
            {
                manager = FindObjectOfType<GameManager>();
            }
            
            if (manager == null)
            {
                Debug.LogError($"GameManager no encontrado para {gameObject.name}!");
            }
        }
    }

    public void Interact()
    {
        // Intentar obtener el manager si aún es null
        if (manager == null)
        {
            GetManagerReference();
        }
        
        if (manager == null)
        {
            Debug.LogError($"No se puede interactuar con {gameObject.name}: GameManager es null");
            return;
        }
        
        string dialogoAMostrar = DeterminarDialogo();
        
        // Validar que el diálogo no esté vacío
        if (string.IsNullOrEmpty(dialogoAMostrar))
        {
            Debug.LogWarning("El diálogo está vacío. Asegúrate de configurar los textos en el Inspector.");
            return;
        }
        
        // Dividir el texto en páginas si es necesario
        List<string> pages = SplitTextIntoPages(dialogoAMostrar, maxCharactersPerPage);
        
        // Validar que la lista no esté vacía
        if (pages == null || pages.Count == 0)
        {
            Debug.LogWarning("No se pudieron crear páginas de diálogo.");
            return;
        }
        
        // Mostrar el diálogo
        manager.NPCShowText(pages, npcName, npcImage, typingSound);
        
        // Manejar misiones después del diálogo
        StartCoroutine(HandleMissionsDespuesDelDialogo());
    }
    
    IEnumerator HandleMissionsDespuesDelDialogo()
    {
        // Esperar a que termine el diálogo
        yield return new WaitUntil(() => manager.DialogFinished);
        
        // Determinar en qué fase estamos y actuar en consecuencia
        
        // FASE 5: Mostrar opciones de diálogo
        if (!string.IsNullOrEmpty(misionRequeridaFase5) && manager.HasMission(misionRequeridaFase5) && !opcionesFase5Mostradas)
        {
            if (!string.IsNullOrEmpty(misionACompletarFase5))
            {
                manager.CompleteMission(misionACompletarFase5);
                Debug.Log($"[{gameObject.name}] FASE 5 - Misión completada: {misionACompletarFase5}");
            }
            
            opcionesFase5Mostradas = true;
            yield return StartCoroutine(MostrarOpcionesDeDialogo());
            yield break; // Terminar aquí, las opciones manejarán el resto
        }
        
        // FASE 2: Completar misión inicial y dar primera misión de objeto
        if (!string.IsNullOrEmpty(misionRequeridaFase2) && manager.HasMission(misionRequeridaFase2) && !misionFase2Dada)
        {
            if (!string.IsNullOrEmpty(misionACompletarFase2))
            {
                manager.CompleteMission(misionACompletarFase2);
                Debug.Log($"[{gameObject.name}] FASE 2 - Misión completada: {misionACompletarFase2}");
            }
            
            if (!string.IsNullOrEmpty(misionADarFase2))
            {
                manager.AddMission(misionADarFase2);
                misionFase2Dada = true;
                Debug.Log($"[{gameObject.name}] FASE 2 - Misión dada: {misionADarFase2}");
            }
        }
        // FASE 3: El jugador ya no tiene la misión del primer objeto (ya lo recogió)
        else if (!string.IsNullOrEmpty(misionRequeridaFase3) && !manager.HasMission(misionRequeridaFase3) && misionFase2Dada && !misionFase3Dada)
        {
            if (!string.IsNullOrEmpty(misionADarFase3))
            {
                manager.AddMission(misionADarFase3);
                misionFase3Dada = true;
                Debug.Log($"[{gameObject.name}] FASE 3 - Misión dada: {misionADarFase3}");
            }
        }
        // FASE 4: El jugador ya no tiene la misión del segundo objeto (ya lo recogió)
        else if (!string.IsNullOrEmpty(misionRequeridaFase4) && !manager.HasMission(misionRequeridaFase4) && misionFase3Dada && !misionFase4Dada)
        {
            if (!string.IsNullOrEmpty(misionADarFase4))
            {
                manager.AddMission(misionADarFase4);
                misionFase4Dada = true;
                Debug.Log($"[{gameObject.name}] FASE 4 - Misión dada: {misionADarFase4}");
            }
        }
    }

    private string DeterminarDialogo()
    {
        if (manager == null)
        {
            Debug.LogWarning($"{gameObject.name}: Manager es null en DeterminarDialogo, usando diálogo por defecto.");
            return dialogoSinHablarConPintacaritas1;
        }
        
        // FASE 6: Ya eligió una opción - Diálogo final
        if (opcionSeleccionada >= 0 && !string.IsNullOrEmpty(dialogoFinal))
        {
            Debug.Log($"[{gameObject.name}] FASE 6 - Diálogo final después de elegir opción");
            return dialogoFinal;
        }
        
        // FASE 5: Volvió después de hablar con Héctor - Mostrar opciones
        if (!string.IsNullOrEmpty(misionRequeridaFase5) && manager.HasMission(misionRequeridaFase5) && 
            !opcionesFase5Mostradas && !string.IsNullOrEmpty(dialogoFase5Inicial))
        {
            Debug.Log($"[{gameObject.name}] FASE 5 - Preparando opciones de diálogo");
            return dialogoFase5Inicial;
        }
        
        // FASE 4: Ya recogió el segundo objeto (no tiene la misión) - Pedir hablar con Pintacaritas1
        if (!string.IsNullOrEmpty(misionRequeridaFase4) && !manager.HasMission(misionRequeridaFase4) && 
            misionFase3Dada && !misionFase4Dada && !string.IsNullOrEmpty(dialogoFase4))
        {
            Debug.Log($"[{gameObject.name}] FASE 4 - Después del segundo objeto");
            return dialogoFase4;
        }
        
        // FASE 3: Ya recogió el primer objeto (no tiene la misión) - Pedir segundo objeto
        if (!string.IsNullOrEmpty(misionRequeridaFase3) && !manager.HasMission(misionRequeridaFase3) && 
            misionFase2Dada && !misionFase3Dada && !string.IsNullOrEmpty(dialogoFase3))
        {
            Debug.Log($"[{gameObject.name}] FASE 3 - Después del primer objeto");
            return dialogoFase3;
        }
        
        // FASE 2: Tiene la misión inicial - Completarla y dar primera misión de objeto
        if (!string.IsNullOrEmpty(misionRequeridaFase2) && manager.HasMission(misionRequeridaFase2) && 
            !misionFase2Dada && !string.IsNullOrEmpty(dialogoFase2))
        {
            Debug.Log($"[{gameObject.name}] FASE 2 - Diálogo largo inicial");
            return dialogoFase2;
        }
        
        // FASE 1: Diálogo inicial por defecto
        Debug.Log($"[{gameObject.name}] FASE 1 - Diálogo corto inicial");
        return dialogoSinHablarConPintacaritas1;
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

        // Validar entrada
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

    // =======================
    // SISTEMA DE OPCIONES
    // =======================
    
    IEnumerator MostrarOpcionesDeDialogo()
    {
        Debug.Log($"[{gameObject.name}] Mostrando opciones de diálogo");
        
        if (opcionesFase5 == null || opcionesFase5.Count == 0)
        {
            Debug.LogError($"[{gameObject.name}] No hay opciones configuradas en opcionesFase5");
            yield break;
        }
        
        // Verificar si hay un sistema de UI asignado (versión Fixed tiene prioridad)
        if (choicesUISystemFixed != null)
        {
            // MODO PRODUCCIÓN: Usar la versión Fixed (más robusta)
            Debug.Log($"[{gameObject.name}] Usando DialogueChoicesUIFixed para mostrar {opcionesFase5.Count} opciones");
            choicesUISystemFixed.ShowChoices(this, opcionesFase5);
            
            // El sistema de UI llamará a OnChoiceSelected() cuando el jugador elija
            // No necesitamos hacer nada más aquí
        }
        else if (choicesUISystem != null)
        {
            // MODO PRODUCCIÓN: Usar el sistema de UI real (versión original)
            Debug.Log($"[{gameObject.name}] Usando DialogueChoicesUI para mostrar {opcionesFase5.Count} opciones");
            choicesUISystem.ShowChoices(this, opcionesFase5);
            
            // El sistema de UI llamará a OnChoiceSelected() cuando el jugador elija
            // No necesitamos hacer nada más aquí
        }
        else
        {
            // MODO TESTING: Simulación automática sin UI
            Debug.LogWarning($"[{gameObject.name}] ⚠️ SISTEMA DE OPCIONES NO IMPLEMENTADO - Usando opción por defecto para testing");
            Debug.Log($"[{gameObject.name}] Opciones disponibles:");
            for (int i = 0; i < opcionesFase5.Count; i++)
            {
                Debug.Log($"  {i + 1}. {opcionesFase5[i].choiceText}");
            }
            
            Debug.Log($"[{gameObject.name}] Simulando elección automática en 2 segundos...");
            yield return new WaitForSeconds(2f);
            OnChoiceSelected(0); // Opción por defecto para testing
        }
    }
    
    // Este método debe ser llamado por tu sistema de UI cuando el jugador elija una opción
    public void OnChoiceSelected(int choiceIndex)
    {
        if (opcionesFase5 == null || choiceIndex < 0 || choiceIndex >= opcionesFase5.Count)
        {
            Debug.LogError($"[{gameObject.name}] Índice de opción inválido: {choiceIndex}");
            return;
        }
        
        opcionSeleccionada = choiceIndex;
        string responseDialogue = opcionesFase5[choiceIndex].responseDialogue;
        
        Debug.Log($"[{gameObject.name}] Jugador eligió opción {choiceIndex + 1}: {opcionesFase5[choiceIndex].choiceText}");
        
        // Mostrar la respuesta del NPC
        if (!string.IsNullOrEmpty(responseDialogue))
        {
            List<string> pages = SplitTextIntoPages(responseDialogue, maxCharactersPerPage);
            manager.NPCShowText(pages, npcName, npcImage, typingSound);
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
