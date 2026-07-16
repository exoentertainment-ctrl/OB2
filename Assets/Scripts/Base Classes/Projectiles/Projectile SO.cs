using UnityEngine;

[CreateAssetMenu(fileName = "Base Projectile SO", menuName = "Projectile SO/Base Projectile SO")]
public class ProjectileSO : ScriptableObject
{
    #region Prefabs

    public GameObject projectilePrefab;
    public GameObject impactPrefab;

    #endregion
    
    public AudioClipSO impactSound;
    public int maxHealth;
    public int moveSpeed;
    public int lifetime;
    public float damage;
    public LayerMask targetLayers;
    public float damageUpgradeAmount;
}
