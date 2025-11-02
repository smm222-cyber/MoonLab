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
    
    [Header("Completion")]
    [Tooltip("Número de conexiones correctas necesarias para completar. 0 = desactivado")]
    public int requiredConnections = 0;
    
    [Tooltip("Cerrar automáticamente cuando se completen todas las conexiones?")]
    public bool autoCloseOnComplete = true;
    
    [Tooltip("Misión a completar cuando se termine el minijuego (opcional)")]
    public string missionToComplete = "";
    
    [Tooltip("Nueva misión a dar cuando se complete el minijuego (opcional)")]
    public string missionToGive = "";

    // Estado en tiempo de ejecución
    private ConnectablePoint startPoint;
    private ConnectionLine previewLine;
    private List<ConnectionLine> connections = new List<ConnectionLine>();
    private GameManager manager;
    
    void Start()
    {
        manager = GameManager.Instance;
        
        // Verificar configuración al inicio
        if (nodeLayerMask == 0)
        {
            Debug.LogWarning($"[ConnectingController] Node Layer Mask está en 'Nothing'. Configúralo en el Inspector.");
        }
        
        // Buscar todos los puntos conectables
        ConnectablePoint[] points = GetComponentsInChildren<ConnectablePoint>(true);
        Debug.Log($"[ConnectingController] Encontrados {points.Length} puntos conectables");
        
        foreach (var point in points)
        {
            Collider2D col = point.GetComponent<Collider2D>();
            if (col == null)
            {
                Debug.LogError($"[ConnectingController] El punto '{point.name}' NO tiene Collider2D!");
            }
            else if (col.isTrigger)
            {
                Debug.LogWarning($"[ConnectingController] El punto '{point.name}' tiene el Collider marcado como Trigger. Debería estar desmarcado.");
            }
        }
    }

    void OnEnable()
    {
        // Limpiar al activar para empezar limpio
        DestroyAllLines();
        
        Debug.Log("[ConnectingController] Minijuego activado - estado limpio");
    }

    void OnDisable()
    {
        // Limpiar todas las líneas cuando se desactive el minijuego
        DestroyAllLines();
        
        Debug.Log("[ConnectingController] Minijuego desactivado - líneas eliminadas");
    }
    
    // Destruir TODAS las líneas (método más agresivo)
    void DestroyAllLines()
    {
        int totalDestroyed = 0;
        
        // Destruir preview si existe
        if (previewLine != null)
        {
            DestroyImmediate(previewLine.gameObject);
            previewLine = null;
            totalDestroyed++;
        }
        
        // Buscar TODAS las ConnectionLine en la escena y destruirlas
        ConnectionLine[] allLines = FindObjectsOfType<ConnectionLine>();
        Debug.Log($"[ConnectingController] Encontradas {allLines.Length} ConnectionLine en la escena");
        foreach (ConnectionLine line in allLines)
        {
            if (line != null && line.gameObject != null)
            {
                Debug.Log($"  Destruyendo inmediatamente: {line.gameObject.name}");
                DestroyImmediate(line.gameObject);
                totalDestroyed++;
            }
        }
        
        // Buscar y destruir TODOS los LineRenderer en la escena que no deberían estar
        LineRenderer[] allLineRenderers = FindObjectsOfType<LineRenderer>();
        foreach (LineRenderer lr in allLineRenderers)
        {
            if (lr != null && lr.gameObject != null && 
                (lr.gameObject.name.Contains("Connection") || lr.gameObject.name.Contains("Preview")))
            {
                Debug.Log($"  Destruyendo LineRenderer: {lr.gameObject.name}");
                DestroyImmediate(lr.gameObject);
                totalDestroyed++;
            }
        }
        
        // Buscar y destruir hijos del transform con nombres específicos
        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child != null && child != transform && (child.name.Contains("Connection") || child.name.Contains("Preview")))
            {
                Debug.Log($"  Destruyendo hijo inmediatamente: {child.name}");
                DestroyImmediate(child.gameObject);
                totalDestroyed++;
            }
        }
        
        // Limpiar la lista de conexiones
        connections.Clear();
        startPoint = null;
        
        // Limpiar referencias en los puntos
        ConnectablePoint[] allPoints = GetComponentsInChildren<ConnectablePoint>(true);
        foreach (var point in allPoints)
        {
            if (point != null)
            {
                point.ClearConnection();
            }
        }
        
        Debug.Log($"[ConnectingController] ✓ Total destruido inmediatamente: {totalDestroyed} líneas");
    }

    void Update()
    {
    // iniciar arrastre
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"[ConnectingController] Click en posición mundo: {wp}");
            
            Collider2D col = Physics2D.OverlapPoint(wp, nodeLayerMask);
            
            if (col != null)
            {
                Debug.Log($"[ConnectingController] Detectado collider: {col.name}");
                var p = col.GetComponentInParent<ConnectablePoint>();
                if (p != null && p.CanAcceptConnection())
                {
                    Debug.Log($"[ConnectingController] Iniciando conexión desde: {p.GetId()}");
                    BeginDrag(p, wp);
                }
                else if (p != null && !p.CanAcceptConnection())
                {
                    Debug.Log($"[ConnectingController] El punto {p.GetId()} ya tiene una conexión");
                }
                else
                {
                    Debug.LogWarning($"[ConnectingController] El collider {col.name} no tiene ConnectablePoint");
                }
            }
            else
            {
                Debug.Log($"[ConnectingController] No se detectó ningún collider en la posición {wp}. Verifica el Layer Mask.");
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

            if (endPoint != null)
            {
                Debug.Log($"[ConnectingController] Soltado en: {endPoint.GetId()}");
                
                // Validación 1: No conectar consigo mismo
                if (endPoint == startPoint)
                {
                    Debug.Log("[ConnectingController] ❌ No puedes conectar un punto consigo mismo");
                    Destroy(previewLine.gameObject);
                }
                // Validación 2: Endpoint ya tiene conexión
                else if (!endPoint.CanAcceptConnection())
                {
                    Debug.Log($"[ConnectingController] ❌ El punto {endPoint.GetId()} ya tiene una conexión");
                    Destroy(previewLine.gameObject);
                }
                // Validación 3: Ya existe conexión entre estos puntos
                else if (ConnectionExists(startPoint, endPoint))
                {
                    Debug.Log($"[ConnectingController] ❌ Ya existe una conexión entre {startPoint.GetId()} y {endPoint.GetId()}");
                    Destroy(previewLine.gameObject);
                }
                // Validación 4: StartPoint tiene restricciones y NO permite este endpoint
                else if (!string.IsNullOrEmpty(startPoint.allowedTargetId) && !startPoint.AllowsConnectionTo(endPoint))
                {
                    Debug.Log($"[ConnectingController] ❌ {startPoint.GetId()} solo puede conectar con '{startPoint.allowedTargetId}', no con '{endPoint.GetId()}'");
                    Destroy(previewLine.gameObject);
                }
                // Validación 5: EndPoint tiene restricciones y NO permite este startpoint
                else if (!string.IsNullOrEmpty(endPoint.allowedTargetId) && !endPoint.AllowsConnectionTo(startPoint))
                {
                    Debug.Log($"[ConnectingController] ❌ {endPoint.GetId()} solo puede conectar con '{endPoint.allowedTargetId}', no con '{startPoint.GetId()}'");
                    Destroy(previewLine.gameObject);
                }
                // Validación 6: Todo OK - Crear conexión
                else
                {
                    Debug.Log($"[ConnectingController] ✓ Conexión válida: {startPoint.GetId()} <-> {endPoint.GetId()}");
                    FinalizeConnection(startPoint, endPoint);
                }
            }
            else
            {
                Debug.Log("[ConnectingController] Soltado fuera de un punto - conexión cancelada");
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
        
        // Verificar si se completaron todas las conexiones necesarias
        CheckCompletion();
    }
    
    void CheckCompletion()
    {
        if (requiredConnections <= 0) return; // No hay requisito de completar
        
        // Contar conexiones válidas
        int validConnections = 0;
        foreach (var conn in connections)
        {
            if (conn != null)
                validConnections++;
        }
        
        if (validConnections >= requiredConnections)
        {
            Debug.Log($"¡Minijuego completado! {validConnections}/{requiredConnections} conexiones");
            OnMiniGameComplete();
        }
    }
    
    void OnMiniGameComplete()
    {
        // Completar misión si está especificada
        if (!string.IsNullOrEmpty(missionToComplete) && manager != null)
        {
            manager.CompleteMission(missionToComplete);
            Debug.Log($"[ConnectingController] Misión completada: {missionToComplete}");
        }
        
        // Dar nueva misión si está especificada
        if (!string.IsNullOrEmpty(missionToGive) && manager != null)
        {
            manager.AddMission(missionToGive);
            Debug.Log($"[ConnectingController] Nueva misión dada: {missionToGive}");
        }
        
        // Cerrar el minijuego
        if (autoCloseOnComplete)
        {
            StartCoroutine(CloseAfterDelay(1f)); // Esperar 1 segundo antes de cerrar
        }
    }
    
    IEnumerator CloseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Buscar el trigger que abrió este minijuego y cerrarlo
        ConnectMiniGameTrigger trigger = FindObjectOfType<ConnectMiniGameTrigger>();
        if (trigger != null)
        {
            trigger.CloseMiniGame();
        }
        else
        {
            // Si no hay trigger, simplemente ocultar este gameObject
            gameObject.SetActive(false);
        }
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
