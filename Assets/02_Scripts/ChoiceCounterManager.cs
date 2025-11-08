using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Administrador centralizado para contar cuántas veces el jugador elige cada opción de diálogo
/// </summary>
public class ChoiceCounterManager : MonoBehaviour
{
    // Singleton para acceso global
    public static ChoiceCounterManager Instance { get; private set; }
    
    // Diccionario para guardar los contadores: Key = nombre de la opción, Value = contador
    private Dictionary<string, int> choiceCounters = new Dictionary<string, int>();
    
    /// <summary>
    /// Evento que se dispara cuando cambian los contadores (para que la UI se actualice)
    /// </summary>
    public event Action OnCountersChanged;

    void Awake()
    {
        // Configurar singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Asegura que exista una instancia del manager en la escena. Si no existe,
    /// intenta instanciar un prefab llamado "ChoiceCounterManagerPrefab" desde Resources/
    /// o crea un GameObject vacío con este componente para evitar errores NRE.
    /// </summary>
    public static void EnsureExists()
    {
        if (Instance != null) return;

        // Intentar cargar prefab desde Resources
        var prefab = Resources.Load<GameObject>("ChoiceCounterManagerPrefab");
        if (prefab != null)
        {
            Instantiate(prefab);
            return;
        }

        // Si no existe prefab, crear un GameObject temporal con este componente
        Debug.LogWarning("[ChoiceCounterManager] Prefab no encontrado en Resources/. Creando instancia vacía para asegurar funcionamiento.");
        var go = new GameObject("ChoiceCounterManager_AutoCreated");
        go.AddComponent<ChoiceCounterManager>();
    }
    
    /// <summary>
    /// Incrementa el contador de una opción específica
    /// </summary>
    public void IncrementChoice(string choiceName)
    {
        if (string.IsNullOrEmpty(choiceName))
        {
            Debug.LogWarning("[ChoiceCounterManager] Nombre de opción vacío!");
            return;
        }
        
        if (!choiceCounters.ContainsKey(choiceName))
        {
            choiceCounters[choiceName] = 0;
        }
        
        choiceCounters[choiceName]++;

        Debug.Log($"📊 [ChoiceCounterManager] '{choiceName}' elegida {choiceCounters[choiceName]} vez/veces");

        // Notificar a listeners (UI, etc.)
        OnCountersChanged?.Invoke();
    }
    
    /// <summary>
    /// Obtiene cuántas veces se ha elegido una opción
    /// </summary>
    public int GetChoiceCount(string choiceName)
    {
        if (choiceCounters.ContainsKey(choiceName))
        {
            return choiceCounters[choiceName];
        }
        return 0;
    }
    
    /// <summary>
    /// Reinicia el contador de una opción específica
    /// </summary>
    public void ResetChoice(string choiceName)
    {
        if (choiceCounters.ContainsKey(choiceName))
        {
            choiceCounters[choiceName] = 0;
            Debug.Log($"🔄 [ChoiceCounterManager] Contador de '{choiceName}' reiniciado");
        }
    }
    
    /// <summary>
    /// Reinicia todos los contadores
    /// </summary>
    public void ResetAllCounters()
    {
        choiceCounters.Clear();
        Debug.Log("🔄 [ChoiceCounterManager] Todos los contadores reiniciados");
    }
    
    /// <summary>
    /// Muestra todos los contadores en la consola (para debug)
    /// </summary>
    [ContextMenu("Mostrar Todos los Contadores")]
    public void ShowAllCounters()
    {
        Debug.Log("=== 📊 CONTADORES DE OPCIONES ===");
        
        if (choiceCounters.Count == 0)
        {
            Debug.Log("No hay opciones registradas aún.");
            return;
        }
        
        foreach (var kvp in choiceCounters)
        {
            Debug.Log($"  '{kvp.Key}': {kvp.Value} veces");
        }
    }
    
    /// <summary>
    /// Obtiene todos los contadores como diccionario (útil para guardar datos)
    /// </summary>
    public Dictionary<string, int> GetAllCounters()
    {
        return new Dictionary<string, int>(choiceCounters);
    }
}
