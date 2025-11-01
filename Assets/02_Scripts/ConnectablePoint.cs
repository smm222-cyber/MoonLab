using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// punto conectable
// acepta una sola conexión
// puede restringir con qué id conectar
[DisallowMultipleComponent]
public class ConnectablePoint : MonoBehaviour
{
    public string pointId;

    public Transform anchor;

    public GameObject highlightVisual;

    public string allowedTargetId;

    private ConnectionLine connectedLine;

    // id del punto
    public string GetId()
    {
        if (!string.IsNullOrEmpty(pointId)) return pointId;
        return gameObject.name;
    }

    // posición del ancla para enganchar la línea
    public Vector3 GetAnchorPosition()
    {
        if (anchor != null) return anchor.position;
        return transform.position;
    }

    // si puede aceptar otra conexión
    public bool CanAcceptConnection()
    {
        return connectedLine == null;
    }

    // si permite conectarse con el otro punto
    public bool AllowsConnectionTo(ConnectablePoint other)
    {
        if (other == null) return false;
        if (string.IsNullOrEmpty(allowedTargetId)) return true;
        return allowedTargetId == other.GetId();
    }

    // activar o desactivar el visual de highlight
    public void SetHighlighted(bool state)
    {
        if (highlightVisual != null) highlightVisual.SetActive(state);
    }

    // registrar la línea conectada
    public void SetConnected(ConnectionLine line)
    {
        connectedLine = line;
    }

    // borrar la referencia de conexión
    public void ClearConnection()
    {
        connectedLine = null;
    }

    // gizmo editor para ver el ancla
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GetAnchorPosition(), 0.12f);
    }
}
