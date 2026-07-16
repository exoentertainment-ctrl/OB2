using System;
using System.Collections;
using UnityEngine;

public class AutoTurnoff : MonoBehaviour
{
    [SerializeField] private int duration;

    private void OnEnable()
    {
        StartCoroutine(TurnoffRoutine());
    }

    IEnumerator TurnoffRoutine()
    {
        yield return new WaitForSeconds(duration);
        
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
