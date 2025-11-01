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
    public string missionToComplete;   // Misión que se completa con este diálogo
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
        string missionToCompleteNow = "";

        // Si usa el sistema de misiones, determinar qué misión mostrar
        if (usesMissionSystem && missions.Count > 0)
        {
            NPCMission missionToUse = null;
            
            // Buscar la primera misión que cumpla con los requisitos
            for (int i = 0; i < missions.Count; i++)
            {
                NPCMission mission = missions[i];
                
                // Si la misión requiere otra misión, verificar si está activa
                if (!string.IsNullOrEmpty(mission.missionRequired))
                {
                    if (manager.HasMission(mission.missionRequired))
                    {
                        // Tiene la misión requerida, usar esta misión
                        missionToUse = mission;
                        break;
                    }
                }
                else
                {
                    // No tiene requisitos, usar esta misión
                    missionToUse = mission;
                    break;
                }
            }
            
            // Si encontramos una misión válida, usarla
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

        // Completar y agregar misiones después de mostrar el diálogo
        if (!string.IsNullOrEmpty(missionToCompleteNow) || !string.IsNullOrEmpty(missionToAdd))
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
}