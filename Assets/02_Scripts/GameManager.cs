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

    //Velocidad de texto
    public float textSpeed = 30f;

    // Variables privadas para el sistema de diálogo
    private List<string> currentDialogPages;
    private int currentPageIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string currentFullText = "";

    void Awake()
    {
        // Asegura que solo haya un GameManager activo
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Detectar click cuando el diálogo está activo
        if (npcDialogBox.activeSelf && Input.GetMouseButtonDown(0))
        {
            HandleDialogClick();
        }
    }

    public void ShowNonCollectableText(string text)
    {
        textBox.SetActive(true);
        infoText.text = text;
    }

    // Función que recibe las páginas del diálogo
    public void NPCShowText(List<string> dialogPages, string name, Sprite image)
    {
        currentDialogPages = dialogPages;
        currentPageIndex = 0;

        npcDialogBox.SetActive(true);
        npcName.text = name;
        npcImage.sprite = image;

        ShowCurrentPage();
    }

    // Muestra la página actual con animación
    private void ShowCurrentPage()
    {
        if (currentPageIndex < currentDialogPages.Count)
        {
            currentFullText = currentDialogPages[currentPageIndex];

            // Detener animación anterior si existe
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            typingCoroutine = StartCoroutine(TypeText(currentFullText));
        }
    }

    // Animación de texto letra por letra
    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        npcDialogText.text = "";

        foreach (char c in text)
        {
            npcDialogText.text += c;
            yield return new WaitForSeconds(1f / textSpeed);
        }

        isTyping = false;
    }

    // Maneja los clicks en el diálogo
    private void HandleDialogClick()
    {
        // Si está escribiendo, completar el texto inmediatamente
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            npcDialogText.text = currentFullText;
            isTyping = false;
        }
        // Si ya terminó de escribir, pasar a la siguiente página
        else
        {
            currentPageIndex++;

            // Si hay más páginas, mostrar la siguiente
            if (currentPageIndex < currentDialogPages.Count)
            {
                ShowCurrentPage();
            }
            // Si no hay más páginas, cerrar el diálogo
            else
            {
                CloseDialog();
            }
        }
    }

    // Cierra el cuadro de diálogo
    private void CloseDialog()
    {
        npcDialogBox.SetActive(false);
        currentDialogPages = null;
        currentPageIndex = 0;
        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }
}