using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SaveUI : MonoBehaviour
{
    public GameObject panel;
    public Button yesButton;
    public Button noButton;

    private PlayerController player;
    private string fileName;

    void Start()
    {
        panel.SetActive(false);

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    public void Show(PlayerController playerToSave, string saveFile)
    {
        player = playerToSave;
        fileName = saveFile;
        panel.SetActive(true);
        // pausa el juego
        Time.timeScale = 0f; 
    }

    private void OnYesClicked()
    {
        SaveSystem.SaveGame(fileName, player);
        panel.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Partida guardada");
    }

    private void OnNoClicked()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("No se guardó la partida");
    }
}
