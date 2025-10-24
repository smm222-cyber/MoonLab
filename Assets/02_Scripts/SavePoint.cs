using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public string saveFileName = "savegame.json";
    public SaveUI saveUI;
    public GameObject interactIndicator;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Presionaste E");
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            saveUI.Show(other.GetComponent<PlayerController>(), saveFileName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactIndicator.SetActive(true);
            Debug.Log("Jugador entró al punto de guardado");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactIndicator.SetActive(false);
            Debug.Log("Jugador salió del punto de guardado");
        }
    }


}
