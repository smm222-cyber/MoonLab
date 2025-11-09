using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla la escena de final del juego mostrando diferentes finales según las elecciones del jugador
/// </summary>
public class EndingSceneController : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("Panel negro de fondo")]
    public GameObject backgroundPanel;
    
    [Tooltip("Texto principal del guion")]
    public TextMeshProUGUI guionText;
    
    [Header("Configuración de Texto")]
    [Tooltip("Velocidad de escritura del texto (caracteres por segundo)")]
    public float typingSpeed = 30f;
    
    [Tooltip("Tiempo de espera después de mostrar el texto")]
    public float waitTimeAfterText = 3f;
    
    [Header("Guiones de Finales")]
    [TextArea(5, 15)]
    [Tooltip("Final si eligió más veces 'Saber sobre el circo'")]
    public string endingCirco = 
        "Tu curiosidad por conocer a los demás del circo te ha llevado a crear lazos increíbles.\n\n" +
        "Cada personaje te confió sus historias, sus sueños y sus miedos.\n\n" +
        "Ahora eres parte importante de esta gran familia circense.\n\n" +
        "El circo no es solo un espectáculo... es un hogar.";
    
    [TextArea(5, 15)]
    [Tooltip("Final si eligió más veces 'Saber sobre mí'")]
    public string endingYo = 
        "Al compartir tu historia con todos, encontraste tu lugar en el circo.\n\n" +
        "Cada personaje te escuchó, te comprendió y te ayudó a crecer.\n\n" +
        "Descubriste que al abrirte a los demás, también te conoces mejor a ti mismo.\n\n" +
        "Esta es tu historia... y apenas comienza.";
    
    [Header("Opciones")]
    [Tooltip("Nombre de la escena a la que ir después (vacío = cerrar juego)")]
    public string nextSceneName = "";
    
    [Tooltip("Tecla para continuar/salir")]
    public KeyCode continueKey = KeyCode.Space;
    
    private bool textFinished = false;
    private bool canContinue = false;
    
    void Start()
    {
        // Asegurarse de que el panel esté visible
        if (backgroundPanel != null)
        {
            backgroundPanel.SetActive(true);
        }
        
        // Determinar qué final mostrar
        StartCoroutine(ShowEnding());
    }
    
    void Update()
    {
        // Permitir saltar el texto o continuar
        if (Input.GetKeyDown(continueKey))
        {
            if (!textFinished)
            {
                // Saltar animación de texto
                StopAllCoroutines();
                textFinished = true;
                ShowFullText();
            }
            else if (canContinue)
            {
                // Continuar a siguiente escena o salir
                ContinueOrExit();
            }
        }
    }
    
    IEnumerator ShowEnding()
    {
        // Obtener contadores
        int circoCount = 0;
        int yoCount = 0;
        
        if (ChoiceCounterManager.Instance != null)
        {
            circoCount = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreElCirco");
            yoCount = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreMi");
        }
        
        // Determinar qué final mostrar
        string finalText = "";
        
        if (circoCount > yoCount)
        {
            finalText = endingCirco;
            Debug.Log("[EndingScene] Final: Más interesado en el circo");
        }
        else
        {
            // Si hay empate o yoCount es mayor, mostrar final "Yo"
            finalText = endingYo;
            Debug.Log("[EndingScene] Final: Más interesado en compartir sobre ti");
        }
        
        // Animar texto letra por letra
        yield return StartCoroutine(TypeText(finalText));
        
        textFinished = true;
        
        // Esperar un tiempo
        yield return new WaitForSeconds(waitTimeAfterText);
        
        // Mostrar mensaje de continuar
        canContinue = true;
        
        if (guionText != null)
        {
            guionText.text += $"\n\n<color=grey>[Presiona {continueKey} para continuar]</color>";
        }
    }
    
    IEnumerator TypeText(string text)
    {
        if (guionText == null)
        {
            Debug.LogError("[EndingScene] guionText no está asignado!");
            yield break;
        }
        
        guionText.text = "";
        
        float delay = 1f / typingSpeed;
        
        foreach (char letter in text.ToCharArray())
        {
            guionText.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }
    
    void ShowFullText()
    {
        if (guionText == null) return;
        
        // Determinar qué texto mostrar completo
        int circoCount = 0;
        int yoCount = 0;
        
        if (ChoiceCounterManager.Instance != null)
        {
            circoCount = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreElCirco");
            yoCount = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreMi");
        }
        
        string finalText = "";
        if (circoCount > yoCount)
            finalText = endingCirco;
        else
            finalText = endingYo;
        
        guionText.text = finalText;
        canContinue = true;
        guionText.text += $"\n\n<color=grey>[Presiona {continueKey} para continuar]</color>";
    }
    
    void ContinueOrExit()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            // Ir a la siguiente escena
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // Salir del juego
            Debug.Log("[EndingScene] Saliendo del juego...");
            Application.Quit();
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
    
    /// <summary>
    /// Método público para cargar esta escena desde otro script
    /// </summary>
    public static void LoadEndingScene(string sceneName = "EndingScene")
    {
        SceneManager.LoadScene(sceneName);
    }
}
