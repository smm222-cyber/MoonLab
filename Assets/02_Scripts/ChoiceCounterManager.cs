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
                    if (!choiceCounters.ContainsKey(ic.key)) choiceCounters[ic.key] = 0;
                    choiceCounters[ic.key] += ic.count;
                }

                // Notificar al inicio si hay datos
                if (choiceCounters.Count > 0)
                    OnCountersChanged?.Invoke();
            }
            
            // Intentar cargar contadores previos guardados en PlayerPrefs
            LoadFromPlayerPrefs();
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
                    // sumar al manager persistente
                    int existing = Instance.GetChoiceCount(ic.key);
                    for (int i = 0; i < ic.count; i++)
                        Instance.IncrementChoice(ic.key);
                    Debug.Log($"[ChoiceCounterManager] Merge: añadidos {ic.count} a '{ic.key}' (antes {existing}, ahora {Instance.GetChoiceCount(ic.key)})");
                }
            }

            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        // Guardar contadores al salir (también se puede llamar manualmente desde UI o manager)
        SaveToPlayerPrefs();
    }

    /// <summary>
    /// Guarda todos los contadores en PlayerPrefs (serializados como JSON).
    /// </summary>
    public void SaveToPlayerPrefs()
    {
        try
        {
            var all = GetAllCounters();
            var list = new List<SerializableEntry>();
            foreach (var kv in all)
            {
                list.Add(new SerializableEntry() { key = kv.Key, value = kv.Value });
            }
            var wrapper = new SerializableWrapper() { entries = list.ToArray() };
            string json = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString("ChoiceCounters_v1", json);
            PlayerPrefs.Save();
            if (debugMode) Debug.Log("[ChoiceCounterManager] Contadores guardados en PlayerPrefs.");
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("[ChoiceCounterManager] Error guardando PlayerPrefs: " + ex.Message);
        }
    }

    /// <summary>
    /// Carga contadores desde PlayerPrefs si existen.
    /// </summary>
    public void LoadFromPlayerPrefs()
    {
        try
        {
            if (!PlayerPrefs.HasKey("ChoiceCounters_v1")) return;
            string json = PlayerPrefs.GetString("ChoiceCounters_v1");
            if (string.IsNullOrEmpty(json)) return;
            var wrapper = JsonUtility.FromJson<SerializableWrapper>(json);
            if (wrapper != null && wrapper.entries != null)
            {
                foreach (var e in wrapper.entries)
                {
                    if (string.IsNullOrEmpty(e.key)) continue;
                    choiceCounters[e.key] = e.value;
                }
                OnCountersChanged?.Invoke();
                if (debugMode) Debug.Log("[ChoiceCounterManager] Contadores cargados desde PlayerPrefs.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("[ChoiceCounterManager] Error cargando PlayerPrefs: " + ex.Message);
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
        if (debugMode) Debug.Log($"[ChoiceCounterManager] Invocando OnCountersChanged after '{choiceName}' -> {choiceCounters[choiceName]}");
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

    // Clases auxiliares para serializar el diccionario en PlayerPrefs
    [System.Serializable]
    private class SerializableEntry
    {
        public string key;
        public int value;
    }

    [System.Serializable]
    private class SerializableWrapper
    {
        public SerializableEntry[] entries;
    }
}
