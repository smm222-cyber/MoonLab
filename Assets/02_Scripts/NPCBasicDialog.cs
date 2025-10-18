using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBasicDialog : MonoBehaviour,IInteractable
{
    public string name;
    public Sprite image;
    GameManager manager;
    public GameObject interactUI;
    // Start is called before the first frame update
    void Start()
    {
        manager=FindObjectOfType<GameManager>();
    }
//Logica del cuadro de texto
   public void Interact()
    {
        manager.NPCShowText("Texto",name, image);
    }

    public void ShowIndicator(bool state)
    {
        if (interactUI != null)
            interactUI.SetActive(state);
        
    }
}
