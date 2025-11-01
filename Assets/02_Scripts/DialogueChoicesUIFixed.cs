using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// VERSIÓN SIMPLIFICADA Y MÁS ROBUSTA DEL SISTEMA DE UI PARA OPCIONES
/// 
/// Esta versión tiene más validaciones y debugging para asegurarse de que funciona.
/// 
/// IMPORTANTE: Este script debe estar en un GameObject que SIEMPRE esté activo,
/// NO en el panel que se activa/desactiva. Por ejemplo:
/// - Canvas (SIEMPRE ACTIVO) ← Script aquí
///   - DialogueChoicesPanel (se activa/desactiva)
///     - ButtonsContainer
/// </summary>
public class DialogueChoicesUIFixed : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("Panel que se activará/desactivará (puede estar desactivado al inicio)")]
    public GameObject choicesPanel;
    
    [Tooltip("Contenedor donde se instanciarán los botones (hijo del panel)")]
    public Transform buttonsContainer;
    
    [Tooltip("Prefab del botón de opción (debe tener Button y TextMeshProUGUI)")]
    public GameObject choiceButtonPrefab;
    
    private PintacaritasNPC currentNPC;
    private List<GameObject> spawnedButtons = new List<GameObject>();
    
    void Awake()
    {
        // Validar referencias en Awake (antes que Start)
        ValidateReferences();
        
        // Asegurarse de que el panel esté oculto al inicio
        if (choicesPanel != null)
        {
            choicesPanel.SetActive(false);
            Debug.Log("DialogueChoicesUIFixed: Panel inicializado como desactivado");
        }
    }
    
    void ValidateReferences()
    {
        bool allValid = true;
        
        if (choicesPanel == null)
        {
            Debug.LogError("❌ DialogueChoicesUIFixed: 'choicesPanel' no está asignado en el Inspector!");
            allValid = false;
        }
        
        if (buttonsContainer == null)
        {
            Debug.LogError("❌ DialogueChoicesUIFixed: 'buttonsContainer' no está asignado en el Inspector!");
            allValid = false;
        }
        
        if (choiceButtonPrefab == null)
        {
            Debug.LogError("❌ DialogueChoicesUIFixed: 'choiceButtonPrefab' no está asignado en el Inspector!");
            allValid = false;
        }
        
        if (allValid)
        {
            Debug.Log("✅ DialogueChoicesUIFixed: Todas las referencias están asignadas correctamente");
        }
    }
    
    /// <summary>
    /// Muestra las opciones de diálogo del NPC especificado
    /// </summary>
    public void ShowChoices(PintacaritasNPC npc, List<PintacaritasNPC.DialogueChoice> choices)
    {
        Debug.Log("=== DialogueChoicesUIFixed: ShowChoices() LLAMADO ===");
        
        // Validaciones
        if (npc == null)
        {
            Debug.LogError("❌ DialogueChoicesUIFixed: NPC es null!");
            return;
        }
        
        if (choices == null || choices.Count == 0)
        {
            Debug.LogError("❌ DialogueChoicesUIFixed: Lista de opciones vacía o null!");
            return;
        }
        
        if (choicesPanel == null)
        {
            Debug.LogError("❌ DialogueChoicesUIFixed: choicesPanel es null! Asigna la referencia en el Inspector");
            return;
        }
        
        if (buttonsContainer == null)
        {
            Debug.LogError("❌ DialogueChoicesUIFixed: buttonsContainer es null! Asigna la referencia en el Inspector");
            return;
        }
        
        if (choiceButtonPrefab == null)
        {
            Debug.LogError("❌ DialogueChoicesUIFixed: choiceButtonPrefab es null! Asigna el prefab en el Inspector");
            return;
        }
        
        Debug.Log($"✅ Todas las validaciones pasadas. Preparando {choices.Count} opciones...");
        
        currentNPC = npc;
        
        // Limpiar botones anteriores
        ClearButtons();
        
        // ACTIVAR EL PANEL
        Debug.Log($"🔄 Activando panel: {choicesPanel.name}");
        choicesPanel.SetActive(true);
        Debug.Log($"✅ Panel activado. Estado actual: {choicesPanel.activeSelf}");
        
        // Crear botones
        for (int i = 0; i < choices.Count; i++)
        {
            Debug.Log($"🔄 Creando botón {i + 1}/{choices.Count}: '{choices[i].choiceText}'");
            
            GameObject buttonObj = Instantiate(choiceButtonPrefab, buttonsContainer);
            buttonObj.name = $"ChoiceButton_{i}";
            
            // Asignar texto
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = choices[i].choiceText;
                Debug.Log($"  ✅ Texto asignado: '{choices[i].choiceText}'");
            }
            else
            {
                Text legacyText = buttonObj.GetComponentInChildren<Text>();
                if (legacyText != null)
                {
                    legacyText.text = choices[i].choiceText;
                    Debug.Log($"  ✅ Texto asignado (legacy): '{choices[i].choiceText}'");
                }
                else
                {
                    Debug.LogError($"  ❌ No se encontró TextMeshProUGUI ni Text en el prefab!");
                }
            }
            
            // Configurar botón
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                int choiceIndex = i;
                button.onClick.RemoveAllListeners(); // Limpiar listeners anteriores
                button.onClick.AddListener(() => OnChoiceClicked(choiceIndex));
                Debug.Log($"  ✅ Listener configurado para índice {choiceIndex}");
            }
            else
            {
                Debug.LogError($"  ❌ El prefab no tiene componente Button!");
            }
            
            spawnedButtons.Add(buttonObj);
        }
        
        Debug.Log($"🎉 ShowChoices COMPLETADO: {spawnedButtons.Count} botones creados, panel activo: {choicesPanel.activeSelf}");
    }
    
    /// <summary>
    /// Se llama cuando el jugador hace click en una opción
    /// </summary>
    private void OnChoiceClicked(int choiceIndex)
    {
        Debug.Log($"🖱️ DialogueChoicesUIFixed: Jugador hizo click en opción {choiceIndex}");
        
        // Notificar al NPC
        if (currentNPC != null)
        {
            currentNPC.OnChoiceSelected(choiceIndex);
            Debug.Log($"✅ Opción {choiceIndex} notificada al NPC");
        }
        else
        {
            Debug.LogError("❌ currentNPC es null, no se puede notificar la elección");
        }
        
        // Ocultar panel
        HideChoices();
    }
    
    /// <summary>
    /// Oculta el panel de opciones
    /// </summary>
    public void HideChoices()
    {
        Debug.Log("🔄 DialogueChoicesUIFixed: Ocultando panel...");
        
        if (choicesPanel != null)
        {
            choicesPanel.SetActive(false);
            Debug.Log("✅ Panel desactivado");
        }
        
        ClearButtons();
        currentNPC = null;
    }
    
    /// <summary>
    /// Limpia todos los botones creados
    /// </summary>
    private void ClearButtons()
    {
        if (spawnedButtons.Count > 0)
        {
            Debug.Log($"🧹 Limpiando {spawnedButtons.Count} botones...");
        }
        
        foreach (GameObject button in spawnedButtons)
        {
            if (button != null)
            {
                Destroy(button);
            }
        }
        
        spawnedButtons.Clear();
    }
}
