using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingController : MonoBehaviour
{
    [Header("Input / Layers")]
    public LayerMask nodeLayerMask; // layer donde están los ConnectablePoint

    [Header("Linea / Visual")]
    public Material lineMaterial;
    public float lineWidth = 0.05f;

    // Estado en tiempo de ejecución
    private ConnectablePoint startPoint;
    private ConnectionLine previewLine;
    private List<ConnectionLine> connections = new List<ConnectionLine>();

    void Update()
    {
    // iniciar arrastre
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(wp, nodeLayerMask);
            if (col != null)
            {
                var p = col.GetComponentInParent<ConnectablePoint>();
                if (p != null && p.CanAcceptConnection())
                {
                    BeginDrag(p, wp);
                }
            }
        }
    // actualizar preview mientras arrastras
        if (previewLine != null && startPoint != null && Input.GetMouseButton(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            wp.z = 0f;
            previewLine.UpdateTarget(wp);
            HighlightUnderMouse(true);
        }
    // soltar: finalizar o cancelar
        if (previewLine != null && Input.GetMouseButtonUp(0))
        {
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(wp, nodeLayerMask);
            ConnectablePoint endPoint = null;
            if (col != null) endPoint = col.GetComponentInParent<ConnectablePoint>();

            if (endPoint != null && endPoint != startPoint && endPoint.CanAcceptConnection() && !ConnectionExists(startPoint, endPoint) && startPoint.AllowsConnectionTo(endPoint) && endPoint.AllowsConnectionTo(startPoint))
            {
                FinalizeConnection(startPoint, endPoint);
            }
            else
            {
                // cancelar
                Destroy(previewLine.gameObject);
            }

            ClearHighlights();
            previewLine = null;
            startPoint = null;
        }
    }

    void BeginDrag(ConnectablePoint p, Vector3 worldPoint)
    {
        startPoint = p;
        GameObject go = new GameObject("ConnectionPreview");
        go.transform.parent = transform;
        var conn = go.AddComponent<ConnectionLine>();
        conn.InitializePreview(startPoint.GetAnchorPosition(), worldPoint, lineMaterial, lineWidth);
        previewLine = conn;
    }

    bool ConnectionExists(ConnectablePoint a, ConnectablePoint b)
    {
        foreach (var c in connections)
            if (c.Links(a, b)) return true;
        return false;
    }

    void FinalizeConnection(ConnectablePoint a, ConnectablePoint b)
    {
        // reutilizar preview si existe
        ConnectionLine conn = previewLine;
        if (conn == null)
        {
            GameObject go = new GameObject("Connection");
            go.transform.parent = transform;
            conn = go.AddComponent<ConnectionLine>();
            conn.InitializeConnection(a.GetAnchorPosition(), b.GetAnchorPosition(), lineMaterial, lineWidth);
        }
        else
        {
            conn.InitializeConnection(a.GetAnchorPosition(), b.GetAnchorPosition(), lineMaterial, lineWidth);
        }

        conn.SetEndpoints(a, b);
        connections.Add(conn);
        a.SetConnected(conn);
        b.SetConnected(conn);

        // feedback visual
        Debug.Log($"Conectado: {a.GetId()} -> {b.GetId()}");
    }

    void HighlightUnderMouse(bool enable)
    {
        Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D col = Physics2D.OverlapPoint(wp, nodeLayerMask);
        ClearHighlights();
        if (col != null)
        {
            var node = col.GetComponentInParent<ConnectablePoint>();
            if (node != null && node != startPoint && node.CanAcceptConnection()) node.SetHighlighted(true);
        }
    }

    void ClearHighlights()
    {
        var all = FindObjectsOfType<ConnectablePoint>();
        foreach (var n in all) n.SetHighlighted(false);
    }

    // helpers opcionales
    public void ClearAllConnections()
    {
        foreach (var c in connections.ToArray()) c.DestroySelf();
        connections.Clear();
        var all = FindObjectsOfType<ConnectablePoint>();
        foreach (var n in all) n.ClearConnection();
    }
}
