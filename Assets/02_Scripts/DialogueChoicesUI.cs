using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// EJEMPLO DE SISTEMA DE UI PARA OPCIONES DE DIÁLOGO
/// 
/// Este script es un EJEMPLO de cómo implementar una UI para mostrar opciones de diálogo.
/// Puedes adaptarlo a tu sistema de UI existente.
/// 
/// CÓMO USAR:
/// 1. Crea un Canvas con un Panel para las opciones
/// 2. Crea un Prefab de botón (con TextMeshProUGUI)
/// 3. Asigna este script al Panel
/// 4. Asigna las referencias en el Inspector
/// 5. Llama a ShowChoices() desde PintacaritasNPC.cs
/// </summary>
public class DialogueChoicesUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("Panel que contiene todos los botones de opciones")]
    public GameObject choicesPanel;
    
    [Tooltip("Contenedor donde se instanciarán los botones (ej: Vertical Layout Group)")]
    public Transform buttonsContainer;
    
    [Tooltip("Prefab del botón de opción (debe tener TextMeshProUGUI)")]
    public GameObject choiceButtonPrefab;
    
    [Header("Configuración")]
    [Tooltip("Espaciado entre botones")]
    public float buttonSpacing = 10f;
    
    private PintacaritasNPC currentNPC;
    private List<GameObject> spawnedButtons = new List<GameObject>();
    
    void Start()
    {
        // Asegurarse de que el panel esté oculto al inicio
        if (choicesPanel != null)
        {
            choicesPanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// Muestra las opciones de diálogo del NPC especificado
    /// </summary>
    public void ShowChoices(PintacaritasNPC npc, List<PintacaritasNPC.DialogueChoice> choices)
    {
        if (npc == null || choices == null || choices.Count == 0)
        {
            Debug.LogError("DialogueChoicesUI: NPC o lista de opciones inválidos");
            return;
        }
        
        currentNPC = npc;
        
        // Limpiar botones anteriores si existen
        ClearButtons();
        
        // Crear un botón por cada opción
        for (int i = 0; i < choices.Count; i++)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, buttonsContainer);
            
            // Obtener el componente de texto (TextMeshProUGUI o Text)
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = choices[i].choiceText;
            }
            else
            {
                // Fallback a Text si no usa TextMeshPro
                Text legacyText = buttonObj.GetComponentInChildren<Text>();
                if (legacyText != null)
                {
                    legacyText.text = choices[i].choiceText;
                }
            }
            
            // Configurar el botón
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                int choiceIndex = i; // Capturar el índice en una variable local
                button.onClick.AddListener(() => OnChoiceClicked(choiceIndex));
            }
            
            spawnedButtons.Add(buttonObj);
        }
        
        // Mostrar el panel
        choicesPanel.SetActive(true);
        
        Debug.Log($"DialogueChoicesUI: Mostrando {choices.Count} opciones");
    }
    
    /// <summary>
    /// Se llama cuando el jugador hace click en una opción
    /// </summary>
    private void OnChoiceClicked(int choiceIndex)
    {
        Debug.Log($"DialogueChoicesUI: Jugador eligió opción {choiceIndex}");
        
        // Notificar al NPC de la elección
        if (currentNPC != null)
        {
            currentNPC.OnChoiceSelected(choiceIndex);
        }
        
        // Ocultar el panel de opciones
        HideChoices();
    }
    
    /// <summary>
    /// Oculta el panel de opciones
    /// </summary>
    public void HideChoices()
    {
        if (choicesPanel != null)
        {
            choicesPanel.SetActive(false);
        }
        
        ClearButtons();
        currentNPC = null;
    }
    
    /// <summary>
    /// Limpia todos los botones creados
    /// </summary>
    private void ClearButtons()
    {
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
