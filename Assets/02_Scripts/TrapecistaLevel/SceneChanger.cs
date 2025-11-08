using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("Nombre del nivel actual (para decidir el siguiente)")]
    [SerializeField] private string currentLevel = "Tutorial";

    // 🔹 Puedes definir los nombres exactos de las escenas aquí
    private const string TUTORIAL = "Tutorial_Level";
    private const string PINTACARITAS = "PintaCaritas_Level";
    private const string PAYASITO = "Payasito_Level";

    // ✅ Llama a este método desde el Item, NPC o evento cuando deba cambiar la escena
    public void ChangeScene()
    {
        string nextScene = GetNextScene(currentLevel);

        if (!string.IsNullOrEmpty(nextScene))
        {
            Debug.Log($"[SceneChanger] Cambiando de '{currentLevel}' a '{nextScene}'...");
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.LogWarning($"[SceneChanger] No hay escena configurada después de '{currentLevel}'.");
        }
    }

    // 🔁 Decide qué escena va después, según la actual
    private string GetNextScene(string current)
    {
        switch (current)
        {
            case TUTORIAL:
                return PINTACARITAS;

            case PINTACARITAS:
                return PAYASITO;

            case PAYASITO:
                Debug.Log("🎉 Juego completo. No hay más niveles configurados.");
                return null;

            default:
                Debug.LogWarning($"[SceneChanger] Escena desconocida: {current}");
                return null;
        }
    }
}
