using UnityEngine;

public class SewingCanvasController : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject canvasCostura;      // Asignar Canvas_Costura
    public ThreadDrawer threadDrawer;     // Asignar ThreadDrawer
    public KeyCode openCanvasKey = KeyCode.O;
    public KeyCode startSewingKey = KeyCode.L;

    private bool canvasActivo = false;
    private int lPressCount = 0; // Contador de veces que se presiona L

    void Start()
    {
        // Asegurarse de que el canvas y el ThreadDrawer estén desactivados al inicio
        canvasCostura.SetActive(false);
        threadDrawer.enabled = false;
    }

    void Update()
    {
        // Abrir canvas con O
        if (Input.GetKeyDown(openCanvasKey) && !canvasActivo)
        {
            canvasCostura.SetActive(true);
            canvasActivo = true;
            lPressCount = 0;
            Debug.Log("Canvas de costura abierto. Presiona L dos veces para coser.");
        }

        // Contar L para activar la costura
        if (canvasActivo && Input.GetKeyDown(startSewingKey))
        {
            lPressCount++;

            if (lPressCount == 2)
            {
                threadDrawer.enabled = true; // Activar el ThreadDrawer
                Debug.Log("Costura activa. Haz click para coser.");
            }
            else
            {
                Debug.Log("Presiona L una vez más para comenzar a coser.");
            }
        }
    }

    public void TerminarCostura()
    {
        // Ocultar canvas y resetear estado
        canvasCostura.SetActive(false);
        canvasActivo = false;
        lPressCount = 0;

        // Desactivar ThreadDrawer
        threadDrawer.enabled = false;
  
        Debug.Log("Costura terminada.");
    }
}
