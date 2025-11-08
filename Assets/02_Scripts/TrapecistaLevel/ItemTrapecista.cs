using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrapecista : MonoBehaviour, IInteractable
{
    [Header("Configuración del Ítem")]
    [SerializeField] private GameObject interactUI;
    [SerializeField] private Sprite itemIcon;

    [Header("Misiones")]
    [SerializeField] private string missionToComplete = "";
    [SerializeField] private string missionToGive = "";
    [SerializeField] private bool requiresMission = false;
    [SerializeField] private string lockedMessage = "No necesito esto ahora...";

    public void ShowIndicator(bool state)
    {
        if (interactUI != null)
            interactUI.SetActive(state);
    }

    public void Interact()
    {
        if (requiresMission && !string.IsNullOrEmpty(missionToComplete))
        {
            if (!GameManager.Instance.HasMission(missionToComplete))
            {
                GameManager.Instance.ShowNonCollectableText(lockedMessage);
                return;
            }
        }

        if (!string.IsNullOrEmpty(missionToComplete))
            GameManager.Instance.CompleteMission(missionToComplete);

        if (!string.IsNullOrEmpty(missionToGive))
            GameManager.Instance.AddMission(missionToGive);

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddItem(itemIcon);

        Destroy(gameObject);
    }
}
