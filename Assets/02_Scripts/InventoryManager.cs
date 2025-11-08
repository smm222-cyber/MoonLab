using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; // Para acceder desde otros scripts

    public Transform inventoryPanel; // Donde se mostrar�n los �conos
    public GameObject inventorySlotPrefab; // El prefab del �tem visual

    [Header("Configuración de inventario")]
    [Tooltip("Número máximo de slots visibles en el inventario (fijo)")]
    public int maxSlots = 3;

    private List<Sprite> collectedItems = new List<Sprite>(); // Lista interna
    private List<Image> slotImages = new List<Image>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Inicializar panel como invisble (el jugador lo abre con G)
        if (inventoryPanel != null)
            inventoryPanel.gameObject.SetActive(false);

        // Pre-crear slots fijos para mantener orden visual
        if (inventoryPanel != null && inventorySlotPrefab != null)
        {
            // Limpiar hijos existentes (por si acaso)
            for (int i = inventoryPanel.childCount - 1; i >= 0; i--)
            {
                Destroy(inventoryPanel.GetChild(i).gameObject);
            }

            slotImages.Clear();
            for (int i = 0; i < maxSlots; i++)
            {
                GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
                slot.name = $"InventorySlot_{i}";
                Image img = slot.GetComponent<Image>();
                if (img != null)
                {
                    img.enabled = false; // ocultar hasta que tenga item
                    slotImages.Add(img);
                }
                else
                {
                    // Buscar imagen en hijos
                    Image childImg = slot.GetComponentInChildren<Image>();
                    if (childImg != null)
                    {
                        childImg.enabled = false;
                        slotImages.Add(childImg);
                    }
                    else
                    {
                        slotImages.Add(null);
                    }
                }
            }
        }
    }

    void Update()
    {
        // Toggle simple del panel con la tecla G
        if (Input.GetKeyDown(KeyCode.G) && inventoryPanel != null)
        {
            inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);
        }
    }

    public void AddItem(Sprite itemIcon)
    {
        if (itemIcon == null) return;

        // Si está lleno, avisar y no añadir
        if (collectedItems.Count >= maxSlots)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ShowNonCollectableText("Inventario lleno. No puedes recoger más objetos.");
            Debug.LogWarning("InventoryManager: intento de añadir item pero el inventario está lleno.");
            return;
        }

        collectedItems.Add(itemIcon);

        // Actualizar el primer slot vacío visualmente
        for (int i = 0; i < slotImages.Count; i++)
        {
            if (slotImages[i] == null) continue;
            if (!slotImages[i].enabled)
            {
                slotImages[i].sprite = itemIcon;
                slotImages[i].enabled = true;
                break;
            }
        }

        // Asegurar panel visible si se añadió el primer item
        if (inventoryPanel != null && !inventoryPanel.gameObject.activeSelf)
            inventoryPanel.gameObject.SetActive(true);
    }




    //a�adi esto para guardar los items recolectados
    public List<string> GetCollectedItemNames()
    {
        List<string> names = new List<string>();
        foreach (var item in collectedItems)
        {
            //guardamos el nombre del Sprite
            names.Add(item.name);
        }
        return names;
    }
    public void SetItemsByName(List<string> names, List<Sprite> allSprites)
    {
        //Limpiamos el inventario actual
        collectedItems.Clear();
        // Limpiar visuales
        for (int i = 0; i < slotImages.Count; i++)
        {
            if (slotImages[i] != null)
            {
                slotImages[i].sprite = null;
                slotImages[i].enabled = false;
            }
        }

        // Reconstruimos el inventario por nombre (hasta maxSlots)
        foreach (string name in names)
        {
            if (collectedItems.Count >= maxSlots) break;
            Sprite sprite = allSprites.Find(s => s.name == name);
            if (sprite != null)
            {
                AddItem(sprite);
            }
        }
    }

}
