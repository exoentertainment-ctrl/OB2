using UnityEngine;

public interface IHealth
{
    public virtual void TakeDamage(float damage){}
    
    public float GetHealth();
    public float GetShield();
}
