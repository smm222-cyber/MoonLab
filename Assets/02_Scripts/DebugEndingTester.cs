using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Herramienta de depuración para probar contadores y finales sin recorrer todo el juego.
/// Usa el menú de contexto del componente en el Inspector para invocar los métodos.
/// </summary>
public class DebugEndingTester : MonoBehaviour
{
    [Tooltip("Nombre de la misión que activa el final (debe coincidir con la configuración del NPC)")]
    public string testMissionName = "Abrir portal secreto";

    [ContextMenu("Debug: Incrementar SaberSobreElCirco +1")]
    public void DebugIncrementCirco()
    {
        ChoiceCounterManager.EnsureExists();
        ChoiceCounterManager.Instance?.IncrementChoice("SaberSobreElCirco");
        Debug.Log("[DebugEndingTester] Incrementado SaberSobreElCirco");
    }

    [ContextMenu("Debug: Incrementar SaberSobreMi +1")]
    public void DebugIncrementYo()
    {
        ChoiceCounterManager.EnsureExists();
        ChoiceCounterManager.Instance?.IncrementChoice("SaberSobreMi");
        Debug.Log("[DebugEndingTester] Incrementado SaberSobreMi");
    }

    [ContextMenu("Debug: Mostrar todos los contadores")] 
    public void DebugShowCounters()
    {
        ChoiceCounterManager.EnsureExists();
        if (ChoiceCounterManager.Instance != null)
        {
            ChoiceCounterManager.Instance.ShowAllCounters();
        }
        else
        {
            Debug.LogWarning("[DebugEndingTester] ChoiceCounterManager no disponible");
        }
    }

    [ContextMenu("Debug: Forzar misión y decidir final")] 
    public void DebugForceMissionAndDecide()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[DebugEndingTester] GameManager.Instance es null");
            return;
        }

        // Añadir la misión al GameManager (síncrono en la mayoría de implementaciones)
        GameManager.Instance.AddMission(testMissionName);
        Debug.Log($"[DebugEndingTester] Misión '{testMissionName}' añadida al GameManager");

        // Llamar al helper para decidir y cargar el final inmediatamente
        MissionToEndingChooserHelper.DecideAndLoad();
    }
}
