using UnityEngine;

public class BaseShield : MonoBehaviour, IHealth
{
    public virtual void TakeDamage(float damage)
    {}
    
    public float GetHealth()
    {
        return 0;
    }

    public float GetShield()
    {
        return 0;
    }
}
