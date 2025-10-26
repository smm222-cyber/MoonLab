using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script simple para completar misiones al interactuar con objetos
public class MissionTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactUI;
    
    [Header("Misión a completar")]
    public string missionToComplete;
    
    [Header("¿Destruir objeto al completar?")]
    public bool destroyOnComplete = true;
    
    private GameManager manager;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public void ShowIndicator(bool state)
    {
        if (interactUI != null)
            interactUI.SetActive(state);
    }

    public void Interact()
    {
        // Solo completar si la misión está activa
        if (!string.IsNullOrEmpty(missionToComplete) && manager.HasMission(missionToComplete))
        {
            manager.CompleteMission(missionToComplete);
            
            if (destroyOnComplete)
            {
                Destroy(gameObject);
            }
        }
    }
}
