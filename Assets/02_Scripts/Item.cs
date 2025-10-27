using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour,IInteractable
{
    [SerializeField] private GameObject interactUI;
    [SerializeField] private Sprite itemIcon; // ?? A�ad� esto (el �cono del globo)
    
    [Header("Misión (Opcional)")]
    [SerializeField] private string missionToComplete = ""; // Dejar vacío si no completa misión
    [SerializeField] private bool requiresMission = false; // Solo se puede recoger si tienes la misión activa?
    
    [Header("Mensaje si no tienes la misión")]
    [SerializeField] private string lockedMessage = "No necesito esto ahora..."; // Mensaje cuando está bloqueado

    public void ShowIndicator(bool state)
    {
        // Solo mostrar indicador si puede ser recogido
        if (interactUI != null)
        {
            // Si requiere misión y no la tienes, no mostrar indicador
            if (requiresMission && !string.IsNullOrEmpty(missionToComplete))
            {
                if (GameManager.Instance != null && !GameManager.Instance.HasMission(missionToComplete))
                {
                    interactUI.SetActive(false);
                    return;
                }
            }
            
            interactUI.SetActive(state);
        }
    }

    public void Interact()
    {
        // Si requiere misión, verificar que la tenga activa
        if (requiresMission && !string.IsNullOrEmpty(missionToComplete))
        {
            if (GameManager.Instance == null || !GameManager.Instance.HasMission(missionToComplete))
            {
                // No tiene la misión, mostrar mensaje y no recoger
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ShowNonCollectableText(lockedMessage);
                }
                return; // No hacer nada más
            }
        }
        
        // Si llegamos aquí, el item se puede recoger
        
        // Completar la misión si tiene una asignada
        if (!string.IsNullOrEmpty(missionToComplete) && GameManager.Instance != null)
        {
            if (GameManager.Instance.HasMission(missionToComplete))
            {
                GameManager.Instance.CompleteMission(missionToComplete);
            }
        }
        
        // Añadir al inventario
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemIcon);
        }

        // Destruir del mundo
        Destroy(gameObject);
    }
}