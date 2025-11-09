using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Añade opciones en el menú Tools para invocar las acciones de DebugEndingTester
/// directamente desde la barra superior (útil si no encuentras el engranaje/context menu).
/// Estas opciones funcionan en Play Mode.
/// </summary>
public static class DebugEndingTesterMenu
{
    const string menuRoot = "Tools/Debug Ending Tester/";

    [MenuItem(menuRoot + "Incrementar SaberSobreElCirco +1")]
    public static void Menu_IncrementCirco()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            Debug.LogWarning("Entradas de depuración solo funcionan en Play Mode. Entra en Play y vuelve a usar el menú.");
            return;
        }
#endif
        ChoiceCounterManager.EnsureExists();
        ChoiceCounterManager.Instance?.IncrementChoice("SaberSobreElCirco");
        Debug.Log("[DebugEndingTesterMenu] Incrementado SaberSobreElCirco");
    }

    [MenuItem(menuRoot + "Incrementar SaberSobreMi +1")]
    public static void Menu_IncrementYo()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            Debug.LogWarning("Entradas de depuración solo funcionan en Play Mode. Entra en Play y vuelve a usar el menú.");
            return;
        }
#endif
        ChoiceCounterManager.EnsureExists();
        ChoiceCounterManager.Instance?.IncrementChoice("SaberSobreMi");
        Debug.Log("[DebugEndingTesterMenu] Incrementado SaberSobreMi");
    }

    [MenuItem(menuRoot + "Mostrar todos los contadores")]
    public static void Menu_ShowCounters()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            Debug.LogWarning("Entradas de depuración solo funcionan en Play Mode. Entra en Play y vuelve a usar el menú.");
            return;
        }
#endif
        ChoiceCounterManager.EnsureExists();
        if (ChoiceCounterManager.Instance != null)
            ChoiceCounterManager.Instance.ShowAllCounters();
        else
            Debug.LogWarning("[DebugEndingTesterMenu] ChoiceCounterManager no disponible");
    }

    [MenuItem(menuRoot + "Forzar misión y decidir final")]
    public static void Menu_ForceMissionAndDecide()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            Debug.LogWarning("Entradas de depuración solo funcionan en Play Mode. Entra en Play y vuelve a usar el menú.");
            return;
        }
#endif
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[DebugEndingTesterMenu] GameManager.Instance es null");
            return;
        }
        string mission = "Abrir portal secreto";
        GameManager.Instance.AddMission(mission);
        Debug.Log($"[DebugEndingTesterMenu] Misión '{mission}' añadida al GameManager");
        MissionToEndingChooserHelper.DecideAndLoad();
    }
}
