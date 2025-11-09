DecisionAccumulator Test (quick start)

Purpose
- Small runtime helper to test that the global choice counters persist across scenes and between runs.

Files added
- `Assets/02_Scripts/DecisionAccumulatorTest.cs` - A small MonoBehaviour which exposes an on-screen GUI to increment the two main counters, save/load PlayerPrefs and simulate a scene load.

How to use
1. Open Unity and load the project.
2. Open `Assets/01_Scenes/SampleScene.unity` (or any scene you prefer).
3. Create an empty GameObject in the scene (GameObject > Create Empty).
4. Attach the component `DecisionAccumulatorTest` (Assets/02_Scripts/DecisionAccumulatorTest.cs) to that GameObject.
5. Press Play. A small GUI will appear in the top-left with buttons to:
   - Increment Circo (key: "SaberSobreElCirco")
   - Increment Yo (key: "SaberSobreMi")
   - Save to PlayerPrefs
   - Load from PlayerPrefs
   - Simulate Scene Load (loads "SampleScene")
   - Show all counters to the console

What to verify
- Increment values should update on-screen and in the console.
- Save then stop the Play session, start again, click Load and confirm counters restored.
- Use "Simulate Scene Load" to verify the manager persists across scenes (or re-creates via EnsureExists).

Notes
- The test relies on `ChoiceCounterManager` existing and exposes the same keys used by the ending logic.
- If you want the GUI in a different scene, attach the component there instead.

