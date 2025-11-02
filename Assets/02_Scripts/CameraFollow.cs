using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Camara sigue a la protagonista
    public Transform target;
    
    [Header("Control")]
    [Tooltip("Si está en false, la cámara no seguirá al target")]
    public bool isFollowing = true;
    
    private void LateUpdate()
    {
        if (isFollowing && target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
    
    /// <summary>
    /// Activa o desactiva el seguimiento de la cámara
    /// </summary>
    public void SetFollowing(bool state)
    {
        isFollowing = state;
    }

}
