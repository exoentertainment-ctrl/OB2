using System;
using UnityEngine;

public class WarpJumpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnObject;
    [SerializeField] Transform spawnPoint;
    [SerializeField] private float tunnelPositionSpawn;

    //GameObject spawnObject;
    
    private void Start()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("Planet").transform);
    }

    public void SetSpawnObject(GameObject objectToSpawn)
    {
        spawnObject = objectToSpawn;
    }
    
    private void Update()
    {
        if (spawnPoint.localPosition.z >= tunnelPositionSpawn)
        {
            if(spawnObject != null)
                Instantiate(spawnObject, spawnPoint.position, spawnPoint.rotation);
            
            Destroy(gameObject);
        }
    }
}
