using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //Control de movimiento del jugador
    public static bool CanPlayerMove { get; private set; } = true;

    //Variables para escenario
    public GameObject[] scenarios;
    public GameObject player;
    public FadeController fadeController;

    private int currentScenario = 0;

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

    //Sistema de Misiones
    public GameObject missionPanel;
    public TextMeshProUGUI missionUIText;
    private List<string> activeMissions = new List<string>();

    // Variables privadas para el sistema de diálogo
    private List<string> currentDialogPages;
    private int currentPageIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string currentFullText = "";
    private AudioClip currentTypingSound;

    public AudioSource audioSource;

    // Variable para controlar si ya se mostró el diálogo de Pintacaritas
    private bool pintacaritasDialogShown = false;
    // Variable para controlar si ya se mostró el diálogo de Vendedor
    private bool vendedorDialogShown = false;

    public bool DialogFinished { get; private set; } = false;

    void Awake()
    {
        // Permitir una instancia por escena (NO usar DontDestroyOnLoad para cambios de escena/nivel)
        // Solo persistir si estamos en la misma escena (para cambios de escenario dentro de la misma escena)
        if (Instance == null)
        {
            Instance = this;
            // NO usar DontDestroyOnLoad - cada escena tendrá su propio GameManager
            // DontDestroyOnLoad(gameObject);
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
            
            Debug.Log($"[GameManager] Inicializado en escena: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
        else if (Instance != this)
        {
            // Si ya existe una instancia EN ESTA ESCENA (duplicado), destruir este
            Debug.LogWarning($"GameManager duplicado encontrado en {gameObject.name}. Destruyendo este.");
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Limpiar la instancia singleton cuando se destruya
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        // Verificar que las referencias estén asignadas
        if (npcDialogBox == null)
            Debug.LogError("npcDialogBox no está asignado en el GameManager!");
        if (npcDialogText == null)
            Debug.LogError("npcDialogText no está asignado en el GameManager!");
        if (npcName == null)
            Debug.LogError("npcName no está asignado en el GameManager!");
        if (npcImage == null)
            Debug.LogError("npcImage no está asignado en el GameManager!");
            
        // Verificar si estamos en la escena Pintacaritas_Level o Vendedor_Level
        CheckForIntroScenes();
    }

    void Update()
    {
        // Detectar click cuando el diálogo esta activo
        if (npcDialogBox != null && npcDialogBox.activeSelf && Input.GetMouseButtonDown(0))
        {
            HandleDialogClick();
        }
        //Detecta cuando el cuadro de texto esta activo
        if (textBox != null && textBox.activeSelf && Input.GetMouseButtonDown(0))
        {
            CloseNonCollectableText();
        }
    }

    public void ShowNonCollectableText(string text)
    {
        if (textBox == null || infoText == null)
        {
            Debug.LogWarning("GameManager: textBox o infoText es null, no se puede mostrar texto.");
            return;
        }
        
        textBox.SetActive(true);
        infoText.text = text;
        CanPlayerMove = false; // Bloquear movimiento del jugador
    }

    // Función que recibe las páginas del diálogo
    public void NPCShowText(List<string> dialogPages, string name, Sprite image, AudioClip typingSound = null)

    {
        if (npcDialogBox == null || npcDialogText == null || npcName == null || npcImage == null)
        {
            Debug.LogWarning("GameManager: Referencias UI de diálogo son null, no se puede mostrar diálogo.");
            return;
        }
        
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
        if (npcDialogText == null)
        {
            Debug.LogWarning("GameManager: npcDialogText es null, no se puede animar texto.");
            yield break;
        }
        
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
        if (npcDialogText == null)
        {
            Debug.LogWarning("GameManager: npcDialogText es null, no se puede manejar click de diálogo.");
            return;
        }
        
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
        if (npcDialogBox != null)
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
    public void ChangeScenario(int newScenario, Transform specificSpawn)
    {
        if (newScenario < 0 || newScenario >= scenarios.Length || newScenario == currentScenario)
            return;

        if (fadeController != null)
            StartCoroutine(ChangeWithFade(newScenario, specificSpawn));
        else
            ChangeDirect(newScenario, specificSpawn);
    }

    void ChangeDirect(int newScenario, Transform specificSpawn)
    {
        scenarios[currentScenario].SetActive(false);
        scenarios[newScenario].SetActive(true);

        if (player != null && specificSpawn != null)
            player.transform.position = specificSpawn.position;

        currentScenario = newScenario;
    }

    IEnumerator ChangeWithFade(int newScenario, Transform specificSpawn)
    {
        // Bloquear movimiento durante la transición
        CanPlayerMove = false;

        yield return fadeController.FadeOut();

        scenarios[currentScenario].SetActive(false);
        scenarios[newScenario].SetActive(true);

        if (player != null && specificSpawn != null)
            player.transform.position = specificSpawn.position;

        currentScenario = newScenario;

        yield return fadeController.FadeIn();

        // Desbloquear movimiento después de la transición
        CanPlayerMove = true;
    }

    //Misiones
    
    //Agregar nueva misión
    public void AddMission(string missionDescription)
    {
        if (!activeMissions.Contains(missionDescription))
        {
            activeMissions.Add(missionDescription);
            UpdateMissionUI();
            
            // Mostrar el panel de misiones si estaba oculto
            if (missionPanel != null && activeMissions.Count > 0)
                missionPanel.SetActive(true);
        }
    }

    //Completar misión
    public void CompleteMission(string missionDescription)
    {
        if (activeMissions.Contains(missionDescription))
        {
            activeMissions.Remove(missionDescription);
            UpdateMissionUI();
            
            // Ocultar el panel si no hay misiones activas
            if (missionPanel != null && activeMissions.Count == 0)
                missionPanel.SetActive(false);
        }
    }

    //Verificar si tiene una misión activa
    public bool HasMission(string missionDescription)
    {
        return activeMissions.Contains(missionDescription);
    }

    //Actualizar el texto del panel de misiones
    private void UpdateMissionUI()
    {
        if (missionUIText != null)
        {
            if (activeMissions.Count > 0)
            {
                missionUIText.text = "Misiones:\n" + string.Join("\n", activeMissions);
            }
            else
            {
                missionUIText.text = "";
            }
        }
    }

    // Verificar si estamos en la escena Pintacaritas_Level o Vendedor_Level y mostrar diálogo
    void CheckForIntroScenes()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Pintacaritas_Level" && !pintacaritasDialogShown)
        {
            StartCoroutine(ShowPintacaritasIntroDialog());
        }
        if (currentSceneName == "Vendedor_Level" && !vendedorDialogShown)
        {
            StartCoroutine(ShowVendedorIntroDialog());
        }
    }

    // Diálogo de introducción de Pintacaritas
    IEnumerator ShowPintacaritasIntroDialog()
    {
        yield return new WaitForSeconds(0.5f);
        pintacaritasDialogShown = true;
        PlayerController playerController = FindObjectOfType<PlayerController>();
        Sprite playerImage = null;
        AudioClip typingSound = null;
        if (playerController != null)
        {
            playerImage = playerController.playerDialogImage;
            typingSound = playerController.playerTypingSound;
        }
        List<string> dialogPages = new List<string>
        {
            "Hasta ahora no ví a ninguna de las pintacaritas...",
            "Debería acercarme a preguntar si necesitan ayuda en algo.",
            "¿Por dónde estarán ellas?",
            "Mejor las busco."
        };
        NPCShowText(dialogPages, "...", playerImage, typingSound);
    }

    // Diálogo de introducción de Vendedor
    IEnumerator ShowVendedorIntroDialog()
    {
        yield return new WaitForSeconds(0.5f);
        vendedorDialogShown = true;
        PlayerController playerController = FindObjectOfType<PlayerController>();
        Sprite playerImage = null;
        AudioClip typingSound = null;
        if (playerController != null)
        {
            playerImage = playerController.playerDialogImage;
            typingSound = playerController.playerTypingSound;
        }
        List<string> dialogPages = new List<string>
        {
            "No se me ocurre qué hacer hoy...",
            "Tal vez debería preguntar si alguien necesita algo.",
            "Voy a caminar un poco."
        };
        NPCShowText(dialogPages, "...", playerImage, typingSound);
    }



    //añadido para restringir el movimiento del jugador mientras duerme
    public void SetPlayerMovement(bool canMove)
    {
        CanPlayerMove = canMove;
    }

}
