using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    //Texto para los objetos no recolectables
    public GameObject textBox;
    public TextMeshProUGUI infoText;

    //Texto para los dialogos con npc
    public GameObject npcDialogBox;
    public TextMeshProUGUI npcDialogText;
    public TextMeshProUGUI npcName;
    public Image npcImage;
    void Awake()
    {
        // Asegura que solo haya un GameManager activo
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShowNonCollectableText(string text)
    {
        textBox.SetActive(true);
        infoText.text = text;
    }
    public void NPCShowText(string text, string name, Sprite image)
    {
        npcDialogBox.SetActive(true);
        npcDialogText.text = text;
        npcName.text = name;
        npcImage.sprite = image;
    }
}

