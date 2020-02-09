using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemiesFolder;
    GameState gameState;
    // Start is called before the first frame update
    void Start()
    {
        gameState = Camera.main.GetComponent<GameState>();
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (!gameState.isGameOver())
        {
            GameObject enemyGO = Instantiate(enemyPrefab, enemiesFolder.transform);
            enemyGO.transform.position = transform.position;

            NavMeshAgent enemyGONavMesh = enemyGO.GetComponent<NavMeshAgent>();
            enemyGONavMesh.enabled = true;

            yield return new WaitForSeconds(5f);
        }
    }
}
