# Trapecista Level Scene Setup Guide

## 1. TrapecistaNPC Setup
1. Create an empty GameObject named "TrapecistaNPC"
2. Add the following components:
   - Sprite Renderer
     - Assign the Trapecista character sprite
   - Box Collider 2D
     - Set as Trigger
     - Size: Adjust to match character sprite
   - TrapecistaNPC script
     - Set NPC Name: "Trapecista"
     - Assign NPC Image (same as sprite)
     - Set Max Characters Per Page: 40
     - Fill in all dialogue fields from inspector
   - AudioSource
     - Assign typing sound clip
     - Set Spatial Blend to 0 (2D)

3. Create child GameObject "InteractionUI"
   - Add UI elements to show "Press E to interact"
   - Reference this in TrapecistaNPC's InteractUI field

## 2. Mission Items Setup

### Hilo Item
1. Create empty GameObject "Hilo_Item"
2. Add components:
   - Sprite Renderer
     - Assign thread/string sprite
   - Box Collider 2D
     - Set as Trigger
   - InteractableItem script
     - Set Item ID: "BuscarHilo"
   - AudioSource for pickup sound
  - (Alternative) Use ItemSpawner to spawn items automatically. See below.

### Aguja Item
1. Create empty GameObject "Aguja_Item"
2. Add components:
   - Sprite Renderer
     - Assign needle sprite
   - Box Collider 2D
     - Set as Trigger
   - InteractableItem script
     - Set Item ID: "BuscarAguja"
   - AudioSource for pickup sound
  - (Alternative) Use ItemSpawner to spawn items automatically. See below.

## 2.1 ItemSpawner (convenience)

1. Add an empty GameObject to the scene and name it `ItemSpawner`.
2. Attach the `ItemSpawner` script (`Assets/02_Scripts/NivelTrapecista/ItemSpawner.cs`).
3. Create `ItemDefinition` assets for your items: Right-click in Project window -> Create -> NivelTrapecista -> Item Definition.
   - Set `itemId` to `BuscarHilo` or `BuscarAguja`.
   - Assign sprite and pickup sound and optional `interactUI` prefab.
4. Add the `ItemDefinition` assets to the `items` list on the `ItemSpawner` component.
5. Create empty child Transforms under the `ItemSpawner` GameObject to mark spawn positions, or add Transforms from the scene to the `spawnPoints` list.
6. Run the scene: `ItemSpawner` will instantiate GameObjects with `SpriteRenderer`, `BoxCollider2D`, `AudioSource` and `InteractableItem` pre-configured.

This approach avoids manual prefab creation and ensures items are ready to pick up at runtime.
## 3. Scene Requirements
1. Ensure GameManager is present
2. Verify Player spawn point is set
3. Set up appropriate environment colliders
4. Configure lighting and background

## 4. Testing Checklist
- [ ] TrapecistaNPC interaction zone works
- [ ] Dialogue appears correctly
- [ ] Items can be collected
- [ ] Mission progression works
- [ ] Scene transition triggers after both items collected

## Notes
- Position the items in logical locations that make sense for the level flow
- Ensure all prefabs are properly connected in the Unity Editor
- Test the full mission sequence before finalizing