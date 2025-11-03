using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLevel : MonoBehaviour
{
    public string levelSceneName = "NivelCosturaScene";

    public void ReturnToLevel()
    {
        SceneManager.LoadScene(levelSceneName);
    }
}
