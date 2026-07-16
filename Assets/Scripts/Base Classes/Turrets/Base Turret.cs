using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseTurret : MonoBehaviour
{
    #region Serialized Fields

    [Header("Components")] 
    [SerializeField] private bool requireLOS;
    [SerializeField] private Transform raycastOrigin;
    
    [SerializeField] TurretSO turretSO;

    [SerializeField] private int targetResetInterval;
    
    [SerializeField] private bool showGizmos;

    #endregion

    #region Variables And Properties

    private float lastTargetResetTime;
    private bool isPrimarySet;
    private bool isSecondarySet;
    private GameObject primaryTarget;
    private GameObject secondaryTarget;
    private GameObject currentTarget;
    GameObject lastTarget;

    #endregion

    private TurretRotation turretRotation;
    private TurretAttack turretAttack;

    private float lastTimeOnTarget;

    private void Awake()
    {
        turretRotation = GetComponent<TurretRotation>();
        turretAttack = GetComponent<TurretAttack>();
    }

    private void Update()
    {
        if (currentTarget == null || !currentTarget.activeSelf)
        {
            SearchForTarget();
        }
        else
        {
            CheckDistanceToTargets();
            
            if(requireLOS)
                CheckLOS();
        }
        
        CheckTargetResetTime();
    }

    //Receive selected target from turret controller and pass it along to the turret's components
    public void SetPrimaryTarget(GameObject target)
    {
        primaryTarget = target;
        currentTarget = primaryTarget;
        lastTargetResetTime = Time.time;

        turretRotation?.SetTarget(currentTarget);
        turretAttack?.SetTarget(currentTarget);
    }

    void SearchForTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, turretSO.engageRange, turretSO.targetLayers);

        if (possibleTargets.Length > 0)
        {
            float closestEnemy = Mathf.Infinity;

            for (int x = 0; x < possibleTargets.Length; x++)
            {
                float distanceToEnemy =
                    Vector3.Distance(possibleTargets[x].transform.position, transform.position);

                // if (lastTarget != possibleTargets[x].transform.root.gameObject)
                // {
                    if (distanceToEnemy < closestEnemy && distanceToEnemy >  turretSO.minEngageRange)
                    {
                        closestEnemy = distanceToEnemy;
                        
                        secondaryTarget = possibleTargets[x].transform.root.gameObject;
                    }
                //}
            }

            //Set secondary target and pass it along to the turret's components
            currentTarget = secondaryTarget;
            //lastTarget = currentTarget;
            lastTimeOnTarget = Time.time;
            lastTargetResetTime = Time.time;
            turretRotation?.SetTarget(currentTarget);
            turretAttack?.SetTarget(currentTarget);
        }
    }

    void CheckDistanceToTargets()
    {
        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

            //If the distance to current target is beyond the turret's range
            if (distanceToTarget > turretSO.engageRange || distanceToTarget < turretSO.minEngageRange)
            {
                //If the current target is the secondary target then set secondary to null
                if (currentTarget == secondaryTarget)
                    secondaryTarget = null;

                currentTarget = null;
                turretRotation?.SetTarget(null);
                turretAttack?.SetTarget(null);
            }
        }

        //If the turret has a primary target set but is not currently targeting it, check distance to primary to see if it's come into range
        if (primaryTarget != null && currentTarget != primaryTarget)
        {
            float distanceToTarget = Vector3.Distance(transform.position, primaryTarget.transform.position);

            if (distanceToTarget < turretSO.engageRange)
            {
                lastTimeOnTarget = Time.time;
                currentTarget = primaryTarget;
            }
        }
    }

    void CheckLOS()
    {
        if (currentTarget != null)
        {
            // if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * turretSO.engageRange,
            //         out RaycastHit hit, turretSO.engageRange, turretSO.targetLayers))
            // {
            //     if (hit.collider.gameObject.layer == currentTarget.layer)
            //     {
            //         lastTimeOnTarget = Time.time;
            //     }
            // }
            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * turretSO.engageRange,
                    out RaycastHit hit, turretSO.engageRange, turretSO.targetLayers))
            {
                if (hit.collider.gameObject.layer == currentTarget.layer)
                {
                    lastTimeOnTarget = Time.time;
                }
            }
            else if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * turretSO.engageRange,
                         out RaycastHit asteroidHit, turretSO.engageRange, turretSO.asteroidLayer))
            {
                if (asteroidHit.collider.gameObject.layer == currentTarget.layer)
                {
                    lastTimeOnTarget = Time.time;
                }
            }
            else
            {
                if ((Time.time - lastTimeOnTarget) >= 5)
                {
                    lastTarget = currentTarget;
                    
                    if (currentTarget == secondaryTarget)
                        secondaryTarget = null;

                    currentTarget = null;
                    turretRotation?.SetTarget(null);
                    turretAttack?.SetTarget(null);
                }
            }
        }
    }

    void CheckTargetResetTime()
    {
        if ((Time.time - lastTargetResetTime) >= targetResetInterval)
        {
            lastTargetResetTime = Time.time;
            lastTarget = null;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, turretSO.engageRange);
            
            Gizmos.DrawWireSphere(transform.position, turretSO.minEngageRange);
            
            Gizmos.DrawRay(raycastOrigin.position, raycastOrigin.forward * turretSO.engageRange);
        }
    }
}
