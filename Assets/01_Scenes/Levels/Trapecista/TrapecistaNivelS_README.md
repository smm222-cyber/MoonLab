TrapecistaNivelS - README

This new scene is a minimal template for the Trapecista level. It contains placeholder GameObjects; open the scene in Unity and finish the setup as described below.

Required steps in Unity Editor:

1) Open scene: Assets/01_Scenes/Levels/Trapecista/TrapecistaNivelS.unity

2) Assign components:
   - Select 'Trapecista_NPC_Placeholder' and add component `NPCBasicDialog`.
     * Set `npcName` to "Trapecista".
     * Set `usesMissionSystem` = true.
     * Add a mission entry:
         - dialogueText: "¿Puedes ayudarme a coser esta tela?"
         - missionToGive: "BuscarHilo"
         - missionToComplete: (leave blank)
         - missionRequired: (leave blank)
     * (Optionally) set interactUI child, maxCharactersPerPage, typingSound.
   - Add component `TrapecistaInteraction` (or assign existing script) to the same GameObject.
     * Set `sewingSceneName` to "SewingScene" (or the sewing scene you use).
     * Set `interactionKey` to O and `goToSewingKey` to L.

3) Create item prefabs in the Prefabs folder (or use the placeholders):
   - Hilo (item) - mission name to complete when picked: "BuscarHilo"
   - Aguja (item) - mission name to complete when picked: "BuscarAguja"

4) Place Hilo and Aguja item prefabs in the scene where the player can find them. Configure your Item script to call:
      GameManager.Instance.CompleteMission("BuscarHilo");
      GameManager.Instance.CompleteMission("BuscarAguja");
   when picked up.

5) Make sure the scene is added to Build Settings (File -> Build Settings). The repository EditorBuildSettings.asset has been updated to include this scene, but if you open Unity you can confirm and reorder as necessary.

Mission names to copy/paste:
  - BuscarHilo
  - BuscarAguja

Notes:
- The prefab placeholders here are minimal YAML stubs. In Unity, replace them with proper prefabs containing your Item script and sprite.
- The scene created is minimal; add your UI, camera settings, lighting, audio and NPC sprites as in Pintacaritas_Level.

If you want I can also create a copy of `Pintacaritas_Level.unity` renamed and patch its mission strings, but that will be a larger file change. Tell me if you prefer that approach and I'll proceed.