using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidHealth : MonoBehaviour, IHealth
{
    #region --Serialize Field--

    [SerializeField] GameObject debrisPrefab;
    [SerializeField] AudioClipSO destroySFX;
    [SerializeField] private float maxHealth;

    #endregion
    
    private float currentHealth;
    private bool isHit;
    private bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    private void Update()
    {
        isHit = false;
    }

    public void TakeDamage(float damage)
    {
        if(!isHit)
        {
            isHit = true;
            currentHealth -= damage;
            
            if (currentHealth <= 0 && !isDead)
            {
                isDead = true;
                SpawnDebris();
            }
        }
    }

    void SpawnDebris()
    {
        if(destroySFX != null)
            if(AudioManager.instance != null)
                AudioManager.instance.PlaySound(destroySFX, transform.position);
        
        Instantiate(debrisPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    
    public float GetHealth()
    {
        return 0;
    }

    public float GetShield()
    {
        return 0;
    }
}
