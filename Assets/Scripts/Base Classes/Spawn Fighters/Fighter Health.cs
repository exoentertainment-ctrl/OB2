using System;
using UnityEngine;
using UnityEngine.Events;

public class FighterHealth : MonoBehaviour, IHealth
{
    #region Serialized Fields

    [SerializeField] private BaseShipSO shipSO;
    [SerializeField] UnityEvent onShipDeath;

    #endregion
    
    #region Variables
    
    private float currentHealth;
    private bool isHit;
    private bool isDead;

    #endregion


    private void Start()
    {
        currentHealth = shipSO.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if(!isHit)
        {
            isHit = true;
            currentHealth -= damage;
                
            if (currentHealth <= 0 && !isDead)
            {
                ResourceManager.instance.IncreaseCredits(shipSO.pointValue);
                isDead = true;
                onShipDeath?.Invoke();
                Instantiate(shipSO.explosionPrefab, transform.position, Quaternion.identity);
            }
        }
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
