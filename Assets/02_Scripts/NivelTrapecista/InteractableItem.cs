using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    [Header("Item Configuration")]
    public string itemId;  // "BuscarHilo" or "BuscarAguja"
    public GameObject interactUI;
    public AudioClip pickupSound;

    private bool playerInRange = false;
    private GameManager manager;
    private AudioSource audioSource;

    void Start()
    {
        manager = GameManager.Instance;
        if (manager == null)
            Debug.LogError($"[{gameObject.name}] GameManager no encontrado!");

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && pickupSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && manager != null)
        {
            // Add mission completion
            manager.AddMission(itemId);
            
            // Play pickup sound
            if (audioSource != null && pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }

            // Hide UI and destroy object
            if (interactUI != null)
                interactUI.SetActive(false);
            
            Destroy(gameObject, pickupSound != null ? pickupSound.length : 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactUI != null)
                interactUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactUI != null)
                interactUI.SetActive(false);
        }
    }
}