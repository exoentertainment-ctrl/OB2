using UnityEngine;

public class TransportShipSpawner : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private int spawnTime;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int numToSpawn;

    #endregion

    #region Variables

    private int spawnAmount;
    private float lastSpawnTime;
    ObjectPool objectPool;

    #endregion

    private void Awake()
    {
        objectPool = GetComponent<ObjectPool>();
    }
    
    private void Start()
    {
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        SpawnObject();
    }
    
    void SpawnObject()
    {
        if (Time.time - lastSpawnTime > spawnTime)
        {
            if (spawnAmount < numToSpawn)
            {
                GameObject spawnObject = objectPool.GetPooledObject();

                if (spawnObject != null)
                {
                    spawnObject.transform.position = spawnPoint.position;
                    spawnObject.transform.rotation = spawnPoint.rotation;
                    spawnObject.SetActive(true);
                }

                spawnAmount++;
                lastSpawnTime = Time.time;
            }
        }
    }
}
