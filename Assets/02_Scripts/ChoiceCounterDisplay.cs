using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Muestra los contadores de opciones en pantalla
/// </summary>
public class ChoiceCounterDisplay : MonoBehaviour
{
    [Header("Referencias UI")]
    [Tooltip("Panel que contendrá los contadores")]
    public GameObject counterPanel;
    
    [Tooltip("Texto donde se mostrarán los contadores (TextMeshPro)")]
    public TextMeshProUGUI counterText;
    
    [Header("Configuración")]
    [Tooltip("Actualizar automáticamente cada X segundos")]
    public float updateInterval = 1f;
    
    [Tooltip("Mostrar solo contadores mayores a 0")]
    public bool hideZeroCounters = true;
    
    [Tooltip("Tecla para mostrar/ocultar el panel (opcional)")]
    public KeyCode toggleKey = KeyCode.Tab;
    
    [Tooltip("Iniciar con el panel visible")]
    public bool startVisible = true;
    
    private float updateTimer;
    
    void Start()
    {
        if (counterPanel != null)
        {
            counterPanel.SetActive(startVisible);
        }
        
        UpdateDisplay();
    }
    
    void Update()
    {
        // Toggle del panel con tecla
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePanel();
        }
        
        // Actualización automática
        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;
            UpdateDisplay();
        }
    }
    
    /// <summary>
    /// Actualiza el texto con los contadores actuales
    /// </summary>
    public void UpdateDisplay()
    {
        if (counterText == null || ChoiceCounterManager.Instance == null)
            return;
        
        Dictionary<string, int> counters = ChoiceCounterManager.Instance.GetAllCounters();
        
        if (counters.Count == 0)
        {
            counterText.text = "Sin opciones elegidas aún...";
            return;
        }
        
        string displayText = "📊 <b>CONTADORES GLOBALES</b>\n\n";
        
        foreach (var kvp in counters)
        {
            // Saltar si está en 0 y hideZeroCounters está activo
            if (hideZeroCounters && kvp.Value == 0)
                continue;
            
            // Nombres personalizados amigables
            string displayName = kvp.Key;
            
            switch (kvp.Key)
            {
                case "SaberSobreElCirco":
                    displayName = "Saber sobre el circo";
                    break;
                case "SaberSobreMi":
                    displayName = "Saber sobre mí";
                    break;
                default:
                    // Para opciones adicionales que no estén definidas
                    displayName = kvp.Key.Replace("_", " ");
                    break;
            }
            
            displayText += $"• {displayName}: <color=yellow><b>{kvp.Value}</b></color> veces\n";
        }
        
        counterText.text = displayText;
    }
    
    /// <summary>
    /// Muestra u oculta el panel
    /// </summary>
    public void TogglePanel()
    {
        if (counterPanel != null)
        {
            counterPanel.SetActive(!counterPanel.activeSelf);
            
            // Actualizar al mostrar
            if (counterPanel.activeSelf)
            {
                UpdateDisplay();
            }
        }
    }
    
    /// <summary>
    /// Muestra el panel
    /// </summary>
    public void ShowPanel()
    {
        if (counterPanel != null)
        {
            counterPanel.SetActive(true);
            UpdateDisplay();
        }
    }
    
    /// <summary>
    /// Oculta el panel
    /// </summary>
    public void HidePanel()
    {
        if (counterPanel != null)
        {
            counterPanel.SetActive(false);
        }
    }
}
