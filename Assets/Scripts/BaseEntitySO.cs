using UnityEngine;

public class BaseEntitySO : ScriptableObject
{
    public int maxHealth;
    public int maxShield;
    public float lowShieldPercentage;
    
    public float upgradeAmount;
    public GameObject explosionPrefab;
    public int numExplosions;
    public float explosionFrequency;
}
