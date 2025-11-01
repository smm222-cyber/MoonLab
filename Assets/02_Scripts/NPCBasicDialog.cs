using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMission
{
    [TextArea(3, 10)]
    public string dialogueText;
    public string missionToGive;      // Misión que se da en este diálogo
    public string missionRequired;     // Misión que debe completarse para ver este diálogo
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

    [Header("Diálogo Simple (sin misiones)")]
    [TextArea(3, 10)]
    public string dialogueText;

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

        // Si usa el sistema de misiones, usar el índice actual
        if (usesMissionSystem && missions.Count > 0)
        {
            // Asegurarse de no salirse del rango
            if (currentMissionIndex >= missions.Count)
            {
                currentMissionIndex = missions.Count - 1;
            }

            NPCMission currentMission = missions[currentMissionIndex];
            
            // Verificar si tiene la misión anterior activa (no la completó)
            if (currentMissionIndex > 0)
            {
                NPCMission previousMission = missions[currentMissionIndex - 1];
                
                // Si la misión anterior todavía está activa, repetir el diálogo anterior
                if (!string.IsNullOrEmpty(previousMission.missionToGive) && 
                    manager.HasMission(previousMission.missionToGive))
                {
                    textToShow = previousMission.dialogueText;
                    missionToAdd = ""; // No dar misión nueva
                }
                else
                {
                    // La misión anterior se completó, avanzar
                    textToShow = currentMission.dialogueText;
                    missionToAdd = currentMission.missionToGive;
                    currentMissionIndex++; // Solo avanzar si completó la anterior
                }
            }
            else
            {
                // Primera misión
                textToShow = currentMission.dialogueText;
                missionToAdd = currentMission.missionToGive;
                
                // Solo avanzar si no tiene la misión activa (para evitar dar la misión múltiples veces)
                if (string.IsNullOrEmpty(currentMission.missionToGive) || 
                    !manager.HasMission(currentMission.missionToGive))
                {
                    currentMissionIndex++;
                }
            }
        }

        // Dividir el texto en páginas
        List<string> pages = SplitTextIntoPages(textToShow, maxCharactersPerPage);
        manager.NPCShowText(pages, npcName, npcImage, typingSound);

        // Agregar la misión después de mostrar el diálogo
        if (!string.IsNullOrEmpty(missionToAdd))
        {
            StartCoroutine(AddMissionAfterDialog(missionToAdd));
        }
    }

    IEnumerator AddMissionAfterDialog(string mission)
    {
        // Esperar a que termine el diálogo
        yield return new WaitUntil(() => manager.DialogFinished);
        manager.AddMission(mission);
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
}