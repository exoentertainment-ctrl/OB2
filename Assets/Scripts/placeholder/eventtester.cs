using System;
using UnityEngine;
using UnityEngine.Events;

public class eventtester : MonoBehaviour
{
    [SerializeField] UnityEvent OnHit;
    
    private Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHit?.Invoke();
    }

    public void Hit()
    {
        Debug.Log("hit");
    }
}
