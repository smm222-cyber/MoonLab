using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectMiniGameTrigger : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private GameObject interactUI;
    
    [Header("Requisitos de Misión")]
    [Tooltip("Misión que debe estar activa para poder interactuar. Dejar vacío si no requiere misión.")]
    public string requiredMission;
    
    [Header("Canvas del Minijuego")]
    [Tooltip("El Canvas o GameObject que contiene el minijuego de conectar")]
    public GameObject connectMiniGameCanvas;
    
    [Header("Cámara")]
    [Tooltip("Script CameraFollow para desactivar el seguimiento durante el minijuego (opcional)")]
    public CameraFollow cameraFollow;
    
    [Tooltip("Posición fija donde mover la cámara al abrir el minijuego (opcional). Dejar en (0,0,0) si no quieres moverla")]
    public Vector3 cameraPositionDuringMinigame = Vector3.zero;
    
    [Tooltip("Mover la cámara a la posición especificada? Solo aplica si cameraPositionDuringMinigame no es (0,0,0)")]
    public bool moveCameraToPosition = false;
    
    [Header("Feedback")]
    [TextArea(2, 5)]
    public string messageWhenLocked = "Necesitas completar una misión primero";
    
    private GameManager manager;
    private Vector3 originalCameraPosition;

    void Start()
    {
        manager = GameManager.Instance;
        
        if (manager == null)
        {
            Debug.LogError($"GameManager no encontrado para {gameObject.name}");
        }
        
        // Asegurarse de que el canvas esté oculto al inicio
        if (connectMiniGameCanvas != null)
        {
            connectMiniGameCanvas.SetActive(false);
        }
    }

    public void ShowIndicator(bool state)
    {
        if (interactUI != null)
            interactUI.SetActive(state);
    }

    public void Interact()
    {
        if (manager == null) return;

        // Verificar si tiene la misión requerida
        bool canInteract = string.IsNullOrEmpty(requiredMission) || manager.HasMission(requiredMission);

        if (canInteract)
        {
            // Mostrar el minijuego
            if (connectMiniGameCanvas != null)
            {
                connectMiniGameCanvas.SetActive(true);
                
                // Desactivar movimiento del jugador mientras juega
                if (manager != null)
                {
                    manager.SetPlayerMovement(false);
                }
                
                // Desactivar seguimiento de cámara
                if (cameraFollow != null)
                {
                    cameraFollow.SetFollowing(false);
                }
                
                // Mover cámara a posición fija (opcional)
                if (moveCameraToPosition && cameraPositionDuringMinigame != Vector3.zero && Camera.main != null)
                {
                    originalCameraPosition = Camera.main.transform.position;
                    Camera.main.transform.position = new Vector3(
                        cameraPositionDuringMinigame.x, 
                        cameraPositionDuringMinigame.y, 
                        originalCameraPosition.z // Mantener la Z original
                    );
                }
                
                Debug.Log("Minijuego de conectar cables iniciado");
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: No se asignó el Canvas del minijuego");
            }
        }
        else
        {
            // No tiene la misión requerida
            if (!string.IsNullOrEmpty(messageWhenLocked))
            {
                manager.ShowNonCollectableText(messageWhenLocked);
            }
            Debug.Log($"Necesitas la misión '{requiredMission}' para interactuar");
        }
    }

    // Método para cerrar el minijuego (llamar desde ConnectingController cuando se complete)
    public void CloseMiniGame()
    {
        if (connectMiniGameCanvas != null)
        {
            connectMiniGameCanvas.SetActive(false);
        }
        
        // Reactivar movimiento del jugador
        if (manager != null)
        {
            manager.SetPlayerMovement(true);
        }
        
        // Reactivar seguimiento de cámara
        if (cameraFollow != null)
        {
            cameraFollow.SetFollowing(true);
        }
        
        // Restaurar posición original de cámara
        if (moveCameraToPosition && originalCameraPosition != Vector3.zero && Camera.main != null)
        {
            Camera.main.transform.position = originalCameraPosition;
        }
    }
}
