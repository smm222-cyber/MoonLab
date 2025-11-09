using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Small runtime test helper to exercise ChoiceCounterManager persistence and scene transitions.
/// Attach this to a GameObject in any scene (for quick tests use Assets/01_Scenes/SampleScene.unity).
/// It provides an on-screen UI to increment the two choice counters, save/load PlayerPrefs and simulate scene loads.
/// </summary>
public class DecisionAccumulatorTest : MonoBehaviour
{
    public bool showGui = true;
    private int circo = 0;
    private int yo = 0;

    void Start()
    {
        // Ensure the manager exists so tests are meaningful even when launching from level selector
        ChoiceCounterManager.EnsureExists();
        RefreshCounts();
    }

    void RefreshCounts()
    {
        if (ChoiceCounterManager.Instance != null)
        {
            circo = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreElCirco");
            yo = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreMi");
        }
        else
        {
            circo = 0; yo = 0;
        }
    }

    void OnGUI()
    {
        if (!showGui) return;

        GUILayout.BeginArea(new Rect(10, 10, 280, 220), GUI.skin.box);
        GUILayout.Label("DecisionAccumulator Test");

        GUILayout.Label($"Circo: {circo}    |    Yo: {yo}");

        if (GUILayout.Button("Increment Circo"))
        {
            ChoiceCounterManager.EnsureExists();
            ChoiceCounterManager.Instance?.IncrementChoice("SaberSobreElCirco");
            RefreshCounts();
        }

        if (GUILayout.Button("Increment Yo"))
        {
            ChoiceCounterManager.EnsureExists();
            ChoiceCounterManager.Instance?.IncrementChoice("SaberSobreMi");
            RefreshCounts();
        }

        GUILayout.Space(6);

        if (GUILayout.Button("Save to PlayerPrefs"))
        {
            ChoiceCounterManager.Instance?.SaveToPlayerPrefs();
            Debug.Log("[DecisionAccumulatorTest] Saved counters to PlayerPrefs.");
        }

        if (GUILayout.Button("Load from PlayerPrefs"))
        {
            ChoiceCounterManager.EnsureExists();
            ChoiceCounterManager.Instance?.LoadFromPlayerPrefs();
            RefreshCounts();
            Debug.Log("[DecisionAccumulatorTest] Loaded counters from PlayerPrefs.");
        }

        GUILayout.Space(6);

        if (GUILayout.Button("Simulate Scene Load (SampleScene)"))
        {
            // Ensure existence before leaving
            ChoiceCounterManager.EnsureExists();
            Debug.Log("[DecisionAccumulatorTest] Loading SampleScene to simulate scene change...");
            SceneManager.LoadScene("SampleScene");
        }

        if (GUILayout.Button("Show All Counters (Log)"))
        {
            ChoiceCounterManager.Instance?.ShowAllCounters();
        }

        GUILayout.EndArea();
    }
}
