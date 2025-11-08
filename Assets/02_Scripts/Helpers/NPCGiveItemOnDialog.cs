using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Helpers/NPC Give Item On Dialog")]
public class NPCGiveItemOnDialog : MonoBehaviour
{
    [Tooltip("Icono (Sprite) del item que se añadirá al inventario cuando se invoque GiveItem(). Si está vacío, solo se harán las misiones.")]
    public Sprite itemIcon;

    [Tooltip("Si true, se añadirá el sprite al inventario llamando a InventoryManager.Instance.AddItem(itemIcon)")]
    public bool addToInventory = true;

    [Tooltip("Misión que se completará al dar el item (opcional)")]
    public string missionToComplete;

    [Tooltip("Misión que se dará/activará al dar el item (opcional)")]
    public string missionToGive;

    [Tooltip("Mensaje de log para debugging si falta alguna dependencia")]
    public bool verbose = true;

    // Método público que puedes enlazar al UnityEvent `onDialogFinished` de `NPCBasicDialog`
    public void GiveItem()
    {
        // Primero añadir al inventario (si aplica)
        if (addToInventory)
        {
            if (itemIcon != null)
            {
                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.AddItem(itemIcon);
                }
                else if (verbose)
                {
                    Debug.LogWarning("InventoryManager.Instance es null. No se puede añadir el item: " + itemIcon.name);
                }
            }
            else if (verbose)
            {
                Debug.LogWarning("itemIcon no está asignado en NPCGiveItemOnDialog para " + gameObject.name);
            }
        }

        // Completar misión si aplica
        if (!string.IsNullOrEmpty(missionToComplete) && GameManager.Instance != null)
        {
            GameManager.Instance.CompleteMission(missionToComplete);
        }

        // Dar nueva misión si aplica
        if (!string.IsNullOrEmpty(missionToGive) && GameManager.Instance != null)
        {
            GameManager.Instance.AddMission(missionToGive);
        }
    }
}
