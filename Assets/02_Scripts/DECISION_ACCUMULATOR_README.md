Decision Accumulator (ChoiceCounterManager)

What changed
- The project already contains `ChoiceCounterManager.cs`, a persistent singleton that stores counts of player choices.
- I added persistence (save/load) to `ChoiceCounterManager` using PlayerPrefs (key: `ChoiceCounters_v1`). It now loads saved counters in Awake and saves on application quit.
- `LevelSelector.ChangeLevelSelector(...)` now calls `ChoiceCounterManager.EnsureExists()` before loading a scene so the accumulator is present when the player enters a level via the level menu.
- `EndingSceneController` now ensures the accumulator exists and uses tie-breaking by random choice when counters are equal.

How to test in Unity
1. Open the project in Unity.
2. From the top menu, create a new temporary scene where you can reference the `ChoiceCounterManager` prefab or component (or rely on auto-created instance).
3. Start the game, perform some dialogue choices that call `ChoiceCounterManager.Instance.IncrementChoice("SaberSobreElCirco")` or `"SaberSobreMi"`.
4. Save and quit (or press Play stop to trigger OnApplicationQuit save). Relaunch and check that the saved counters are loaded (if debugMode enabled, logs show load/save).
5. From the Level Selector UI, open the level selection and load a level: counters should persist because `EnsureExists()` is called before loading.
6. Play up to the ending: `EndingSceneController` will select the ending according to highest counter; if tied, it will choose randomly.

Notes
- The accumulator is implemented in `ChoiceCounterManager.cs`. I avoided putting this logic into `GameManager` as requested.
- I did NOT modify Trapecista scenes or include Trapecista in Build Settings as you requested to leave it out of this revision.

Next steps (recommended)
- If you want counters to persist across separate play sessions reliably, consider adding an explicit UI option to "Save progress" or call `ChoiceCounterManager.SaveToPlayerPrefs()` at key moments (e.g., after completing a day's activities).
- Review where your dialogue choices call `ChoiceCounterManager.Instance.IncrementChoice(...)` and ensure the choice keys used match the keys the ending expects ("SaberSobreElCirco", "SaberSobreMi").

File list changed
- Modified: Assets/02_Scripts/ChoiceCounterManager.cs
- Modified: Assets/02_Scripts/LevelSelector.cs
- Modified: Assets/02_Scripts/EndingSceneController.cs
- Added: Assets/02_Scripts/DECISION_ACCUMULATOR_README.md

Branching note
- Please create a new git branch from `cambiarDiasBeta` before committing these changes, as requested.
