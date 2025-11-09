using UnityEngine;
using TMPro;

/// <summary>
/// Controlador para el panel que muestra los contadores globales.
/// Coloca este script en el GameObject del CounterPanel y asigna los campos en el Inspector.
/// El panel se suscribirá al ChoiceCounterManager y actualizará los textos automáticamente.
/// </summary>
public class CounterPanelController : MonoBehaviour
{
    [Header("Text Elements")]
    public TextMeshProUGUI circoText;
    public TextMeshProUGUI yoText;
    [Tooltip("Si solo tienes un TextMeshProUGUI para mostrar ambos contadores, arrástralo aquí.")]
    public TextMeshProUGUI combinedText;
    [Tooltip("Si está activado, mostrará también el porcentaje de cada opción (útil en un solo Text)")]
    public bool showPercentages = false;

    void OnEnable()
    {
        // Asegurar que el manager exista
        ChoiceCounterManager.EnsureExists();

        if (ChoiceCounterManager.Instance != null)
        {
            ChoiceCounterManager.Instance.OnCountersChanged += UpdateUI;
            UpdateUI(); // refrescar al activarse
        }
    }

    void OnDisable()
    {
        if (ChoiceCounterManager.Instance != null)
        {
            ChoiceCounterManager.Instance.OnCountersChanged -= UpdateUI;
        }
    }

    public void UpdateUI()
    {
        int circo = 0;
        int yo = 0;

        if (ChoiceCounterManager.Instance == null)
        {
            // No hay manager aún: mostrar ceros
            if (combinedText != null)
                combinedText.text = "Circo: 0   |   Yo: 0";
            else
            {
                if (circoText != null) circoText.text = "0";
                if (yoText != null) yoText.text = "0";
            }
            return;
        }

        circo = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreElCirco");
        yo = ChoiceCounterManager.Instance.GetChoiceCount("SaberSobreMi");

        if (combinedText != null)
        {
            if (showPercentages)
            {
                int total = Mathf.Max(1, circo + yo);
                float pCirco = (circo / (float)total) * 100f;
                float pYo = (yo / (float)total) * 100f;
                combinedText.text = $"Circo: {circo} ({pCirco:0.#}%)   |   Yo: {yo} ({pYo:0.#}%)";
            }
            else
            {
                combinedText.text = $"Circo: {circo}   |   Yo: {yo}";
            }
        }
        else
        {
            if (circoText != null) circoText.text = circo.ToString();
            if (yoText != null) yoText.text = yo.ToString();
        }
    }

    // Método de prueba público que puedes enlazar a botones para incrementar (debug)
    public void DebugIncrementCirco() => ChoiceCounterManager.Instance?.IncrementChoice("SaberSobreElCirco");
    public void DebugIncrementYo() => ChoiceCounterManager.Instance?.IncrementChoice("SaberSobreMi");
}
