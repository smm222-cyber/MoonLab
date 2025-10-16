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

    // Update is called once per frame
    void Update()
    {
        //Se tiene un input manager llamado interact que se activa cuando apretas la tecla E
        if (Input.GetButtonDown("Interact"))
        {
            Interact();
        }
    }
    void Interact()
    {
        //Busca los colliders dentro del area de deteccion
        Collider2D[] grabbedItems=Physics2D.OverlapBoxAll(interactController.position,boxDimensions,0f,interactiveLayers);
        foreach (Collider2D i in grabbedItems)
        {
            Debug.Log(i.name);
            //Solo los objetos con el script itrm seran recogibles(sujeto a cambio segun la logica de inventario)
            if (i.TryGetComponent(out Item item))
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
