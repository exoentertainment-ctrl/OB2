using System;
using UnityEngine;
using System.Collections;
using MoreMountains.Feedbacks;

public class TurretAttack : MonoBehaviour
{
    #region Serialized Fields

    [Header("Components")] 
    [SerializeField] private bool requireLOS;
    [SerializeField] private Transform raycastOrigin;
    
    [SerializeField] private TurretSO turretSO;
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private Transform[] spawnPoints;
    
    [SerializeField] MMFeedbacks fireFeedbacks;

    [SerializeField] private bool showGizmos;
    
    #endregion
    
    private GameObject target;
    ObjectPool projectilePool;
    float lastFireTime;
    private float rofUpgrade = 1;
    private float rofModifier = 1;

    enum WeaponType
    {
        Unguided = 0,
        Guided
    }
    
    private void Awake()
    {
        projectilePool = GetComponent<ObjectPool>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastFireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null && target.activeSelf)
            Fire();
    }
    
    public void SetTarget(GameObject newTarget)
    {
        if (newTarget != null)
        {
            target = newTarget;
        }
        else
        {
            target = null;
        }
    }

    void Fire()
    {
        if(IsTargetInLOS())
        {
            if ((Time.time - lastFireTime) > ((turretSO.fireRate * rofModifier) * rofUpgrade))
            {
                StartCoroutine(FireRoutine());
            }
        }
    }


    protected virtual IEnumerator FireRoutine()
    {
        lastFireTime = Time.time;

        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject projectile = projectilePool.GetPooledObject(); 
            if (projectile != null) {
                projectile.transform.position = spawnPoint.position;
                projectile.transform.rotation = spawnPoint.rotation;
                projectile.SetActive(true);

                if (weaponType == WeaponType.Guided && target != null)
                {
                    if(target != null)
                        projectile.GetComponent<ProjectileHomingMove>().SetTarget(target);
                }
            }
            
            // Instantiate(turretSO.projectileSO.projectilePrefab, spawnPoint.position, platformTurret.rotation);
            
            // if (turretSO.projectileSO.dischargePrefab != null)
            //     Instantiate(turretSO.projectileSO.dischargePrefab, spawnPoint.position,
            //         Quaternion.identity);
            
            // Instantiate(turretSO.projectileSO.projectilePrefab,  spawnPoint.position, spawnPoint.rotation);


            if (turretSO.dischargePrefab != null)
            {
                GameObject muzzle = Instantiate(turretSO.dischargePrefab, spawnPoint.position,
                    spawnPoint.rotation);
                Destroy(muzzle, .1f);
            }

            if(turretSO.fireSFX != null)
                if(AudioManager.instance != null)
                    AudioManager.instance.PlaySound(turretSO.fireSFX, transform.position);
            
            fireFeedbacks?.PlayFeedbacks();
            
            yield return new WaitForSeconds(turretSO.barrelFireDelay);
        }
    }
    
    bool IsTargetInLOS()
    {
        if(!requireLOS)
            return true;
        
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * turretSO.engageRange,
                out RaycastHit hit, turretSO.engageRange, turretSO.targetLayers))
        {
            if (hit.collider.gameObject.layer == target.layer)
            {
                return true;
            }
        }
        else
        {
            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * turretSO.engageRange,
                    out RaycastHit asteroidHit, turretSO.engageRange, turretSO.asteroidLayer))
            {
                if (asteroidHit.collider.gameObject.layer == target.layer)
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, turretSO.engageRange);

            if (requireLOS)
                Gizmos.DrawRay(raycastOrigin.position, raycastOrigin.forward * 50);
        }
    }

    public void Upgrade()
    {
        rofUpgrade -= turretSO.rofUpgradeAmount;
    }

    public void AdjustRateOfFire(float rate)
    {
        rofModifier = rate;
    }
}
