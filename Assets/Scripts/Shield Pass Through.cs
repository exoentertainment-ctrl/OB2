using System;
using UnityEngine;

public class ShieldPassThrough : MonoBehaviour, IHealth
{
    BaseShield shieldHealth;

    private void Awake()
    {
        shieldHealth = GetComponentInParent<BaseShield>();
    }

    public void TakeDamage(float damage)
    {
        shieldHealth.TakeDamage(damage);
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
