using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public void OnCreditsEnd()
    {
        // Cargar la escena del menú principal
        SceneManager.LoadScene("Menu"); 
    }

    void Update()
    {
        // Si hace clic con el mouse
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
