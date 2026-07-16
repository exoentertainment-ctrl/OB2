using UnityEngine;

public class MoveAround : ShipMovement
{
    //[BoxGroup("Move Around Variables")]
    //[ShowIfGroup("Move Around Variables/movementBehavior", Value = MovementBehavior.MoveAround)]
    [SerializeField] private int minOrbitRadius;
    //[ShowIfGroup("Move Around Variables/movementBehavior", Value = MovementBehavior.MoveAround)]
    [SerializeField] private int maxOrbitRadius;
    
    private Vector3 targetPos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
        // if (targetObject != null && !isMovingToDetour && !isMovingToDestination && isAggressive)
        if (targetObject != null && !isMovingToDetour && !isMovingToDestination)
        {
            RotateTowardsTargetPos();
            CheckDistanceToTargetPos();
            MoveShip();
        }
    }
    
    private void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    //Receive target from ship controller, set new position around target
    public override void SetTarget(GameObject target)
    {
        if (target != null)
        {
            isMovingToDestination = false;
            isMovingToTarget = true;

            targetObject = target;
            SetNewTargetPos();

            ActivateEngines();
        }
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
    
    //Assign a new random spot around the target
    void SetNewTargetPos()
    {
        Vector3 randomSpot = (Random.insideUnitSphere * Random.Range(minOrbitRadius, maxOrbitRadius));
        
        if(randomSpot.y < -10)
            randomSpot.y = -10;
        
        targetPos = randomSpot + targetObject.transform.position;
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
