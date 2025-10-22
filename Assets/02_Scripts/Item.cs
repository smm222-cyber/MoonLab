using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject interactUI;
    [SerializeField] private Sprite itemIcon; // ?? A�ad� esto (el �cono del globo)

    public void ShowIndicator(bool state)
    {
        if (interactUI != null)
            interactUI.SetActive(state);
    }

    public void Interact()
    {
        // A�adir al inventario
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemIcon);
        }

        // Destruir del mundo
        Destroy(gameObject);
    }
}
