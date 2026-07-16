using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MirvMissileSpawner : MonoBehaviour
{
    [SerializeField] ProjectileSO projectileSO;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private GameObject secondaryMissile;
    [SerializeField] private int distanceToSpawn;

    GameObject target;

    private void OnDisable()
    {
        target = null;
    }

    private void Update()
    {
        if(target == null)
            FindTarget();
        else
        {
            CheckDistanceToTarget();
        }
    }

    void CheckDistanceToTarget()
    {
        if(Vector3.Distance(transform.position, target.transform.position) <= distanceToSpawn)
            SpawnSecondaryMissiles();
    }
    
    void FindTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity,
            projectileSO.targetLayers);

        if (possibleTargets.Length > 0)
        {
            target = possibleTargets[Random.Range(0, possibleTargets.Length)].transform.root.gameObject;
        }
    }

    void SpawnSecondaryMissiles()
    {
        //Play SFX
        //Play some sort of VFX of puff of smoke or small explosion
        
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject missile = Instantiate(secondaryMissile, spawnPoint.position, spawnPoint.rotation);
            missile.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5)) * 500, ForceMode.Force);
            //missile.GetComponent<Rigidbody>().AddExplosionForce(10, transform.position, 5);
        }

        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToSpawn);
    }
}
