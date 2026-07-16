using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    #region -- Serialized Fields --
    
    [Header("Variables")]
    [SerializeField] GameObject bossPrefab;
    
    [Header("Scriptable Object")] 
    [SerializeField] private EnemySpawnerScriptableObject[] spawnWavesSO;
    
    #endregion
    
    int currentWave;

    public void SpawnEnemy()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        if (Time.timeScale > 0)
        {
            if(currentWave >= spawnWavesSO.Length)
                yield break;
            
            for (int x = 0; x < spawnWavesSO[currentWave].numSpawns; x++)
            {
                int enemySpawn = Random.Range(0, spawnWavesSO[currentWave].enemyShipsPrefabs.Length);
                Instantiate(spawnWavesSO[currentWave].enemyShipsPrefabs[enemySpawn], transform.position,
                         transform.rotation);

                float waitTime = Random.Range(spawnWavesSO[currentWave].mintimeBetweenSpawns,
                    spawnWavesSO[currentWave].maxtimeBetweenSpawns);
                
                yield return new WaitForSeconds(waitTime);
            }

            currentWave++;
        }
    }
    
    public void SpawnBoss()
    {   
        if(GameObject.FindGameObjectWithTag("Boss incoming") != null)
            GameObject.FindGameObjectWithTag("Boss incoming").SetActive(true);
        
        Instantiate(bossPrefab, transform.position, Quaternion.identity);
    }
}