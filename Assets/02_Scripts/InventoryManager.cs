using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; // Para acceder desde otros scripts

    public Transform inventoryPanel; // Donde se mostrarán los íconos
    public GameObject inventorySlotPrefab; // El prefab del ítem visual

    private List<Sprite> collectedItems = new List<Sprite>(); // Lista interna

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(Sprite itemIcon)
    {
        collectedItems.Add(itemIcon);

        // Crear un slot visual
        GameObject newSlot = Instantiate(inventorySlotPrefab, inventoryPanel);
        newSlot.GetComponent<Image>().sprite = itemIcon;
    }
}
