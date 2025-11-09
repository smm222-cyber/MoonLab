using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void ChangeLevelSelector(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
     public void ChangeLevelSelector(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }
}
