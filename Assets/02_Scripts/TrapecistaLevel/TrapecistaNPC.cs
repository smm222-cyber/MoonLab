using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapecistaNPC : MonoBehaviour, IInteractable
{
    [Header("Información del NPC")]
    public string npcName = "Trapecista";
    public Sprite npcImage;
    public GameObject interactUI;

    [Header("FASE 1: Diálogo Inicial")]
    [TextArea(3, 10)]
    public string dialogoInicial;

    [Header("FASE 2: Primer encuentro / Misión inicial")]
    public string misionRequeridaFase2 = "Hablar con Trapecista";
    [TextArea(3, 10)]
    public string dialogoFase2;
    public string misionACompletarFase2 = "Hablar con Trapecista";
    public string misionADarFase2 = "Recoger anillo del trapecista";

    [Header("FASE 3: Después de recoger objeto")]
    public string misionRequeridaFase3 = "Recoger anillo del trapecista";
    [TextArea(3, 10)]
    public string dialogoFase3;
    public string misionADarFase3 = "Traer manzana al trapecista";

    [Header("FASE 4: Después de segunda misión")]
    public string misionRequeridaFase4 = "Traer manzana al trapecista";
    [TextArea(3, 10)]
    public string dialogoFase4;
    public string misionACompletarFase4 = "Traer manzana al trapecista";

    [Header("FASE 5: Opciones de diálogo")]
    public string misionRequeridaFase5 = "Volver con Trapecista";
    [TextArea(3, 10)]
    public string dialogoFase5Inicial;

    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        [TextArea(3, 10)]
        public string responseDialogue;
    }
    public List<DialogueChoice> opcionesFase5 = new List<DialogueChoice>();

    [Header("FASE 6: Diálogo final")]
    [TextArea(3, 10)]
    public string dialogoFinal;

    [Header("Audio")]
    public AudioClip typingSound;

    [Header("Configuración Avanzada")]
    public int maxCharactersPerPage = 40;

    [Header("Sistema de UI (Opcional)")]
    public DialogueChoicesUI_Trapecista choicesUISystem;
    public DialogueChoicesUI_Trapecista_Fixed choicesUISystemFixed;

    // Variables internas
    private GameManager manager;
    private bool misionFase2Dada = false;
    private bool misionFase3Dada = false;
    private bool misionFase4Dada = false;
    private bool opcionesFase5Mostradas = false;
    private int opcionSeleccionada = -1;

    // 🔹 Inicialización robusta del GameManager
    private IEnumerator Start()
    {
        // Esperamos un frame para asegurarnos de que GameManager ya se haya inicializado
        yield return null;

        manager = GameManager.Instance ?? FindObjectOfType<GameManager>();
        if (manager == null)
            Debug.LogError($"GameManager no encontrado para {gameObject.name}");
    }

    public void Interact()
    {
        if (manager == null) return; // Seguridad extra

        string dialogoAMostrar = DeterminarDialogo();
        if (string.IsNullOrEmpty(dialogoAMostrar)) return;

        List<string> pages = SplitTextIntoPages(dialogoAMostrar, maxCharactersPerPage);
        manager.NPCShowText(pages, npcName, npcImage, typingSound);

        StartCoroutine(HandleMissionsDespuesDelDialogo());
    }

    private string DeterminarDialogo()
    {
        if (opcionSeleccionada >= 0 && !string.IsNullOrEmpty(dialogoFinal))
            return dialogoFinal;

        if (!string.IsNullOrEmpty(misionRequeridaFase5) && manager.HasMission(misionRequeridaFase5) && !opcionesFase5Mostradas)
            return dialogoFase5Inicial;

        if (!string.IsNullOrEmpty(misionRequeridaFase4) && !manager.HasMission(misionRequeridaFase4) && misionFase3Dada)
            return dialogoFase4;

        if (!string.IsNullOrEmpty(misionRequeridaFase3) && !manager.HasMission(misionRequeridaFase3) && misionFase2Dada)
            return dialogoFase3;

        if (!string.IsNullOrEmpty(misionRequeridaFase2) && manager.HasMission(misionRequeridaFase2) && !misionFase2Dada)
            return dialogoFase2;

        return dialogoInicial;
    }

    private IEnumerator HandleMissionsDespuesDelDialogo()
    {
        yield return new WaitUntil(() => manager.DialogFinished);

        // FASE 2
        if (!misionFase2Dada && manager.HasMission(misionRequeridaFase2))
        {
            if (!string.IsNullOrEmpty(misionACompletarFase2)) manager.CompleteMission(misionACompletarFase2);
            if (!string.IsNullOrEmpty(misionADarFase2)) { manager.AddMission(misionADarFase2); misionFase2Dada = true; }
        }
        // FASE 3
        else if (!misionFase3Dada && !manager.HasMission(misionRequeridaFase3) && misionFase2Dada)
        {
            if (!string.IsNullOrEmpty(misionADarFase3)) { manager.AddMission(misionADarFase3); misionFase3Dada = true; }
        }
        // FASE 4
        else if (!misionFase4Dada && !manager.HasMission(misionRequeridaFase4) && misionFase3Dada)
        {
            if (!string.IsNullOrEmpty(misionACompletarFase4)) manager.CompleteMission(misionACompletarFase4);
            misionFase4Dada = true;
        }
        // FASE 5: Opciones de diálogo
        else if (!opcionesFase5Mostradas && manager.HasMission(misionRequeridaFase5))
        {
            opcionesFase5Mostradas = true;
            yield return StartCoroutine(MostrarOpcionesDeDialogo());
        }
    }

    private List<string> SplitTextIntoPages(string text, int maxChars)
    {
        List<string> pages = new List<string>();
        if (string.IsNullOrEmpty(text)) return pages;
        if (text.Length <= maxChars) { pages.Add(text); return pages; }

        string[] words = text.Split(' ');
        string currentPage = "";

        foreach (string word in words)
        {
            string testLine = currentPage.Length == 0 ? word : currentPage + " " + word;
            if (testLine.Length > maxChars)
            {
                if (currentPage.Length > 0) { pages.Add(currentPage); currentPage = word; }
                else { pages.Add(word); currentPage = ""; }
            }
            else currentPage = testLine;
        }
        if (currentPage.Length > 0) pages.Add(currentPage);

        return pages;
    }

    private IEnumerator MostrarOpcionesDeDialogo()
    {
        if (opcionesFase5 == null || opcionesFase5.Count == 0) yield break;

        // Pasamos la lista de DialogueChoice a la UI
        if (choicesUISystemFixed != null) choicesUISystemFixed.ShowChoices(this, opcionesFase5);
        else if (choicesUISystem != null) choicesUISystem.ShowChoices(this, opcionesFase5);
        else
        {
            // Simulación automática para testing
            yield return new WaitForSeconds(2f);
            OnChoiceSelected(0);
        }
    }

    public void OnChoiceSelected(int choiceIndex)
    {
        if (opcionesFase5 == null || choiceIndex < 0 || choiceIndex >= opcionesFase5.Count) return;

        opcionSeleccionada = choiceIndex;
        string responseDialogue = opcionesFase5[choiceIndex].responseDialogue;
        if (!string.IsNullOrEmpty(responseDialogue))
        {
            List<string> pages = SplitTextIntoPages(responseDialogue, maxCharactersPerPage);
            manager.NPCShowText(pages, npcName, npcImage, typingSound);
        }
    }

    // Indicador de interacción
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) ShowIndicator(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) ShowIndicator(false);
    }

    public void ShowIndicator(bool state)
    {
        if (interactUI != null) interactUI.SetActive(state);
    }
}
