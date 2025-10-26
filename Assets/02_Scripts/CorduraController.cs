using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorduraController : MonoBehaviour
{
    [Header("Referencias")]
    public Slider corduraSlider;
    public Transform player;
    public Transform bed;
    public Animator animator;

    [Header("Parámetros")]
    public float cordura = 100f;
    public float recoverSpeed = 15f;

    private bool isSleeping = false;
    private bool nearBed = false;
    private Rigidbody2D playerRb;
    private float originalGravityScale;

    void Start()
    {
        if (corduraSlider != null)
            corduraSlider.value = cordura;

        if (animator == null && player != null)
            animator = player.GetComponentInChildren<Animator>();

        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
                originalGravityScale = playerRb.gravityScale;
        }
    }

    void Update()
    {
        // Dormir y levantarse con P
        if (Input.GetKeyDown(KeyCode.P) && nearBed)
        {
            if (!isSleeping)
                GoToSleep();
            else
                WakeUp();
        }

        // Recuperar cordura cuando duerme
        if (isSleeping)
        {
            cordura += recoverSpeed * Time.deltaTime;
            cordura = Mathf.Clamp(cordura, 0, 100);

            if (corduraSlider != null)
                corduraSlider.value = cordura;
        }
    }

    private void GoToSleep()
    {
        isSleeping = true;

        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.gravityScale = 0f;
            playerRb.isKinematic = true;
        }

        if (bed != null)
            player.position = bed.position + new Vector3(0, 0.5f, 0);

        if (animator != null)
        {
            animator.SetBool("Sleeped", true);
            animator.Play("Sleeped"); 
        }

        Debug.Log("💤 El jugador se ha echado a dormir");
    }

    private void WakeUp()
    {
        isSleeping = false;

        if (playerRb != null)
        {
            playerRb.isKinematic = false;
            playerRb.gravityScale = originalGravityScale;
        }

        if (animator != null)
            animator.SetBool("Sleeped", false);

        Debug.Log("☀️ El jugador se levantó de la cama");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bed"))
        {
            nearBed = true;
           
            if (!isSleeping)
            {
                if (animator != null)
                {
                    animator.SetBool("Sleeped", true);
                    animator.Play("Sleeped");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bed"))
        {
            nearBed = false;

            // Si no está durmiendo, back to idle
            if (!isSleeping && animator != null)
                animator.SetBool("Sleeped", false);
        }
    }

    public bool IsSleeping()
    {
        return animator != null && animator.GetBool("Sleeped");
    }

}
