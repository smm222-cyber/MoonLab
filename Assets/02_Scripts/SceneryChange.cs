using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryChange : MonoBehaviour
{
    public int destinationScenarioIndex;
    public Transform destinationSpawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.ChangeScenario(destinationScenarioIndex, destinationSpawn);
        }
    }
}