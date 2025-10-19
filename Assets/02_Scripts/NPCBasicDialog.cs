using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBasicDialog : MonoBehaviour, IInteractable
{
    public string npcName;
    public Sprite npcImage;
    GameManager manager;
    public GameObject interactUI;

    [TextArea(3, 10)]
    public string dialogueText;

    [Header("Configuración")]
    [Tooltip("Máximo de caracteres por página")]
    public int maxCharactersPerPage = 40;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public void Interact()
    {
        // Dividir el texto en páginas
        List<string> pages = SplitTextIntoPages(dialogueText, maxCharactersPerPage);
        manager.NPCShowText(pages, npcName, npcImage);
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