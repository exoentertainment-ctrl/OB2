using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    #region --Serialize Field--

    [SerializeField] int numAsteroids;
    [SerializeField] int distBetweenAsteroids;
    [SerializeField] GameObject[] asteroidPrefabs;
    [SerializeField] Collider collider;

    #endregion
    
    List<GameObject> spawnedAsteroids;

    private void Start()
    {
        spawnedAsteroids = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnAsteroids();
    }

    void SpawnAsteroids()
    {
        Vector3 asteroidPosition = new Vector3();
        
        if (spawnedAsteroids.Count < numAsteroids)
        {
            spawnedAsteroids.Add(Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)]));
            spawnedAsteroids[spawnedAsteroids.Count - 1].transform.parent = transform;

            do
            {
                asteroidPosition = new Vector3(Random.Range(collider.bounds.min.x, collider.bounds.max.x), Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                    Random.Range(collider.bounds.min.z, collider.bounds.max.z));
                
                spawnedAsteroids[spawnedAsteroids.Count - 1].transform.position = asteroidPosition;
                
            } while (!CheckDistBetweenAsteroids(spawnedAsteroids[spawnedAsteroids.Count - 1]));
            
        }
    }

    bool CheckDistBetweenAsteroids(GameObject spawnedAsteroid)
    {
        foreach (var asteroid in spawnedAsteroids)
        {
            if (spawnedAsteroid != asteroid)
            {
                if (Vector3.Distance(spawnedAsteroid.transform.position, asteroid.transform.position) <
                    distBetweenAsteroids)
                {
                    Debug.Log("asteroid is too close to: " + asteroid.name);
                    return false;
                }
            }
        }
        
        return true;
    }
}
