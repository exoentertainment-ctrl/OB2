using System;
using UnityEngine;

public class BlackholeLauncher : MonoBehaviour
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private int bombEjectionForce;
    
    private void OnEnable()
    {
        GameObject bomb = Instantiate(blackholePrefab, transform.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody>().AddForce(transform.forward * bombEjectionForce, ForceMode.Impulse);
    }
}
