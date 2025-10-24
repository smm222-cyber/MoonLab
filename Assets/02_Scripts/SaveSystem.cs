using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

[System.Serializable]
public class PlayerData
{
    //esto es lo que gusrdamos
    //posision
    public float[] position;
    //vida
    public int health;
    //items
    public List<string> collectedItems;
}
public class SaveSystem : MonoBehaviour
{
    public static void SaveGame(string fileName, PlayerController player)
    {
        PlayerData data = new PlayerData();
        data.position = new float[3];
        data.position[0] = player.transform.position.x;
        data.position[1] = player.transform.position.y;
        data.position[2] = player.transform.position.z;
        data.health = player.health;

        // Guardar items, si no hay InventoryManager o no hay items, guardamos lista vacia
        if (InventoryManager.Instance != null)
        {
            data.collectedItems = InventoryManager.Instance.GetCollectedItemNames();
        }
        else
        {
            data.collectedItems = new List<string>(); // lista vacía
        }



        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);
    }


    public static void LoadGame(string fileName, PlayerController player, List<Sprite> allItemSprites)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            // Posición y vida
            player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            player.health = data.health;

            // Restaurar inventario, si no hay InventoryManager, lo ignoramos
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.SetItemsByName(data.collectedItems, allItemSprites);
            }

        }
        else
        {
            Debug.Log("No hay partida guardada");
        }
    }

}
