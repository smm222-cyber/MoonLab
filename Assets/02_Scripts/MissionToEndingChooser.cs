using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Método estático helper para decidir y cargar un final inmediatamente desde otros scripts.
/// </summary>
public static class MissionToEndingChooserHelper
{
    /// <summary>
    /// Decide qué escena de final cargar usando los contadores y carga la escena.
    /// </summary>
    /// <param name="finalIfCirco">Nombre de la escena si gana 'SaberSobreElCirco'</param>
    /// <param name="finalIfYo">Nombre de la escena si gana 'SaberSobreMi'</param>
    public static void DecideAndLoad(string finalIfCirco = "Final1", string finalIfYo = "Final2")
    {
        // Si hay una instancia de MissionToEndingChooser en la escena, usar sus valores configurados
        var inst = Object.FindObjectOfType<MissionToEndingChooser>();
        if (inst != null)
        {
            finalIfCirco = string.IsNullOrEmpty(inst.finalIfCirco) ? finalIfCirco : inst.finalIfCirco;
            finalIfYo = string.IsNullOrEmpty(inst.finalIfYo) ? finalIfYo : inst.finalIfYo;
            Debug.Log($"[MissionToEndingChooser.DecideAndLoad] Usando nombres de instancia: finalIfCirco={finalIfCirco}, finalIfYo={finalIfYo}");
        }

        ChoiceCounterManager.EnsureExists();

        int circo = 0;
        int yo = 0;
        if (ChoiceCounterManager.Instance != null)
        {
            circo = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreElCirco");
            yo = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreMi");
        }

        string chosenScene;
        if (circo > yo) chosenScene = finalIfCirco;
        else if (yo > circo) chosenScene = finalIfYo;
        else chosenScene = (Random.value >= 0.5f) ? finalIfCirco : finalIfYo;

        Debug.Log($"[MissionToEndingChooser.DecideAndLoad] circo={circo} yo={yo} -> Cargando: {chosenScene}");

        try
        {
            EndingSceneController.LoadEndingScene(chosenScene);
        }
        catch (System.Exception)
        {
            SceneManager.LoadScene(chosenScene);
        }
    }
}

/// <summary>
/// Observa una misión específica y, cuando aparece, decide
/// qué escena de final cargar usando los contadores de ChoiceCounterManager.
/// Regla: si un contador tiene más puntos -> ese final. Si empate -> aleatorio.
/// </summary>
public class MissionToEndingChooser : MonoBehaviour
{
    [Tooltip("Nombre de la misión que activará la elección del final (p. ej. 'Abrir portal secreto')")]
    public string missionName = "Abrir portal secreto";

    [Tooltip("Nombre de la escena de final si gana 'SaberSobreElCirco'")]
    public string finalIfCirco = "Final1";

    [Tooltip("Nombre de la escena de final si gana 'SaberSobreMi'")]
    public string finalIfYo = "Final2";

    [Tooltip("Evitar reactivar la lógica varias veces")]
    public bool triggered = false;

    void Update()
    {
        if (triggered) return;
        if (GameManager.Instance == null) return;

        if (!string.IsNullOrEmpty(missionName) && GameManager.Instance.HasMission(missionName))
        {
            triggered = true;
            Debug.Log($"[MissionToEndingChooser] Misión '{missionName}' detectada. Decidiendo final según contadores...");

            // Asegurar que exista el manager de contadores
            ChoiceCounterManager.EnsureExists();

            int circo = 0;
            int yo = 0;
            if (ChoiceCounterManager.Instance != null)
            {
                circo = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreElCirco");
                yo = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreMi");
            }

            string chosenScene;
            if (circo > yo)
            {
                chosenScene = finalIfCirco;
                Debug.Log($"[MissionToEndingChooser] Final seleccionado: CIRCO ({circo} vs {yo}) -> {chosenScene}");
            }
            else if (yo > circo)
            {
                chosenScene = finalIfYo;
                Debug.Log($"[MissionToEndingChooser] Final seleccionado: YO ({yo} vs {circo}) -> {chosenScene}");
            }
            else
            {
                // Empate -> elegir aleatoriamente
                bool chooseCirco = Random.value >= 0.5f;
                chosenScene = chooseCirco ? finalIfCirco : finalIfYo;
                Debug.Log($"[MissionToEndingChooser] EMPATE ({circo} vs {yo}) - elegido aleatoriamente: {chosenScene}");
            }

            if (!string.IsNullOrEmpty(chosenScene))
            {
                // Usar el helper de EndingSceneController si existe, sino SceneManager
                Debug.Log($"[MissionToEndingChooser] Cargando escena de final: {chosenScene}");
                // Preferir método público de EndingSceneController si existe
                try
                {
                    EndingSceneController.LoadEndingScene(chosenScene);
                }
                catch (System.Exception)
                {
                    SceneManager.LoadScene(chosenScene);
                }
            }
        }
    }
}
