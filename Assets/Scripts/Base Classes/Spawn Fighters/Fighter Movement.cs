using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FighterMovement : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private BaseShipSO shipSO;
    [SerializeField] private int lifetime;

    [Tooltip("This is the step size for the raycast that looks for detour routes")] 
    [SerializeField] private int radarStepAmount;
    
    [SerializeField] private int minOrbitRadius;
    [SerializeField] private int maxOrbitRadius;

    [SerializeField] private bool showGizmos;
    
    #endregion

    private GameObject parentObject;
    private GameObject targetObject;
    private Vector3 targetPos;
    private bool returnToBase;
    private bool isReturningToBase;
    private float spawnTime;
    
    private void OnEnable()
    {
        spawnTime = Time.time;
    }
    
    private void Start()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (!returnToBase)
        {
            CheckLifeTime();
            
            if (targetObject != null)
            {
                RotateTowardsTargetPos();
                CheckDistanceToTargetPos();
                MoveShip();
            }
            else
            {
                SetNewTarget();
            }
        }
        else
        {
            RotateTowardsParent();
            CheckDistanceToParent();
            MoveShip();
        }
        
        MoveShip();
    }
    
    void CheckLifeTime()
    {
        if (Time.time - spawnTime > lifetime)
        {
            returnToBase = true;
        }
    }

    public void SetTarget(GameObject target)
    {
        if (target != null)
        {
            targetObject = target;
            
            Vector3 randomSpot = (Random.insideUnitSphere * Random.Range(minOrbitRadius, maxOrbitRadius));
            targetPos = randomSpot + target.transform.position;
        }
    }

    void SetNewTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, shipSO.targetLayerMask);
        GameObject newTarget = null;
        
        float closestEnemy = Mathf.Infinity;

        for (int x = 0; x < possibleTargets.Length; x++)
        {
            float distanceToEnemy =
                Vector3.Distance(possibleTargets[x].transform.position, transform.position);
            
            if (distanceToEnemy < closestEnemy )
            {
                closestEnemy = distanceToEnemy;
                newTarget = possibleTargets[x].gameObject;
            }
        }

        if (newTarget != null)
        {
            targetObject = newTarget;
            SetNewTargetPos();
        }
    }
    
    //Assign a new random spot around the target
    void SetNewTargetPos()
    {
        Vector3 randomSpot = (Random.insideUnitSphere * Random.Range(minOrbitRadius, maxOrbitRadius));
        targetPos = randomSpot + targetObject.transform.position;
    }
    
    void RotateTowardsTargetPos()
    {
        Vector3 targetVector = targetPos - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);


        transform.rotation =
            Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
    }
    
    void CheckDistanceToTargetPos()
    {
        if (Vector3.Distance(transform.position, targetPos) <= 1)
            SetNewTargetPos();
    }
    
    //If either a standard move or move to target is set
    void MoveShip()
    {
        transform.position += transform.forward * (shipSO.moveSpeed * Time.deltaTime);
    }

    public void SetBase(GameObject baseObject)
    {
        parentObject =  baseObject;
    }

    public void ReturnToParent()
    {
        returnToBase = true;    
    }

    void RotateTowardsParent()
    {
        Vector3 targetVector = parentObject.transform.position - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);


        transform.rotation =
            Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
    }

    void CheckDistanceToParent()
    {
        if (Vector3.Distance(transform.position, parentObject.transform.position) <= 1)
            gameObject.SetActive(false);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            //Debug.DrawRay(transform.position, transform.forward * shipSO.lookAheadDistance, Color.blue);
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, minOrbitRadius);
            Gizmos.DrawWireSphere(transform.position, maxOrbitRadius);
        }
    }
}
