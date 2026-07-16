using System;
using UnityEngine;

public class AsteroidDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] AudioClipSO collisionSFX;
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        other.gameObject.TryGetComponent<IHealth>(out IHealth health);
        health?.TakeDamage(damage);
        
        if(collisionSFX != null)
            if(AudioManager.instance != null)
                AudioManager.instance.PlaySound(collisionSFX, transform.position);
        
        Destroy(gameObject);
    }
}
