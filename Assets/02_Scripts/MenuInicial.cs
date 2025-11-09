using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

   
    public void salir()
    {
        Debug.Log("Salir..");
        Application.Quit();
    }

   
    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("LoadSceneByName: sceneName está vacío o null.");
            return;
        }

        // Restaurar timeScale por si el juego estaba pausado
        Time.timeScale = 1f;

        if (IsSceneInBuildSettings(sceneName))
        {
            Debug.Log($"Cargando escena: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning($"La escena '{sceneName}' no está en Build Settings. Añádela en File -> Build Settings.");
        }
    }

    // Comprueba si una escena por nombre está incluida en las Build Settings
    private bool IsSceneInBuildSettings(string sceneName)
    {
        int count = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (string.Equals(name, sceneName, System.StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
}
