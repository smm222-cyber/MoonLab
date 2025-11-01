using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueChoicesUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("Panel de opciones")]
    public GameObject choicesPanel;
    
    [Tooltip("Contenedor de botones")]
    public Transform buttonsContainer;
    
    [Tooltip("Prefab del botón")]
    public GameObject choiceButtonPrefab;
    
    [Header("Configuración")]
    [Tooltip("Espaciado")]
    public float buttonSpacing = 10f;
    
    private PintacaritasNPC currentNPC;
    private List<GameObject> spawnedButtons = new List<GameObject>();
    
    void Start()
    {
        // Ocultar panel
        if (choicesPanel != null)
        {
            choicesPanel.SetActive(false);
        }
    }
    
    // Mostrar opciones
    public void ShowChoices(PintacaritasNPC npc, List<PintacaritasNPC.DialogueChoice> choices)
    {
        if (npc == null || choices == null || choices.Count == 0)
        {
            Debug.LogError("DialogueChoicesUI: NPC o lista de opciones inválidos");
            return;
        }
        
        Debug.Log($"DialogueChoicesUI: ShowChoices llamado con {choices.Count} opciones");
        
        currentNPC = npc;
        
    // Activar panel
        if (choicesPanel != null)
        {
            choicesPanel.SetActive(true);
            Debug.Log("DialogueChoicesUI: Panel activado");
        }
        else
        {
            Debug.LogError("DialogueChoicesUI: choicesPanel es null! Asigna la referencia en el Inspector");
            return;
        }
        
    // Limpiar botones
        ClearButtons();
        
    // Prefab existe?
        if (choiceButtonPrefab == null)
        {
            Debug.LogError("DialogueChoicesUI: choiceButtonPrefab es null! Asigna el prefab en el Inspector");
            return;
        }
        
        if (buttonsContainer == null)
        {
            Debug.LogError("DialogueChoicesUI: buttonsContainer es null! Asigna la referencia en el Inspector");
            return;
        }
        
    // Crear botones
        for (int i = 0; i < choices.Count; i++)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, buttonsContainer);
            Debug.Log($"DialogueChoicesUI: Botón {i} creado: {choices[i].choiceText}");
            
            // Texto del botón
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = choices[i].choiceText;
                Debug.Log($"DialogueChoicesUI: Texto asignado con TextMeshProUGUI");
            }
            else
            {
                // Si no hay TMP, usa Text
                Text legacyText = buttonObj.GetComponentInChildren<Text>();
                if (legacyText != null)
                {
                    legacyText.text = choices[i].choiceText;
                    Debug.Log($"DialogueChoicesUI: Texto asignado con Text (legacy)");
                }
                else
                {
                    Debug.LogWarning($"DialogueChoicesUI: No se encontró componente de texto en el botón {i}");
                }
            }
            
            // Configurar botón
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                int choiceIndex = i; // Índice local
                button.onClick.AddListener(() => OnChoiceClicked(choiceIndex));
                Debug.Log($"DialogueChoicesUI: Listener agregado al botón {i}");
            }
            else
            {
                Debug.LogError($"DialogueChoicesUI: El prefab no tiene componente Button!");
            }
            
            spawnedButtons.Add(buttonObj);
        }
        
    Debug.Log($"DialogueChoicesUI: ✅ {choices.Count} botones creados y panel activado");
    }
    
    // Click en opción
    private void OnChoiceClicked(int choiceIndex)
    {
        Debug.Log($"DialogueChoicesUI: Jugador eligió opción {choiceIndex}");
        
    // Avisar NPC
        if (currentNPC != null)
        {
            currentNPC.OnChoiceSelected(choiceIndex);
        }
        
    // Ocultar panel
        HideChoices();
    }
    
    // Ocultar panel
    public void HideChoices()
    {
        if (choicesPanel != null)
        {
            choicesPanel.SetActive(false);
        }
        
    ClearButtons();
    currentNPC = null;
    }
    
    // Limpiar botones
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
