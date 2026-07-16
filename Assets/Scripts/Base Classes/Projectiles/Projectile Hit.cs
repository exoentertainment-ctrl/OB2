using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileHit : MonoBehaviour, IUpgrade
{
    #region Serialized Fields

    [SerializeField] ProjectileSO projectileSO;

    [Tooltip("Does this projectile have a proximity fuse?")]
    [SerializeField] bool hasProximityFuse;
    [SerializeField] private float proximityRange;
    
    [SerializeField] bool disableOnHit;
    [SerializeField] private bool destroyOnHit;
    
    #endregion

    #region Variables

    private Collider collider;
    private float damageUpgrade = 1;

    #endregion

    private void Awake()
    {
        collider  = GetComponent<Collider>();
    }

    private void Update()
    {
        if(hasProximityFuse)
            CheckProximity();
    }

    private void FixedUpdate()
    {
        //CastRayCast();
    }

    private void OnEnable()
    {
        StartCoroutine(DisableObjectRoutine());
    }

    void CheckProximity()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(collider.bounds.center, proximityRange,
            projectileSO.targetLayers);

        if (possibleTargets.Length > 0)
        {
            foreach (Collider hit in possibleTargets)
            {
                hit.gameObject.TryGetComponent<IHealth>(out IHealth health);
                health?.TakeDamage(projectileSO.damage);
            }
            
            if(destroyOnHit)
                Destroy(gameObject);
            else if(disableOnHit)
                gameObject.SetActive(false);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        float HitRadius = 0.1f;
        float Dirt = 1f;
        float Burn = 1f;
        float Heat = 1f;
        float Clip = 0.7f;
        
        other.gameObject.TryGetComponent<IHealth>(out IHealth health);
        health?.TakeDamage(projectileSO.damage * damageUpgrade);
    
        other.gameObject.TryGetComponent<DamageFX>(out DamageFX damageFX);
        damageFX?.Hit(damageFX.transform.InverseTransformPoint(other.GetContact(0).point), HitRadius, Dirt, Burn,
            Heat, Clip);
    
        Vector3 collisionNormal = other.contacts[0].normal;
        Quaternion collisionRotation = Quaternion.LookRotation(collisionNormal);
        
        if(projectileSO.impactPrefab  != null)
            Instantiate(projectileSO.impactPrefab, other.contacts[0].point, collisionRotation);
        
        if(projectileSO.impactSound != null)
            if(AudioManager.instance != null)
                AudioManager.instance.PlaySound(projectileSO.impactSound, transform.position);
    
        if(destroyOnHit)
            Destroy(gameObject);
        else if(disableOnHit)
            gameObject.SetActive(false);
    }
    
    IEnumerator DisableObjectRoutine()
    {
        yield return new WaitForSeconds(projectileSO.lifetime);
        
        gameObject.SetActive(false);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (hasProximityFuse)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, proximityRange);
        }
    }

    public void Upgrade()
    {
        damageUpgrade += projectileSO.damageUpgradeAmount;
    }
}
