using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueChoicesUIFixed : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("Panel de opciones")]
    public GameObject choicesPanel;
    
    [Tooltip("Contenedor de botones")]
    public Transform buttonsContainer;
    
    [Tooltip("Prefab del botón")]
    public GameObject choiceButtonPrefab;
    
    private PintacaritasNPC currentNPC;
    private List<GameObject> spawnedButtons = new List<GameObject>();
    
    void Awake()
    {
        // Validar referencias
        ValidateReferences();
        // Ocultar panel
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
    
    // Mostrar opciones
    public void ShowChoices(PintacaritasNPC npc, List<PintacaritasNPC.DialogueChoice> choices)
    {
        Debug.Log("=== DialogueChoicesUIFixed: ShowChoices() LLAMADO ===");
        
    // Validar datos
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
        
    // Limpiar botones
        ClearButtons();
        
    // Activar panel
        Debug.Log($"🔄 Activando panel: {choicesPanel.name}");
        choicesPanel.SetActive(true);
        Debug.Log($"✅ Panel activado. Estado actual: {choicesPanel.activeSelf}");
        
    // Crear botones
        for (int i = 0; i < choices.Count; i++)
        {
            Debug.Log($"🔄 Creando botón {i + 1}/{choices.Count}: '{choices[i].choiceText}'");
            
            GameObject buttonObj = Instantiate(choiceButtonPrefab, buttonsContainer);
            buttonObj.name = $"ChoiceButton_{i}";
            
            // Texto del botón
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
                }
            }
            
            // Configurar botón
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                int choiceIndex = i;
                button.onClick.RemoveAllListeners(); // Limpiar listeners
                button.onClick.AddListener(() => OnChoiceClicked(choiceIndex));
            }
            else
            {
                Debug.LogError($"  ❌ El prefab no tiene componente Button!");
            }
            
            spawnedButtons.Add(buttonObj);
        }
        
        Debug.Log($"🎉 ShowChoices COMPLETADO: {spawnedButtons.Count} botones creados, panel activo: {choicesPanel.activeSelf}");
    }
    
    // Click en opción
    private void OnChoiceClicked(int choiceIndex)
    {
        Debug.Log($"🖱️ DialogueChoicesUIFixed: Jugador hizo click en opción {choiceIndex}");
        
    // Avisar NPC
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
    
    // Ocultar panel
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
    
    // Limpiar botones
    // Sobrecarga para NPCBasicDialog
    private NPCBasicDialog currentBasicNPC;
    
    public void ShowChoices(NPCBasicDialog npc, List<DialogueChoice> choices)
    {
        Debug.Log("=== DialogueChoicesUIFixed: ShowChoices() para NPCBasicDialog ===");
        
        if (npc == null || choices == null || choices.Count == 0)
        {
            Debug.LogError("❌ DialogueChoicesUIFixed: NPC o choices inválidos!");
            return;
        }
        
        if (choicesPanel == null || buttonsContainer == null || choiceButtonPrefab == null)
        {
            Debug.LogError("❌ DialogueChoicesUIFixed: Referencias UI no asignadas!");
            return;
        }
        
        currentBasicNPC = npc;
        currentNPC = null; // Limpiar referencia de Pintacaritas
        
        // Limpiar botones anteriores
        ClearButtons();
        
        // Activar panel
        choicesPanel.SetActive(true);
        
        // Crear botones
        for (int i = 0; i < choices.Count; i++)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, buttonsContainer);
            buttonObj.name = $"ChoiceButton_{i}";
            
            // Texto del botón
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = choices[i].choiceText;
            }
            else
            {
                Text legacyText = buttonObj.GetComponentInChildren<Text>();
                if (legacyText != null)
                {
                    legacyText.text = choices[i].choiceText;
                }
            }
            
            // Configurar botón
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                int choiceIndex = i;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnBasicNPCChoiceClicked(choiceIndex));
            }
            
            spawnedButtons.Add(buttonObj);
        }
        
        Debug.Log($"✅ {spawnedButtons.Count} botones creados para NPCBasicDialog");
    }
    
    private void OnBasicNPCChoiceClicked(int choiceIndex)
    {
        Debug.Log($"🖱️ NPCBasicDialog: Jugador eligió opción {choiceIndex}");
        
        if (currentBasicNPC != null)
        {
            currentBasicNPC.OnChoiceSelected(choiceIndex);
        }
        
        // Ocultar panel
        HideChoices();
    }
    
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
