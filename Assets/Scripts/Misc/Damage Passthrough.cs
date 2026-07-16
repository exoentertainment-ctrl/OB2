using System;
using UnityEngine;

public class DamagePassthrough : MonoBehaviour, IHealth
{
    public void TakeDamage(float damage)
    {
        transform.root.TryGetComponent<IHealth>(out IHealth health);
        health?.TakeDamage(damage);
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
