using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapecistaNPC : MonoBehaviour, IInteractable
{
    [Header("Información del NPC")]
    public string npcName = "Trapecista";
    public Sprite npcImage;
    public GameObject interactUI;

    [Header("FASE 1: Diálogo inicial")]
    [TextArea(3, 10)]
    public string dialogoInicial = "¡Ay! Mi traje se rasgó... necesito hilo para repararlo. ¿Podrías traerme uno?";
    public string misionADarFase1 = "Buscar hilo";

    [Header("FASE 2: Ya trajo el hilo")]
    public string misionRequeridaFase2 = "Buscar hilo";
    [TextArea(3, 10)]
    public string dialogoFase2 = "¡Perfecto! Ahora solo necesito una aguja para poder coserlo.";
    public string misionACompletarFase2 = "Buscar hilo";
    public string misionADarFase2 = "Buscar aguja";

    [Header("FASE 3: Ya trajo la aguja")]
    public string misionRequeridaFase3 = "Buscar aguja";
    [TextArea(3, 10)]
    public string dialogoFase3 = "¡Gracias! Con esto podré reparar mi traje. ¡Eres un gran amigo!";
    public string misionACompletarFase3 = "Buscar aguja";

    [Header("Audio")]
    public AudioClip typingSound;
    [Header("Configuración Avanzada")]
    public int maxCharactersPerPage = 40;

    private GameManager manager;
    private bool misionFase1Dada = false;
    private bool misionFase2Dada = false;
    private bool misionFase3Completada = false;

    void Start()
    {
        GetManagerReference();
    }

    private void GetManagerReference()
    {
        if (manager == null)
        {
            manager = GameManager.Instance ?? FindObjectOfType<GameManager>();
            if (manager == null)
                Debug.LogError($"GameManager no encontrado para {gameObject.name}");
        }
    }

    public void Interact()
    {
        if (manager == null)
            GetManagerReference();

        string dialogo = DeterminarDialogo();

        List<string> pages = SplitTextIntoPages(dialogo, maxCharactersPerPage);
        manager.NPCShowText(pages, npcName, npcImage, typingSound);

        StartCoroutine(HandleMissionsDespuesDelDialogo());
    }

    IEnumerator HandleMissionsDespuesDelDialogo()
    {
        yield return new WaitUntil(() => manager.DialogFinished);

        // FASE 1: Dar misión de buscar hilo
        if (!misionFase1Dada && !manager.HasMission(misionADarFase1))
        {
            manager.AddMission(misionADarFase1);
            misionFase1Dada = true;
            yield break;
        }

        // FASE 2: Completar "Buscar hilo" y dar "Buscar aguja"
        if (manager.HasMission(misionRequeridaFase2))
        {
            manager.CompleteMission(misionACompletarFase2);
            manager.AddMission(misionADarFase2);
            misionFase2Dada = true;
            yield break;
        }

        // FASE 3: Completar "Buscar aguja"
        if (manager.HasMission(misionRequeridaFase3))
        {
            manager.CompleteMission(misionACompletarFase3);
            misionFase3Completada = true;
            yield break;
        }
    }

    private string DeterminarDialogo()
    {
        if (manager == null)
            return dialogoInicial;

        if (manager.HasMission(misionRequeridaFase3))
            return dialogoFase3;

        if (manager.HasMission(misionRequeridaFase2))
            return dialogoFase2;

        return dialogoInicial;
    }

    private List<string> SplitTextIntoPages(string text, int maxChars)
    {
        List<string> pages = new List<string>();
        string[] words = text.Split(' ');
        string current = "";

        foreach (string word in words)
        {
            if ((current + " " + word).Length > maxChars)
            {
                pages.Add(current);
                current = word;
            }
            else current += (current == "" ? "" : " ") + word;
        }

        if (!string.IsNullOrEmpty(current))
            pages.Add(current);

        return pages;
    }

    public void ShowIndicator(bool state)
    {
        if (interactUI != null)
            interactUI.SetActive(state);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ShowIndicator(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ShowIndicator(false);
    }
}
