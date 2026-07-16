using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlakCannon : MonoBehaviour
{
    [SerializeField] private int innerDiamater;
    [SerializeField]  private int outerDiameter;
    [SerializeField] private AudioClipSO flakExplosionSFX;
    [SerializeField] private GameObject flakPrefab;
    [SerializeField] private float spawnTime;
    [SerializeField] private int spawnAmount;

    [SerializeField] private bool showGizmos;
    
    float lastSpawnTime;

    private void OnEnable()
    {
        StartCoroutine(SpawnFlakRoutine());
    }

    IEnumerator SpawnFlakRoutine()
    {
        for (int x = 0; x < spawnAmount; x++)
        {
            Vector3 spawnPos = (Random.insideUnitSphere * Random.Range(innerDiamater, outerDiameter));

            Instantiate(flakPrefab, spawnPos + transform.position, Quaternion.identity);

            if (AudioManager.instance != null)
                AudioManager.instance.PlaySound(flakExplosionSFX);

            yield return new WaitForSeconds(spawnTime);
        }

        gameObject.SetActive(false);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.DrawWireSphere(transform.position, innerDiamater);
            Gizmos.DrawWireSphere(transform.position, outerDiameter);
        }
    }
}
