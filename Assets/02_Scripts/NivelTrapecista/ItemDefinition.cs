using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDefinition", menuName = "NivelTrapecista/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    [Tooltip("Unique mission id used by GameManager, e.g. 'BuscarHilo'")]
    public string itemId;

    public Sprite sprite;
    public AudioClip pickupSound;

    [Tooltip("Optional prefab for the interaction UI (Press E)")]
    public GameObject interactUI;
}