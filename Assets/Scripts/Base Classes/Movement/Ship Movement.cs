using System;
using MoreMountains.Tools;
using UnityEngine;
using Random = UnityEngine.Random;
//using Sirenix.OdinInspector;

public class ShipMovement : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private bool isPlayer;
    [SerializeField] protected BaseShipSO shipSO;
    [SerializeField] private GameObject engines;

    [Tooltip("This is the step size for the raycast that looks for detour routes")] 
    [SerializeField] private int radarStepAmount;

    [SerializeField] protected bool showGizmos;
    
    #endregion

    #region Variables

    private int movementLayerOffset;
    protected GameObject targetObject;
    protected Vector3 destinationPos;
    private Vector3 detourPos;

    protected bool isMovingToDestination;
    protected bool isMovingToTarget;
    protected bool isMovingToDetour;
    protected bool isAggressive = true;

    private int radarAngleX = -45;
    private int radarAngleY = -45;

    private float movementModifier = 1;
    
    #endregion

    protected void Start()
    {
        if(GameObject.FindGameObjectWithTag("Movement Layer") != null)
            movementLayerOffset = GameObject.FindGameObjectWithTag("Movement Layer").GetComponent<InteractionLayer>().ReturnOffset();
    }

    protected void Update()
    {
        if (targetObject == null && isAggressive && !isMovingToDestination && !isMovingToDetour)
        {
            DeactivateEngines();
            SearchForTarget();
        }
        
        if (isMovingToDestination)
        {
            RotateTowardsDestination();
            CheckDistanceToDestination();
            MoveShip();
        }
        else if (isMovingToDetour)
        {
            RotateTowardsDetour();
            CheckDistanceToDetour();
            MoveShip();
        }
    }

    protected void FixedUpdate()
    {
        CheckPathAhead();
    }

    //If either a standard move or move to target is set
    protected void MoveShip()
    {
        //if(isAggressive)
            transform.position += transform.forward * ((shipSO.moveSpeed * movementModifier)* Time.deltaTime);
    }

    #region Rotating Methods

    protected void RotateTowardsDetour()
    {
        Vector3 targetVector = detourPos - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);


        transform.rotation =
            Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
    }

    protected virtual void RotateTowardsDestination()
    {
        Vector3 targetVector = destinationPos - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);

        transform.rotation =
            Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
    }
    
    #endregion

    //Set the destination position for a move order
    public void SetDestinationPos(Vector3 pos)
    {
        destinationPos = new Vector3(pos.x, pos.y + movementLayerOffset, pos.z);
        isMovingToDestination = true;
        isMovingToTarget = false;
        ActivateEngines();
    }

    #region Distance Checking

    protected void CheckDistanceToDetour()
    {
        if (Vector3.Distance(transform.position, detourPos) <= 1)
        {
            isMovingToDetour = false;
            
            if(targetObject == null)
                isMovingToDestination = true;
        }
    }
    
    protected void CheckDistanceToDestination()
    {
        if (Vector3.Distance(transform.position, destinationPos) <= 1)
        {
            isMovingToDestination = false;
            DeactivateEngines();
        }
    }
    
    #endregion
    
    //Receive target from ship controller, set new position around target
    public virtual void SetTarget(GameObject target)
    {
        if (target != null)
        {
            isMovingToDestination = false;
            isMovingToTarget = true;

            targetObject = target;

            ActivateEngines();
        }
    }

    void CheckPathAhead()
    {
        Physics.Raycast(transform.position, transform.forward * shipSO.lookAheadDistance, out RaycastHit hitInfo, shipSO.lookAheadDistance, shipSO.obstacleLayerMask);
        
        if(hitInfo.collider != null)
        {
            FindDetour();
        }
    }

    //Scan a 45 degree cone in front of the ship. If it finds a spot that doesn't have an object in the way then it sets the detour position
    void FindDetour()
    {
        bool detourFound  = false;

        while (!detourFound)
        {
            Physics.Raycast(transform.position, Quaternion.Euler(radarAngleX, radarAngleY, 0) * transform.forward, out RaycastHit hitInfo, shipSO.lookAheadDistance, shipSO.obstacleLayerMask);

            if (hitInfo.collider == null)
            {
                detourPos = transform.position + Quaternion.Euler(radarAngleX, radarAngleY, 0) * (transform.forward *
                    shipSO.lookAheadDistance);
                isMovingToDestination = false;
                isMovingToTarget = false;
                isMovingToDetour = true;

                break;
            }
            
            radarAngleX++;
            if (radarAngleX >= 45)
            {
                radarAngleX = -45;
                radarAngleY++;
                if (radarAngleY >= 45)
                    radarAngleY = -45;
                
                //Should we stop the ship if it reaches the bottom right scan area without finding a detour?
            }
        }
    }

    void SearchForTarget()
    {
        if (GameObject.FindGameObjectsWithTag("Priority Target").Length > 0 && !isPlayer)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Priority Target");

            if (targets.Length > 0)
            {
                SetTarget(targets[Random.Range(0, targets.Length - 1)]);
            }
        }
        else
        {
            Collider[] possibleTargets =
                Physics.OverlapSphere(transform.position, Mathf.Infinity, shipSO.targetLayerMask);
            GameObject potentialTarget = null;

            if (possibleTargets.Length > 0)
            {
                float closestEnemy = Mathf.Infinity;

                for (int x = 0; x < possibleTargets.Length; x++)
                {
                    float distanceToEnemy =
                        Vector3.Distance(possibleTargets[x].transform.position, transform.position);

                    if (distanceToEnemy < closestEnemy)
                    {
                        closestEnemy = distanceToEnemy;

                        potentialTarget = possibleTargets[x].transform.root.gameObject;
                    }
                }

                if (potentialTarget != null)
                    SetTarget(potentialTarget);
            }
        }
    }
    
    protected void ActivateEngines()
    {
        engines?.SetActive(true);
    }
    
    void DeactivateEngines()
    {
        engines?.SetActive(false);
    }

    public void ChangeBehavior()
    {
        isAggressive = !isAggressive;

        if (!isAggressive)
        {
            DeactivateEngines();
            SetTarget(null);
        }
    }
    
    public bool GetBehavior()
    {
        return isAggressive;
    }

    public void AdjustMovementModifier(float modifier)
    {
        movementModifier = modifier;
    }
}
