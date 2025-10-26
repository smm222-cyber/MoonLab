using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryChange : MonoBehaviour
{
    public int targetSceneIndex;
    public Transform targetSpawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.ChangeScenario(targetSceneIndex, targetSpawn);
        }
    }
}