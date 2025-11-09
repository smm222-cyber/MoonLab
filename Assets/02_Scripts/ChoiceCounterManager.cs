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
    
    // Diccionario para guardar los contadores: Key = normalized nombre de la opción (trim + lower), Value = contador
    private Dictionary<string, int> choiceCounters = new Dictionary<string, int>();

    // Normaliza claves para evitar discrepancias por mayúsculas/espacios
    private string NormalizeKey(string key)
    {
        return string.IsNullOrEmpty(key) ? "" : key.Trim().ToLowerInvariant();
    }
    
    [System.Serializable]
    public struct InitialCounter
    {
        public string key;
        public int count;
    }

    [Header("Optional: initial counters for this scene (will be merged)")]
    [Tooltip("Valores opcionales que este manager añadirá al manager persistente cuando se cargue la escena. Útil si quieres que cada escena aporte valores iniciales.")]
    public InitialCounter[] initialCounters;
    
    /// <summary>
    /// Evento que se dispara cuando cambian los contadores (para que la UI se actualice)
    /// </summary>
    public event Action OnCountersChanged;

    [Header("Debug")]
    [Tooltip("Activar para ver logs adicionales del ChoiceCounterManager en la consola")]
    public bool debugMode = false;

    void Awake()
    {
        // Configurar singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
            Debug.Log("[ChoiceCounterManager] Instancia creada y marcada DontDestroyOnLoad");

            // Si este manager tiene counters iniciales definidos en el inspector, aplicarlos
            if (initialCounters != null && initialCounters.Length > 0)
            {
                foreach (var ic in initialCounters)
                {
                    if (string.IsNullOrEmpty(ic.key)) continue;
                    var nkey = NormalizeKey(ic.key);
                    if (!choiceCounters.ContainsKey(nkey)) choiceCounters[nkey] = 0;
                    choiceCounters[nkey] += ic.count;
                }

                // Notificar al inicio si hay datos
                if (choiceCounters.Count > 0)
                    OnCountersChanged?.Invoke();
            }
        }
        else
        {
            Debug.Log("[ChoiceCounterManager] Instancia duplicada encontrada - fusionando (merge) contadores e destruyendo objeto adicional");

            // Si el manager duplicado tiene initialCounters definidos, fusionarlos en la instancia existente
            if (initialCounters != null && initialCounters.Length > 0)
            {
                foreach (var ic in initialCounters)
                {
                    if (string.IsNullOrEmpty(ic.key)) continue;
                    // sumar al manager persistente (usar claves normalizadas)
                    int existing = Instance.GetChoiceCount(ic.key);
                    for (int i = 0; i < ic.count; i++)
                        Instance.IncrementChoice(ic.key);
                    Debug.Log($"[ChoiceCounterManager] Merge: añadidos {ic.count} a '{ic.key}' (antes {existing}, ahora {Instance.GetChoiceCount(ic.key)})");
                }
            }

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

        var nkey = NormalizeKey(choiceName);
        if (!choiceCounters.ContainsKey(nkey))
        {
            choiceCounters[nkey] = 0;
        }

        choiceCounters[nkey]++;

        Debug.Log($"📊 [ChoiceCounterManager] '{nkey}' elegida {choiceCounters[nkey]} vez/veces (input: '{choiceName}')");

    // Notificar a listeners (UI, etc.)
        if (debugMode) Debug.Log($"[ChoiceCounterManager] Invocando OnCountersChanged after '{nkey}' -> {choiceCounters[nkey]}");
        OnCountersChanged?.Invoke();

        // Si está en modo debug, mostrar resumen de todos los contadores en la consola
        if (debugMode)
        {
            ShowAllCounters();
        }
    }
    
    /// <summary>
    /// Obtiene cuántas veces se ha elegido una opción
    /// </summary>
    public int GetChoiceCount(string choiceName)
    {
        var nkey = NormalizeKey(choiceName);
        if (choiceCounters.ContainsKey(nkey))
        {
            return choiceCounters[nkey];
        }
        return 0;
    }
    
    /// <summary>
    /// Reinicia el contador de una opción específica
    /// </summary>
    public void ResetChoice(string choiceName)
    {
        var nkey = NormalizeKey(choiceName);
        if (choiceCounters.ContainsKey(nkey))
        {
            choiceCounters[nkey] = 0;
            Debug.Log($"🔄 [ChoiceCounterManager] Contador de '{nkey}' reiniciado (input: '{choiceName}')");
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

    void Update()
    {
        // Atajo de depuración: mostrar contadores con F1 si debugMode está activo
        if (debugMode && Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("[ChoiceCounterManager] Debug hotkey F1 pulsada - mostrando contadores:");
            ShowAllCounters();
        }
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
