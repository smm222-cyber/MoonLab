using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryUI; // Panel del inventario (Canvas o Panel principal)
    private bool isOpen = false;

    void Start()
    {
        // Asegurarte que empiece oculto
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        // Si presiona G, alterna el inventario
        if (Input.GetKeyDown(KeyCode.G))
        {
            isOpen = !isOpen;
            inventoryUI.SetActive(isOpen);
        }
    }
}
