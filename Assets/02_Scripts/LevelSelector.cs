using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void ChangeLevelSelector(string levelName)
    {
        // Asegurar que el acumulador de decisiones exista antes de cambiar de escena
        ChoiceCounterManager.EnsureExists();
        SceneManager.LoadScene(levelName);
    }
     public void ChangeLevelSelector(int levelNumber)
    {
        // Asegurar que el acumulador de decisiones exista antes de cambiar de escena
        ChoiceCounterManager.EnsureExists();
        SceneManager.LoadScene(levelNumber);
    }
}
