using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private GameObject interactUI;

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //Si el jugador esta cerca, la UI se activa
    public void ShowIndicator(bool state)
    {
        if (interactUI != null)
        {
            interactUI.SetActive(state);
        }
    }
    public void Interact()
    {
        Destroy(gameObject);

        if (animator != null)
        {
            animator.SetTrigger("pickUp");
        }
    }
}
