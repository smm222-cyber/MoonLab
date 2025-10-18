using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonCollectableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactUI;
    public string text = "";
    GameManager manager;
    void Start()
    {
        manager=FindObjectOfType<GameManager>();
    }
    public void ShowIndicator(bool state)
    {
        if (interactUI != null)
            interactUI.SetActive(state);
    }

    public void Interact()
    {
        manager.ShowNonCollectableText(text);
    }
}
