using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlador opcional para el nivel del Trapecista.
/// Te ayuda a debuggear y ver el estado de las misiones en tiempo real.
/// </summary>
public class TrapecistaLevelController : MonoBehaviour
{
    [Header("Referencias")]
    public TrapecistaGroupConversation trapecistaGrupo;
    public NPCBasicDialog pintacaritas1;
    public PintacaritasNPC pintacaritas2;

    [Header("Debug - Solo para ver el estado")]
    [SerializeField] private bool mostrarDebugInfo = true;
    [SerializeField] private bool misionTrapecistaActiva;
    [SerializeField] private bool misionPintacaritas1Activa;

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

    void ActualizarDebugInfo()
    {
        misionTrapecistaActiva = manager.HasMission("HablasteCon_Trapecista");
        misionPintacaritas1Activa = manager.HasMission("HablasteCon_Pintacaritas1");
    }

    void ValidarConfiguracion()
    {
        Debug.Log("=== VALIDACIÓN DEL NIVEL TRAPECISTA ===");

        if (trapecistaGrupo != null)
        {
            Debug.Log("✅ Trapecista configurado");
            ValidarNPCGroup();
        }
        else
        {
            Debug.LogWarning("⚠️ Falta asignar: Trapecista");
        }

        if (pintacaritas1 != null)
        {
            Debug.Log("✅ Pintacaritas 1 configurada");
        }
        else
        {
            Debug.LogWarning("⚠️ Falta asignar: Pintacaritas 1");
        }

        if (pintacaritas2 != null)
        {
            Debug.Log("✅ Pintacaritas 2 configurada");
        }
        else
        {
            Debug.LogWarning("⚠️ Falta asignar: Pintacaritas 2");
        }

        Debug.Log("=======================================");
    }

    void ValidarNPCGroup()
    {
        if (trapecistaGrupo.initialConversation.Count == 0)
        {
            Debug.LogWarning("⚠️ Trapecista no tiene conversación inicial configurada");
        }

        if (string.IsNullOrEmpty(trapecistaGrupo.missionRequired))
        {
            Debug.LogWarning("⚠️ Trapecista no tiene 'Mission Required' configurado para la conversación alternativa");
        }
    }

    // Métodos útiles para testing

    [ContextMenu("Resetear Misiones")]
    public void ResetearMisiones()
    {
        if (manager != null)
        {
            manager.CompleteMission("HablasteCon_Trapecista");
            manager.CompleteMission("HablasteCon_Pintacaritas1");
            Debug.Log("🔄 Misiones reseteadas");
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

    [ContextMenu("Completar Misión Pintacaritas1")]
    public void CompletarMisionPintacaritas1()
    {
        if (manager != null)
        {
            manager.CompleteMission("HablasteCon_Pintacaritas1");
            Debug.Log("✅ Misión 'HablasteCon_Pintacaritas1' completada");
        }
    }

    [ContextMenu("Ver Estado Actual")]
    public void MostrarEstadoActual()
    {
        if (manager != null)
        {
            Debug.Log("=== ESTADO ACTUAL ===");
            Debug.Log($"HablasteCon_Trapecista: {(manager.HasMission("HablasteCon_Trapecista") ? "ACTIVA" : "COMPLETADA/NO INICIADA")}");
            Debug.Log($"HablasteCon_Pintacaritas1: {(manager.HasMission("HablasteCon_Pintacaritas1") ? "ACTIVA" : "COMPLETADA/NO INICIADA")}");
            Debug.Log("====================");
        }
    }
}
