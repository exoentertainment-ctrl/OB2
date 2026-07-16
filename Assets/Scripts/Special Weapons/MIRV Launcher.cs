using System;
using System.Collections;
using UnityEngine;

public class MIRVLauncher : MonoBehaviour
{
    #region --Serialize Field--

    [SerializeField] GameObject missilePrefab;
    [SerializeField] private AudioClipSO launchSFX;
    [SerializeField] private float launchDelay;
    [SerializeField] private Transform[] spawnPoints;

    #endregion

    private void OnEnable()
    {
        StartCoroutine(LaunchRoutine());
    }

    IEnumerator LaunchRoutine()
    {
        foreach (Transform spawn in spawnPoints)
        {
            if(AudioManager.instance != null)
                if(launchSFX != null)
                    AudioManager.instance.PlaySound(launchSFX);
                
            Instantiate(missilePrefab, spawn.position, spawn.rotation);
            
            yield return new WaitForSeconds(launchDelay);
        }   
    }
}
