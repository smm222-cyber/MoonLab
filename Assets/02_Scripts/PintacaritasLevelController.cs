using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlador opcional para el nivel de Pintacaritas.
/// Te ayuda a debuggear y ver el estado de las misiones en tiempo real.
/// </summary>
public class PintacaritasLevelController : MonoBehaviour
{
    [Header("Referencias")]
    public NPCGroupConversation grupoPintacaritas1Maestro;
    public NPCBasicDialog trapecista;
    public PintacaritasNPC pintacaritas2;

    [Header("Debug - Solo para ver el estado")]
    [SerializeField] private bool mostrarDebugInfo = true;
    [SerializeField] private bool misionPintacaritas1Activa;
    [SerializeField] private bool misionTrapecistaActiva;

    private GameManager manager;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        
        if (manager == null)
        {
            Debug.LogError("❌ No se encontró GameManager en la escena!");
        }
        
        ValidarConfiguracion();
    }

    void Update()
    {
        if (mostrarDebugInfo && manager != null)
        {
            ActualizarDebugInfo();
        }
    }

    // Actualiza las variables de debug en el inspector
    void ActualizarDebugInfo()
    {
        misionPintacaritas1Activa = manager.HasMission("HablasteCon_Pintacaritas1");
        misionTrapecistaActiva = manager.HasMission("HablasteCon_Trapecista");
    }

    // Valida que todo esté configurado correctamente
    void ValidarConfiguracion()
    {
        Debug.Log("=== VALIDACIÓN DEL NIVEL PINTACARITAS ===");
        
        if (grupoPintacaritas1Maestro != null)
        {
            Debug.Log("✅ Grupo Pintacaritas1 + Maestro configurado");
            ValidarNPCGroup();
        }
        else
        {
            Debug.LogWarning("⚠️ Falta asignar: Grupo Pintacaritas1 + Maestro");
        }

        if (trapecista != null)
        {
            Debug.Log("✅ Trapecista configurada");
        }
        else
        {
            Debug.LogWarning("⚠️ Falta asignar: Trapecista");
        }

        if (pintacaritas2 != null)
        {
            Debug.Log("✅ Pintacaritas 2 configurada");
        }
        else
        {
            Debug.LogWarning("⚠️ Falta asignar: Pintacaritas 2");
        }

        Debug.Log("========================================");
    }

    void ValidarNPCGroup()
    {
        if (grupoPintacaritas1Maestro.initialConversation.Count == 0)
        {
            Debug.LogWarning("⚠️ El grupo no tiene conversación inicial configurada");
        }

        if (string.IsNullOrEmpty(grupoPintacaritas1Maestro.missionRequired))
        {
            Debug.LogWarning("⚠️ El grupo no tiene 'Mission Required' configurado para la conversación alternativa");
        }
    }

    // Métodos útiles para testing (puedes llamarlos desde otros scripts o botones)
    
    [ContextMenu("Resetear Misiones")]
    public void ResetearMisiones()
    {
        if (manager != null)
        {
            manager.CompleteMission("HablasteCon_Pintacaritas1");
            manager.CompleteMission("HablasteCon_Trapecista");
            Debug.Log("🔄 Misiones reseteadas");
        }
    }

    [ContextMenu("Completar Misión Pintacaritas1")]
    public void CompletarMisionPintacaritas1()
    {
        if (manager != null)
        {
            manager.CompleteMission("HablasteCon_Pintacaritas1");
            Debug.Log("✅ Misión 'HablasteCon_Pintacaritas1' completada");
        }
    }

    [ContextMenu("Completar Misión Trapecista")]
    public void CompletarMisionTrapecista()
    {
        if (manager != null)
        {
            manager.CompleteMission("HablasteCon_Trapecista");
            Debug.Log("✅ Misión 'HablasteCon_Trapecista' completada");
        }
    }

    [ContextMenu("Ver Estado Actual")]
    public void MostrarEstadoActual()
    {
        if (manager != null)
        {
            Debug.Log("=== ESTADO ACTUAL ===");
            Debug.Log($"HablasteCon_Pintacaritas1: {(manager.HasMission("HablasteCon_Pintacaritas1") ? "ACTIVA" : "COMPLETADA/NO INICIADA")}");
            Debug.Log($"HablasteCon_Trapecista: {(manager.HasMission("HablasteCon_Trapecista") ? "ACTIVA" : "COMPLETADA/NO INICIADA")}");
            Debug.Log("====================");
        }
    }
}
