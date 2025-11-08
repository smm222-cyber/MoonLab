using System.Collections;
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

    private bool isTyping = false;
    private bool textComplete = false;

    void Start()
    {
        StartCoroutine(ShowEnding());
    }

    IEnumerator ShowEnding()
    {
        // Esperar un momento para que la escena cargue
        yield return new WaitForSeconds(0.5f);

        // Mostrar el texto con efecto de máquina de escribir
        yield return StartCoroutine(TypeText(textoFinal, guionText));
        textComplete = true;
        
        Debug.Log("✅ Texto completado - puedes hacer click para continuar");
    }

    void Update()
    {
        // Permitir saltar el texto con click
        if (skipTextWithClick && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Mostrar todo el texto inmediatamente
                StopAllCoroutines();
                ShowFullText();
            }
            else if (textComplete)
            {
                // Aquí puedes agregar lo que quieras hacer después
                // Por ejemplo, volver al menú principal o cargar otra escena
                Debug.Log("🎬 Click después de completar el texto - aquí puedes cargar otra escena");
                // Ejemplo: SceneManager.LoadScene("MainMenu");
            }
        }
    }

    void ShowFullText()
    {
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
}
