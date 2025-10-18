using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject textBox;
    public TextMeshProUGUI infoText;
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
}
