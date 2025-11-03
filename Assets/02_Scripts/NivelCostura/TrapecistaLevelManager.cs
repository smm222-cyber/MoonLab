using UnityEngine;

public class TrapecistaLevelManager : MonoBehaviour
{
    [Header("Costura")]
    public GameObject canvasCostura;
    public ThreadDrawer threadDrawer;

    public KeyCode openCanvasKey = KeyCode.O;
    public KeyCode startSewingKey = KeyCode.L;

    private bool canvasActivo = false;
    private bool listoParaCoser = false;

    void Start()
    {
        // Asegurarse de que el canvas esté oculto y ThreadDrawer desactivado
        if (canvasCostura != null)
            canvasCostura.SetActive(false);

        if (threadDrawer != null)
            threadDrawer.enabled = false;
    }

    void Update()
    {
        // Abrir canvas con O
        if (Input.GetKeyDown(openCanvasKey) && !canvasActivo)
        {
            if (canvasCostura != null)
                canvasCostura.SetActive(true);

            canvasActivo = true;
            listoParaCoser = false;
        }

        // Empezar a coser con L
        if (Input.GetKeyDown(startSewingKey) && canvasActivo && !listoParaCoser)
        {
            if (threadDrawer != null)
                threadDrawer.enabled = true;

            listoParaCoser = true;
        }
    }

    // Llamar desde botón "Terminar Costura"
    public void TerminarCostura()
    {
        if (canvasCostura != null)
            canvasCostura.SetActive(false);

        canvasActivo = false;
        listoParaCoser = false;

        if (threadDrawer != null)
        {
            threadDrawer.enabled = false;
            threadDrawer.ClearLines();
        }
    }
}
