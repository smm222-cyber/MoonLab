using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script específico para la Trapecista en el nivel Trapecista, maneja diálogos y misiones por fases similar a PintacaritasNPC
/// Clase renombrada a TrapecistaLevelNPC para evitar colisiones con otras implementaciones (NivelCostura).
/// </summary>
public class TrapecistaLevelNPC : MonoBehaviour
{
    [Header("Configuración NPC")]
    public string npcName = "Trapecista";
    public Sprite npcImage;
    public GameObject interactUI;
    public AudioClip typingSound;
    public int maxCharactersPerPage = 40;

    [Header("Diálogos por Fase")]
    [TextArea(3, 10)]
    public string dialogoSinMisiones = "¡Hola! Ahora estoy ocupada practicando, vuelve más tarde.";

    [Header("FASE 1: Primera Interacción")]
    [TextArea(3, 10)]
    public string dialogoFase1 = "¡Hola! ¿Podrías ayudarme? Necesito reparar mi traje pero me faltan algunas cosas. ¿Podrías traerme un hilo?";
    public string misionADarFase1 = "BuscarHilo";

    [Header("FASE 2: Después de encontrar el Hilo")]
    [TextArea(3, 10)]
    public string dialogoFase2 = "¡Gracias por traer el hilo! Ahora necesito una aguja. ¿Podrías buscarla por mí?";
    public string misionRequeridaFase2 = "BuscarHilo";
    public string misionADarFase2 = "BuscarAguja";
    
    [Header("FASE 3: Después de encontrar la Aguja")]
    [TextArea(3, 10)]
    public string dialogoFase3 = "¡Perfecto! Ya tengo todo lo que necesito. ¿Me ayudas a coser el traje?";
    public string misionRequeridaFase3 = "BuscarAguja";
    public string minijuegoEscena = "SewingScene";
    public float delayAntesCambioEscena = 2.0f;

    [Header("Diálogo Final")]
    [TextArea(3, 10)]
    public string dialogoFinal = "¡El traje quedó perfecto! Gracias por tu ayuda.";

    private bool jugadorEnRango = false;
    private bool todasMisionesCompletadas = false;
    private GameManager manager;

    void Start()
    {
        manager = GameManager.Instance;
        if (manager == null)
            Debug.LogError($"[{gameObject.name}] GameManager no encontrado!");
    }

    void Update()
    {
        // Si el jugador está en rango y presiona E
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E))
        {
            MostrarDialogo();
        }
    }

    void MostrarDialogo()
    {
        if (manager == null) return;

        string dialogo = DeterminarDialogo();
        List<string> paginas = SepararEnPaginas(dialogo);
        
        manager.NPCShowText(paginas, npcName, npcImage, typingSound);
        StartCoroutine(ManejarMisionesDespuesDialogo());
    }

    string DeterminarDialogo()
    {
        // Si ya completó la misión de la aguja, mostrar diálogo para ir al minijuego
        if (manager.HasMission("BuscarAguja") && manager.HasMission("BuscarHilo"))
        {
            todasMisionesCompletadas = true;
            return dialogoFase3;
        }
        // Si completó la misión del hilo, dar misión de la aguja
        else if (manager.HasMission("BuscarHilo"))
        {
            return dialogoFase2;
        }
        // Primera interacción - dar misión del hilo
        else
        {
            return dialogoFase1;
        }
    }

    IEnumerator ManejarMisionesDespuesDialogo()
    {
        // Esperar a que termine el diálogo
        yield return new WaitUntil(() => manager.DialogFinished);

        // Si todas las misiones están completadas, cargar minijuego
        if (todasMisionesCompletadas)
        {
            yield return new WaitForSeconds(delayAntesCambioEscena);
            UnityEngine.SceneManagement.SceneManager.LoadScene(minijuegoEscena);
            yield break;
        }

        // Dar misiones según la fase
        if (manager.HasMission("BuscarHilo") && !string.IsNullOrEmpty(misionADarFase2))
        {
            manager.AddMission(misionADarFase2);
        }
        else if (!string.IsNullOrEmpty(misionADarFase1))
        {
            manager.AddMission(misionADarFase1);
        }
    }

    List<string> SepararEnPaginas(string texto)
    {
        List<string> paginas = new List<string>();
        string[] palabras = texto.Split(' ');
        string paginaActual = "";
        int caracteresEnPagina = 0;

        foreach (string palabra in palabras)
        {
            if (caracteresEnPagina + palabra.Length + 1 <= maxCharactersPerPage)
            {
                paginaActual += (caracteresEnPagina == 0 ? "" : " ") + palabra;
                caracteresEnPagina += palabra.Length + 1;
            }
            else
            {
                paginas.Add(paginaActual);
                paginaActual = palabra;
                caracteresEnPagina = palabra.Length;
            }
        }

        if (!string.IsNullOrEmpty(paginaActual))
            paginas.Add(paginaActual);

        return paginas;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            if (interactUI != null)
                interactUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            if (interactUI != null)
                interactUI.SetActive(false);
        }
    }
}