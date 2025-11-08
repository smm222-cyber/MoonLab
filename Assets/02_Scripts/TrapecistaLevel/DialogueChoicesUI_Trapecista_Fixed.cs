using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueChoicesUI_Trapecista_Fixed : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("Panel de opciones")]
    public GameObject choicesPanel;

    [Tooltip("Contenedor de botones")]
    public Transform buttonsContainer;

    [Tooltip("Prefab del botón")]
    public GameObject choiceButtonPrefab;

    private TrapecistaNPC currentNPC;
    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Awake()
    {
        // Validar referencias
        ValidateReferences();

        // Ocultar panel
        if (choicesPanel != null)
        {
            choicesPanel.SetActive(false);
            Debug.Log("DialogueChoicesUI_Trapecista_Fixed: Panel inicializado como desactivado");
        }
    }

    void ValidateReferences()
    {
        bool allValid = true;

        if (choicesPanel == null)
        {
            Debug.LogError("❌ DialogueChoicesUI_Trapecista_Fixed: 'choicesPanel' no está asignado en el Inspector!");
            allValid = false;
        }

        if (buttonsContainer == null)
        {
            Debug.LogError("❌ DialogueChoicesUI_Trapecista_Fixed: 'buttonsContainer' no está asignado en el Inspector!");
            allValid = false;
        }

        if (choiceButtonPrefab == null)
        {
            Debug.LogError("❌ DialogueChoicesUI_Trapecista_Fixed: 'choiceButtonPrefab' no está asignado en el Inspector!");
            allValid = false;
        }

        if (allValid)
            Debug.Log("✅ DialogueChoicesUI_Trapecista_Fixed: Todas las referencias están asignadas correctamente");
    }

    // Mostrar opciones de diálogo
    public void ShowChoices(TrapecistaNPC npc, List<TrapecistaNPC.DialogueChoice> choices)
    {
        Debug.Log("=== DialogueChoicesUI_Trapecista_Fixed: ShowChoices() LLAMADO ===");

        // Validar datos
        if (npc == null)
        {
            Debug.LogError("❌ DialogueChoicesUI_Trapecista_Fixed: NPC es null!");
            return;
        }

        if (choices == null || choices.Count == 0)
        {
            Debug.LogError("❌ DialogueChoicesUI_Trapecista_Fixed: Lista de opciones vacía o null!");
            return;
        }

        if (choicesPanel == null || buttonsContainer == null || choiceButtonPrefab == null)
        {
            Debug.LogError("❌ DialogueChoicesUI_Trapecista_Fixed: Alguna referencia UI es null!");
            return;
        }

        Debug.Log($"✅ Validaciones pasadas. Preparando {choices.Count} opciones...");

        currentNPC = npc;

        // Limpiar botones anteriores
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
            TextMeshProUGUI buttonTextTMP = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonTextTMP != null)
                buttonTextTMP.text = choices[i].choiceText;
            else
            {
                Text buttonTextLegacy = buttonObj.GetComponentInChildren<Text>();
                if (buttonTextLegacy != null)
                    buttonTextLegacy.text = choices[i].choiceText;
            }

            // Configurar listener del botón
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                int choiceIndex = i;
                button.onClick.RemoveAllListeners(); // Limpiar listeners previos
                button.onClick.AddListener(() => OnChoiceClicked(choiceIndex));
            }
            else
            {
                Debug.LogError("❌ El prefab no tiene componente Button!");
            }

            spawnedButtons.Add(buttonObj);
        }

        Debug.Log($"🎉 ShowChoices COMPLETADO: {spawnedButtons.Count} botones creados, panel activo: {choicesPanel.activeSelf}");
    }

    // Click en opción de diálogo
    private void OnChoiceClicked(int choiceIndex)
    {
        Debug.Log($"🖱️ DialogueChoicesUI_Trapecista_Fixed: Jugador hizo click en opción {choiceIndex}");

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

    // Ocultar panel y limpiar botones
    public void HideChoices()
    {
        Debug.Log("🔄 DialogueChoicesUI_Trapecista_Fixed: Ocultando panel...");

        if (choicesPanel != null)
        {
            choicesPanel.SetActive(false);
            Debug.Log("✅ Panel desactivado");
        }

        ClearButtons();
        currentNPC = null;
    }

    // Limpiar botones
    private void ClearButtons()
    {
        if (spawnedButtons.Count > 0)
        {
            Debug.Log($"🧹 Limpiando {spawnedButtons.Count} botones...");
        }

        foreach (GameObject button in spawnedButtons)
        {
            if (button != null)
                Destroy(button);
        }

        spawnedButtons.Clear();
    }
}
