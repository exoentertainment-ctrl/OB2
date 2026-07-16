using System;
using System.Collections;
using UnityEngine;

public class TurretImprove : MonoBehaviour
{
    #region --Serialize Field--

    [SerializeField] private int duration;
    [SerializeField] private GameObject[] normalTurrets;
    [SerializeField] GameObject[] improvedTurrets;

    #endregion

    private void OnEnable()
    {
        StartCoroutine(SwapTurretsRoutine());
    }

    IEnumerator SwapTurretsRoutine()
    {
        foreach (GameObject turret in normalTurrets)
        {
            turret.SetActive(false);
        }

        foreach (GameObject turret in improvedTurrets)
        {
            turret.SetActive(true);
        }
        
        yield return new WaitForSeconds(duration);
        
        foreach (GameObject turret in normalTurrets)
        {
            turret.SetActive(true);
        }

        foreach (GameObject turret in improvedTurrets)
        {
            turret.SetActive(false);
        }
    }
}
