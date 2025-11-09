using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Observa una misión específica en el GameManager y, cuando aparece,
/// carga la escena de final indicada. Útil para activar finales desde misiones.
/// </summary>
public class MissionToEndingTrigger : MonoBehaviour
{
    [Tooltip("Nombre de la misión que activará el final (ej: 'Abrir portal secreto')")]
    public string missionName = "Abrir portal secreto";

    [Tooltip("Nombre de la escena de final a cargar")]
    public string endingSceneName = "Final1";

    [Tooltip("Evitar reactivar varias veces")]
    public bool triggered = false;

    void Update()
    {
        if (triggered) return;

        if (GameManager.Instance == null) return;

        if (!string.IsNullOrEmpty(missionName) && GameManager.Instance.HasMission(missionName))
        {
            triggered = true;
            Debug.Log($"[MissionToEndingTrigger] Misión '{missionName}' detectada. Cargando escena de final: {endingSceneName}");
            EndingSceneController.LoadEndingScene(endingSceneName);
        }
    }
}
