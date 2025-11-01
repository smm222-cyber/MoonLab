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

    [Header("Configuración de Diálogos")]
    [Tooltip("Diálogo que se muestra si aún no has hablado con la Pintacaritas 1")]
    [TextArea(3, 10)]
    public string dialogoSinHablarConPintacaritas1;

    [Tooltip("Diálogo que se muestra después de hablar con la Pintacaritas 1")]
    [TextArea(3, 10)]
    public string dialogoDespuesDeHablarConPintacaritas1;

    [Tooltip("Diálogo adicional (si es necesario después de otra condición)")]
    [TextArea(3, 10)]
    public string dialogoFinal;

    [Header("Sistema de Progreso")]
    [Tooltip("Nombre de la misión que indica que ya hablaste con Pintacaritas 1")]
    public string misionPintacaritas1 = "HablasteCon_Pintacaritas1";
    
    [Tooltip("Nombre de la misión que indica que ya hablaste con la Trapecista")]
    public string misionTrapecista = "HablasteCon_Trapecista";

    [Header("Audio")]
    public AudioClip typingSound;

    [Header("Configuración Avanzada")]
    public int maxCharactersPerPage = 40;
    
    private GameManager manager;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public void Interact()
    {
        string dialogoAMostrar = DeterminarDialogo();
        
        // Dividir el texto en páginas si es necesario
        List<string> pages = SplitTextIntoPages(dialogoAMostrar, maxCharactersPerPage);
        
        // Mostrar el diálogo
        manager.NPCShowText(pages, npcName, npcImage, typingSound);
    }

    private string DeterminarDialogo()
    {
        // Verificar si ya completó la misión de hablar con Pintacaritas 1
        bool habloCon1 = !manager.HasMission(misionPintacaritas1);
        bool habloConTrapecista = !manager.HasMission(misionTrapecista);

        // Lógica de decisión de diálogo
        if (!string.IsNullOrEmpty(dialogoFinal) && habloCon1 && habloConTrapecista)
        {
            return dialogoFinal;
        }
        else if (habloCon1 && !string.IsNullOrEmpty(dialogoDespuesDeHablarConPintacaritas1))
        {
            return dialogoDespuesDeHablarConPintacaritas1;
        }
        else
        {
            return dialogoSinHablarConPintacaritas1;
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
