using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionLine : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 pointA;
    private Vector3 pointB;
    private ConnectablePoint endpointA;
    private ConnectablePoint endpointB;

    // iniciar preview
    public void InitializePreview(Vector3 a, Vector3 target, Material mat, float width)
    {
        EnsureLineRenderer(mat, width);
        pointA = a;
        pointB = target;
        lr.positionCount = 2;
        lr.SetPosition(0, pointA);
        lr.SetPosition(1, pointB);
    }

    // iniciar conexión fija
    public void InitializeConnection(Vector3 a, Vector3 b, Material mat, float width)
    {
        EnsureLineRenderer(mat, width);
        pointA = a;
        pointB = b;
        lr.positionCount = 2;
        lr.SetPosition(0, pointA);
        lr.SetPosition(1, pointB);
    }

    // asegurar LineRenderer y aplicar material y ancho
    void EnsureLineRenderer(Material mat, float width)
    {
        lr = GetComponent<LineRenderer>();
        if (lr == null) lr = gameObject.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = width;
        lr.numCapVertices = 8;
        if (mat != null) lr.material = mat;
    }

    // actualizar objetivo en preview
    public void UpdateTarget(Vector3 worldPos)
    {
        pointB = worldPos;
        if (lr != null)
        {
            lr.SetPosition(1, pointB);
        }
    }

    // actualizar posiciones si está anclada a endpoints
    void Update()
    {
        if (lr == null) return;
        if (endpointA != null)
        {
            pointA = endpointA.GetAnchorPosition();
            lr.SetPosition(0, pointA);
        }
        if (endpointB != null)
        {
            pointB = endpointB.GetAnchorPosition();
            lr.SetPosition(1, pointB);
        }
    }

    // guardar referencias de endpoints
    public void SetEndpoints(ConnectablePoint a, ConnectablePoint b)
    {
        endpointA = a;
        endpointB = b;
    }

    // comprueba si enlaza dos puntos dados
    public bool Links(ConnectablePoint a, ConnectablePoint b)
    {
        if (a == null || b == null) return false;
        if (endpointA == null || endpointB == null) return false;
        return (endpointA == a && endpointB == b) || (endpointA == b && endpointB == a);
    }

    // destruir línea y limpiar endpoints
    public void DestroySelf()
    {
        // limpiar referencias de los endpoints
        if (endpointA != null) endpointA.ClearConnection();
        if (endpointB != null) endpointB.ClearConnection();
        Destroy(gameObject);
    }
}
