using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //Control de movimiento del jugador
    public static bool CanPlayerMove { get; private set; } = true;

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
    
    
    public float audioFadeOutDelay = 0.2f;
    //Duración del fadeout del audio
    public float audioFadeOutDuration = 0.3f;

    // Variables privadas para el sistema de diálogo
    private List<string> currentDialogPages;
    private int currentPageIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string currentFullText = "";
    private AudioClip currentTypingSound;

    public AudioSource audioSource;

    public bool DialogFinished { get; private set; } = false;

    void Awake()
    {
        // Asegura que solo haya un GameManager activo
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Detectar click cuando el diálogo esta activo
        if (npcDialogBox.activeSelf && Input.GetMouseButtonDown(0))
        {
            HandleDialogClick();
        }
        //Detecta cuando el cuadro de texto esta activo
        if (textBox.activeSelf && Input.GetMouseButtonDown(0))
        {
            CloseNonCollectableText();
        }
    }

    public void ShowNonCollectableText(string text)
    {
        textBox.SetActive(true);
        infoText.text = text;
        CanPlayerMove = false; // Bloquear movimiento del jugador
    }

    // Función que recibe las páginas del diálogo
    public void NPCShowText(List<string> dialogPages, string name, Sprite image, AudioClip typingSound = null)

    {
        currentDialogPages = dialogPages;
        currentPageIndex = 0;
        DialogFinished = false;

        npcDialogBox.SetActive(true);
        npcName.text = name;
        npcImage.sprite = image;

        currentTypingSound = typingSound;
        CanPlayerMove = false; 

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

        if (currentTypingSound != null)
        {
            audioSource.clip = currentTypingSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        foreach (char c in text)
        {
            npcDialogText.text += c;
            yield return new WaitForSeconds(1f / textSpeed);
        }

        // Fadeout del audio cuando termina de escribir
        if (audioSource.isPlaying)
            yield return StartCoroutine(FadeOutAudio());

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

            //Detener sonido
            if (audioSource.isPlaying)
                StartCoroutine(FadeOutAudio());
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

    //Cierra los diálogos
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
        if (audioSource.isPlaying)
            StartCoroutine(FadeOutAudio());
        DialogFinished = true;
        CanPlayerMove = true; // Desbloquear movimiento del jugador
    }
    
    // Fadeout suave del audio
    private IEnumerator FadeOutAudio()
    {
        // Esperar antes de empezar el fade-out
        yield return new WaitForSeconds(audioFadeOutDelay);
        
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < audioFadeOutDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / audioFadeOutDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Restaurar el volumen original para el próximo uso
    }
    
    public void CloseNonCollectableText()
    {
        if (textBox != null)
            textBox.SetActive(false);
        CanPlayerMove = true; 
    }
}