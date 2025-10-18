using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    //Para aplicar a item y non collectable item
    void ShowIndicator(bool state);
    void Interact();
}
