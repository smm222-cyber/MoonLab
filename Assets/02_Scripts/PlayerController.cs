 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float speed = 5f;
    //Para salto
    public float jumpForce = 3f;
    public float raycastLenght = 0.1f;
    public LayerMask layerFloor;
    private bool onGround;
    
    private Rigidbody2D rb;

    //animator
    private Animator animator;

    //vida
    public int health = 100;

    //distincion de dormir
    public bool isSleeping = false;

    [Header("Configuración de Diálogos")]
    public Sprite playerDialogImage; // Imagen para diálogos
    public AudioClip playerTypingSound; // Sonido para diálogos
    
    [Header("Audio")]
    public AudioClip jumpClip; // Sonido que se reproducirá al saltar
    private AudioSource audioSource;
    public AudioClip walkClip; // Sonido de pasos en loop
    private AudioSource footstepSource;
    
    [Header("Audio Settings")]
   
    [Range(0f, 2f)] public float walkVolume = 1f; // volumen pasos
    [Range(0.1f, 3f)] public float walkPitch = 1f; // velocidad pasos
    public bool scaleWalkPitchWithSpeed = false; // si true, el pitch de pasos varía con la velocidad del input
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // Crear AudioSource para salto
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        // Crear un AudioSource separado para pasos
        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.playOnAwake = false;
        footstepSource.loop = true;
        // Aplicar valores iniciales de volumen/pitch para pasos
        footstepSource.volume = walkVolume;
        footstepSource.pitch = walkPitch;
    }

    // Update is called once per frame
    void Update()
    {
    //para poder dormir
        if (isSleeping)
        {
            // no poder moverse ni saltar 
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsGrounded", true); 
           
            return;
        }
    // Solo permitir movimiento si el GameManager lo permite
        if (GameManager.CanPlayerMove)
        {
            // Actualizar estado de suelo antes de procesar movimiento
            CheckGrounded();
            Movement();
            Jump();
        }
        else
        {
            // Detener la animación de movimiento cuando está bloqueado
            animator.SetFloat("Speed", 0f);
        }
    }
    void Movement()
    {
        float speedX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        // Changes the player orientation
        Vector3 scale = transform.localScale;
        if (speedX < 0)
            scale.x = -Mathf.Abs(scale.x);
        else if (speedX > 0)
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;

    animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
    // Move
        Vector3 position = transform.position;
        transform.position = new Vector3(position.x + speedX, position.y, position.z);
        
        // Sonido de pasos: reproducir en loop mientras se mueve y está en el suelo
        float horiz = Input.GetAxis("Horizontal");
        bool isMoving = Mathf.Abs(horiz) > 0.01f && GameManager.CanPlayerMove;
        
        // Detener sonido si el movimiento está bloqueado o no hay movimiento
        if (!GameManager.CanPlayerMove || !isMoving || !onGround || walkClip == null)
        {
            if (footstepSource != null && footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }
        // Reproducir sonido solo si podemos movernos y estamos en movimiento
        else if (isMoving && onGround && walkClip != null)
        {
            if (footstepSource != null && !footstepSource.isPlaying)
            {
                footstepSource.clip = walkClip;
                footstepSource.loop = true;
                // Aplicar ajustes de volumen y pitch antes de reproducir el loop
                footstepSource.volume = walkVolume;
                // Pitch constante definido por walkPitch (no escalar con input)
                footstepSource.pitch = walkPitch;
                footstepSource.Play();
            }
        }
    }

    // Comprueba si el jugador está en el suelo y actualiza la variable onGround
    bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLenght, layerFloor);
        onGround = hit.collider != null;
        animator.SetBool("IsGrounded", onGround);
        return onGround;
    }

    void Jump()
    {
        // onGround ya se actualiza en CheckGrounded() llamada desde Update
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            // Reproducir sonido de salto si está asignado
            if (jumpClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(jumpClip);
            }
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastLenght);
    }

}
