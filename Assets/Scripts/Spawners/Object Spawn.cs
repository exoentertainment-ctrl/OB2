using System;
using UnityEngine;

public class ShipSpawn : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private int spawnTime;
    [SerializeField] Transform spawnPoint;

    #endregion

    #region Variables

    private float lastSpawnTime;
    ObjectPool objectPool;
    private GameObject target;

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

    public void SetTarget(GameObject target)
    {
        this.target = target;

        for (int x = 0; x < objectPool.GetPooledObjectCount(); x++)
        {
            objectPool.pooledObjects[x].GetComponent<FighterMovement>().SetTarget(target);
        }
    }
    
    void SpawnObject()
    {
        if (Time.time - lastSpawnTime > spawnTime)
        {
            GameObject spawnObject = objectPool.GetPooledObject();
            
            if (spawnObject != null)
            {
                spawnObject.transform.position = spawnPoint.position;
                spawnObject.transform.rotation = spawnPoint.rotation;
                spawnObject.SetActive(true);

                spawnObject.TryGetComponent<FighterMovement>(out FighterMovement movement);
                movement?.SetBase(spawnPoint.gameObject);
                movement?.SetTarget(target);
            }
            
            lastSpawnTime = Time.time;
        }
    }
}
