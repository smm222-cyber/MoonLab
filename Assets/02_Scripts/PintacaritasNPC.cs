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
        // Intentar obtener el GameManager
        GetManagerReference();
    }

    private void GetManagerReference()
    {
        if (manager == null)
        {
            manager = GameManager.Instance;
            
            if (manager == null)
            {
                manager = FindObjectOfType<GameManager>();
            }
            
            if (manager == null)
            {
                Debug.LogError($"GameManager no encontrado para {gameObject.name}!");
            }
        }
    }

    public void Interact()
    {
        // Intentar obtener el manager si aún es null
        if (manager == null)
        {
            GetManagerReference();
        }
        
        if (manager == null)
        {
            Debug.LogError($"No se puede interactuar con {gameObject.name}: GameManager es null");
            return;
        }
        
        string dialogoAMostrar = DeterminarDialogo();
        
        // Validar que el diálogo no esté vacío
        if (string.IsNullOrEmpty(dialogoAMostrar))
        {
            Debug.LogWarning("El diálogo está vacío. Asegúrate de configurar los textos en el Inspector.");
            return;
        }
        
        // Dividir el texto en páginas si es necesario
        List<string> pages = SplitTextIntoPages(dialogoAMostrar, maxCharactersPerPage);
        
        // Validar que la lista no esté vacía
        if (pages == null || pages.Count == 0)
        {
            Debug.LogWarning("No se pudieron crear páginas de diálogo.");
            return;
        }
        
        // Mostrar el diálogo
        manager.NPCShowText(pages, npcName, npcImage, typingSound);
    }

    private string DeterminarDialogo()
    {
        if (manager == null)
        {
            Debug.LogWarning($"{gameObject.name}: Manager es null en DeterminarDialogo, usando diálogo por defecto.");
            return dialogoSinHablarConPintacaritas1;
        }
        
        // Verificar si tiene las misiones activas
        bool tieneMisionTrapecista = manager.HasMission(misionTrapecista);

        // Lógica SIMPLE:
        // - Si tiene misión de Trapecista = Diálogo largo
        // - Si NO tiene misión de Trapecista = Diálogo corto
        
        if (tieneMisionTrapecista && !string.IsNullOrEmpty(dialogoDespuesDeHablarConPintacaritas1))
        {
            // Tiene la misión de buscar a la otra pintacaritas → Diálogo largo
            return dialogoDespuesDeHablarConPintacaritas1;
        }
        else
        {
            // NO tiene la misión de trapecista → Diálogo corto
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

        // Validar entrada
        if (string.IsNullOrEmpty(text))
        {
            Debug.LogWarning("Texto vacío en SplitTextIntoPages");
            return pages;
        }

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
