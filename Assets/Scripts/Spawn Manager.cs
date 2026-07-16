using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int concurrentSpawns = 1;
    [SerializeField] private Collider spawnArea;
    [SerializeField] private GameObject spawnWarpTunnel;
    [SerializeField] private int timeBeforeFirstSpawn;
    [SerializeField] UnityEvent[] onSpawnBegin;
    [SerializeField] UnityEvent[] onSpawnEnd;
    
    private float lastSpawnTime;
    
    #region -- Serialized Fields --
    
    [Header("Scriptable Object")] 
    [SerializeField] private EnemySpawnerScriptableObject[] spawnWavesSO;
    
    #endregion
    
    int currentWave;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastSpawnTime = Time.time;    
    }
    
    public void SpawnEnemy()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    public void SpawnEnemy(GameObject ship)
    {
        Vector3 spawnPos = new Vector3(Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
            Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
            Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z));

        GameObject spawnTunnel = Instantiate(spawnWarpTunnel, spawnPos, Quaternion.identity);
        spawnTunnel.GetComponent<WarpJumpSpawner>().SetSpawnObject(ship);
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (Time.timeScale > 0)
        {
            if (currentWave >= spawnWavesSO.Length)
            {
                yield break;
            }
            
            if(currentWave == 0)
                yield return new WaitForSeconds(timeBeforeFirstSpawn);
            else
                yield return new WaitForSeconds(spawnWavesSO[currentWave].timeToWaveStart);
            
            onSpawnBegin[currentWave]?.Invoke();

            for (int y = 0; y < concurrentSpawns; y++)
            {
                for (int x = 0; x < spawnWavesSO[currentWave].numSpawns; x++)
                {
                    Vector3 spawnPos = new Vector3(Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                        Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                        Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z));

                    GameObject spawnTunnel = Instantiate(spawnWarpTunnel, spawnPos, Quaternion.identity);
                    spawnTunnel.GetComponent<WarpJumpSpawner>().SetSpawnObject(spawnWavesSO[currentWave]
                        .enemyShipsPrefabs[Random.Range(0, spawnWavesSO[currentWave].enemyShipsPrefabs.Length)]);

                    // int enemySpawn = Random.Range(0, spawnWavesSO[currentWave].enemyShipsPrefabs.Length);
                    // Instantiate(spawnWavesSO[currentWave].enemyShipsPrefabs[enemySpawn], spawnPos,
                    //     transform.rotation);

                    float waitTime = Random.Range(spawnWavesSO[currentWave].mintimeBetweenSpawns,
                        spawnWavesSO[currentWave].maxtimeBetweenSpawns);

                    yield return new WaitForSeconds(waitTime);
                }
            }

            onSpawnEnd[currentWave]?.Invoke();
            currentWave++;
        }
    }

    public void ChangeConcurrentSpawn(int value)
    {
        concurrentSpawns = value;
    }
}
