using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPlayerItem : MonoBehaviour
{
    //Para el area de deteccion del item
    [SerializeField] private Transform interactController;
    [SerializeField] private Vector2 boxDimensions= new Vector2(2f,2f);
    //Layermask donde pondremos todos los items interactuables
    [SerializeField] private LayerMask interactiveLayers;
    private List<IInteractable> highlightedItems = new List<IInteractable>();

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectItems();
        //Se tiene un input manager llamado interact que se activa cuando apretas la tecla E
        if (Input.GetButtonDown("Interact"))
        {
            Interact();

            animator.SetTrigger("pickUp");
        }
    }
    void DetectItems()
    {
        Collider2D[] nearbyItems = Physics2D.OverlapBoxAll(interactController.position, boxDimensions, 0f, interactiveLayers);

        // Primero, desactivar los indicadores de los ítems que ya no están cerca
        foreach (IInteractable item in highlightedItems)
        {
            item.ShowIndicator(false);
        }
        highlightedItems.Clear();

        // Activar indicador en todos los ítems cercanos
        foreach (Collider2D col in nearbyItems)
        {
            if (col.TryGetComponent(out IInteractable item))
            {
                item.ShowIndicator(true);
                highlightedItems.Add(item);
            }
        }
    }
    void Interact()
    {
        //Busca los colliders dentro del area de deteccion
        Collider2D[] grabbedItems=Physics2D.OverlapBoxAll(interactController.position,boxDimensions,0f,interactiveLayers);
        foreach (Collider2D i in grabbedItems)
        {
            Debug.Log(i.name);
            //Interactuar con cualquier objeto que implemente IInteractable
            if (i.TryGetComponent(out IInteractable item))
            {
                item.Interact();
            }
        }
       
        
    }
    //Solo es guia visual del area de recoleccion
    private void OnDrawGizmos()
    {
       
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(interactController.position, boxDimensions);
    }
}
