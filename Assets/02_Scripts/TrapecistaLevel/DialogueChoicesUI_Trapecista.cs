using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueChoicesUI_Trapecista : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("Panel de opciones")]
    public GameObject choicesPanel;

    [Tooltip("Contenedor de botones")]
    public Transform buttonsContainer;

    [Tooltip("Prefab del botón")]
    public GameObject choiceButtonPrefab;

    [Header("Configuración")]
    [Tooltip("Espaciado entre botones")]
    public float buttonSpacing = 10f;

    private TrapecistaNPC currentNPC;
    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Start()
    {
        // Ocultar panel al inicio
        if (choicesPanel != null)
            choicesPanel.SetActive(false);
    }

    // Mostrar opciones de diálogo del Trapecista
    public void ShowChoices(TrapecistaNPC npc, List<TrapecistaNPC.DialogueChoice> choices)
    {
        if (npc == null || choices == null || choices.Count == 0)
        {
            Debug.LogError("DialogueChoicesUI_Trapecista: NPC o lista de opciones inválidos");
            return;
        }

        currentNPC = npc;

        // Activar panel
        if (choicesPanel != null)
            choicesPanel.SetActive(true);
        else
        {
            Debug.LogError("DialogueChoicesUI_Trapecista: choicesPanel es null! Asigna la referencia en el Inspector");
            return;
        }

        // Limpiar botones anteriores
        ClearButtons();

        // Verificar prefab y contenedor
        if (choiceButtonPrefab == null)
        {
            Debug.LogError("DialogueChoicesUI_Trapecista: choiceButtonPrefab es null! Asigna el prefab en el Inspector");
            return;
        }

        if (buttonsContainer == null)
        {
            Debug.LogError("DialogueChoicesUI_Trapecista: buttonsContainer es null! Asigna la referencia en el Inspector");
            return;
        }

        // Crear botones según la lista de opciones
        for (int i = 0; i < choices.Count; i++)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, buttonsContainer);

            // Asignar texto al botón
            TextMeshProUGUI buttonTextTMP = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonTextTMP != null)
                buttonTextTMP.text = choices[i].choiceText;
            else
            {
                Text buttonTextLegacy = buttonObj.GetComponentInChildren<Text>();
                if (buttonTextLegacy != null)
                    buttonTextLegacy.text = choices[i].choiceText;
                else
                    Debug.LogWarning($"DialogueChoicesUI_Trapecista: No se encontró componente de texto en el botón {i}");
            }

            // Configurar listener del botón
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                int choiceIndex = i; // Captura local para el listener
                button.onClick.AddListener(() => OnChoiceClicked(choiceIndex));
            }
            else
            {
                Debug.LogError("DialogueChoicesUI_Trapecista: El prefab no tiene componente Button!");
            }

            spawnedButtons.Add(buttonObj);
        }
    }

    // Click en opción de diálogo
    private void OnChoiceClicked(int choiceIndex)
    {
        if (currentNPC != null)
            currentNPC.OnChoiceSelected(choiceIndex);

        HideChoices();
    }

    // Ocultar panel y limpiar botones
    public void HideChoices()
    {
        if (choicesPanel != null)
            choicesPanel.SetActive(false);

        ClearButtons();
        currentNPC = null;
    }

    // Destruir botones antiguos
    private void ClearButtons()
    {
        foreach (GameObject button in spawnedButtons)
        {
            if (button != null)
                Destroy(button);
        }
        spawnedButtons.Clear();
    }
}
