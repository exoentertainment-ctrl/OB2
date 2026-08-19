using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    #region --Serialize Field--

    [SerializeField] GameObject cometPrefab;
    [SerializeField] private int minSpawnTime;
    [SerializeField] private int maxSpawnTime;
    [SerializeField] private Transform spawnPoint;

    #endregion

    private int spawnTime;
    float lastSpawnTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);    
        lastSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawnTime > spawnTime)
        {
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            lastSpawnTime = Time.time;
            Instantiate(cometPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
