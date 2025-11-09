using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ItemSpawner : MonoBehaviour
{
    [Tooltip("List of item definitions to spawn (order matters if spawnPoints length matches)")]
    public List<ItemDefinition> items = new List<ItemDefinition>();

    [Tooltip("Place empty child transforms here to mark spawn positions. If empty, will use this GameObject's position.")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Tooltip("Optional parent for spawned objects. Defaults to this GameObject.")]
    public Transform spawnedParent;

    void Start()
    {
        if (spawnedParent == null)
            spawnedParent = this.transform;

        if (items == null || items.Count == 0)
        {
            Debug.LogWarning($"[{name}] No ItemDefinitions assigned to ItemSpawner.");
            return;
        }

        // If no spawn points are defined, use this transform as a single spawn point
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            SpawnAtPoint(items, transform.position);
            return;
        }

        // Spawn items: if there are more items than spawn points, reuse the last spawn point
        for (int i = 0; i < items.Count; i++)
        {
            Transform point = spawnPoints[Mathf.Min(i, spawnPoints.Count - 1)];
            SpawnItem(items[i], point.position);
        }
    }

    void SpawnAtPoint(List<ItemDefinition> itemsToSpawn, Vector3 pos)
    {
        for (int i = 0; i < itemsToSpawn.Count; i++)
        {
            SpawnItem(itemsToSpawn[i], pos + new Vector3(i * 0.5f, 0f, 0f));
        }
    }

    void SpawnItem(ItemDefinition def, Vector3 position)
    {
        if (def == null)
        {
            Debug.LogWarning($"[{name}] Null ItemDefinition encountered, skipping.");
            return;
        }

        GameObject go = new GameObject(def.name ?? def.itemId ?? "Item");
        go.transform.position = position;
        go.transform.parent = spawnedParent;

        // Sprite Renderer
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = def.sprite;

        // Collider (trigger)
        BoxCollider2D col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        // AudioSource
        AudioSource audio = go.AddComponent<AudioSource>();
        audio.playOnAwake = false;
        if (def.pickupSound != null)
            audio.clip = def.pickupSound;

        // InteractableItem script
        InteractableItem interact = go.AddComponent<InteractableItem>();
        interact.itemId = def.itemId;
        interact.pickupSound = def.pickupSound;
        interact.interactUI = def.interactUI;

        // Name for clarity in Hierarchy
        go.name = $"Item_{def.itemId}";
    }
}