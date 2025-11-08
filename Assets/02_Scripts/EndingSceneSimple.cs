using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

/// <summary>
/// Versión simple del controlador de escena final - solo muestra texto bonito sin usar contadores
/// </summary>
public class EndingSceneSimple : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Texto principal que se mostrará")]
    public TextMeshProUGUI guionText;

    [Header("Ending Text")]
    [TextArea(5, 15)]
    [Tooltip("El texto que quieres mostrar en la escena final")]
    public string textoFinal = "Este es el texto del final de tu historia...\n\nAquí puedes escribir lo que quieras que aparezca.\n\nEl efecto de escritura hace que se vea más bonito y cinematográfico.\n\nPuedes usar \\n para saltos de línea.";

    [Header("Settings")]
    [Tooltip("Velocidad de escritura (más bajo = más lento)")]
    public float textSpeed = 0.05f;
    
    [Tooltip("Permite saltar el efecto de escritura haciendo click")]
    public bool skipTextWithClick = true;

    [Header("Líneas e Imágenes")]
    [Tooltip("Si está activado, usa las líneas definidas manualmente en el inspector en vez de dividir 'textoFinal'")]
    public bool useManualLines = false;

    [Tooltip("Lista de líneas (cada elemento será mostrado como una 'línea' independiente que espera avanzar)")]
    public List<string> manualLines = new List<string>();

    [Tooltip("Imagen UI donde se mostrará la imagen asociada a cada línea ")]
    public Image imageDisplay;

    [Tooltip("Lista de sprites, uno por línea. Si hay menos sprites que líneas, las restantes no mostrarán imagen")]
    public List<Sprite> lineImages = new List<Sprite>();

    private bool isTyping = false;
    private bool textComplete = false;
    // Control de avance por línea
    private bool waitingForAdvance = false;
    private bool advanceRequested = false;

    void Start()
    {
        StartCoroutine(ShowEnding());
    }

    IEnumerator ShowEnding()
    {
        // Esperar un momento para que la escena cargue
        yield return new WaitForSeconds(0.5f);

        // Construir las líneas a mostrar (manual o a partir de textoFinal)
        string[] lines;
        if (useManualLines && manualLines != null && manualLines.Count > 0)
        {
            lines = manualLines.ToArray();
        }
        else
        {
            // Párrafos separados por doble salto de línea si existen
            string normalized = textoFinal.Replace("\r\n", "\n").Replace("\r", "\n");
            if (normalized.Contains("\n\n"))
                lines = normalized.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);
            else
                lines = normalized.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        }

        // Mostrar línea por línea, esperando click entre cada una
        yield return StartCoroutine(TypeTextByLine(lines));

        textComplete = true;
        Debug.Log("✅ Todas las líneas completadas - puedes hacer click para continuar");
    }

    void Update()
    {
        // No permite saltar mientras se está tipeando
        if (skipTextWithClick && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                
                return;
            }

            if (waitingForAdvance)
            {
                // Marcar solicitud de avance, el coroutine la consumirá
                advanceRequested = true;
                return;
            }

            if (textComplete)
            {
                Debug.Log("🎬 Click después de completar todo el texto - aquí puedes cargar otra escena");
                // Ejemplo: SceneManager.LoadScene("MainMenu");
            }
        }
    }

    void ShowFullText()
    {
        // No se usa en esta versió
        isTyping = false;
        textComplete = true;
        guionText.text = textoFinal;
        Debug.Log("⏭️ Texto mostrado completo (saltado)");
    }
    IEnumerator TypeText(string text, TextMeshProUGUI textComponent)
    {
        if (textComponent == null)
        {
            Debug.LogError("❌ GuionText no está asignado en el Inspector!");
            yield break;
        }

        isTyping = true;
        textComponent.text = "";

        foreach (char c in text)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    // Escribe varias líneas una por una, mostrando la imagen asociada si existe.
    IEnumerator TypeTextByLine(string[] lines)
    {
        if (guionText == null)
        {
            Debug.LogError("❌ GuionText no está asignado en el Inspector!");
            yield break;
        }

        waitingForAdvance = false;
        advanceRequested = false;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i] ?? string.Empty;

            // Mostrar imagen si hay una para esta línea
            if (imageDisplay != null && lineImages != null && lineImages.Count > i && lineImages[i] != null)
            {
                imageDisplay.sprite = lineImages[i];
                imageDisplay.enabled = true;
            }
            else if (imageDisplay != null)
            {
                imageDisplay.enabled = false;
            }

            // Escribir la línea carácter a carácter
            isTyping = true;
            guionText.text = string.Empty;
            foreach (char c in line)
            {
                guionText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
            isTyping = false;

            // Esperar click del usuario para avanzar a la siguiente línea
            waitingForAdvance = true;
            // (Opcional) podrías mostrar aquí un indicador visual para 'click to continue'
            yield return new WaitUntil(() => advanceRequested == true);

            // Reset y continuar
            advanceRequested = false;
            waitingForAdvance = false;
        }
    }
}
