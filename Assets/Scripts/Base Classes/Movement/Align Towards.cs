using UnityEngine;

public class AlignTowards : ShipMovement
{
    [SerializeField] private int minStandOffRange;
    [SerializeField] private int maxStandOffRange;
    [SerializeField] private TurretAttack[] fixedTurret;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (targetObject != null && !isMovingToDetour && !isMovingToDestination)
        {
            CheckDistanceToTargetObject();
        }
    }
    
    private void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    void CheckDistanceToTargetObject()
    {
        if (Vector3.Distance(transform.position, targetObject.transform.position) >= minStandOffRange && Vector3.Distance(transform.position, targetObject.transform.position) <= maxStandOffRange)
        {
            RotateTowardsTargetObject();
        }
        else
        {
            RotateTowardsTargetObject();
            MoveShip();
        }
    }
    
    void RotateTowardsTargetObject()
    {
        Vector3 targetVector = targetObject.transform.position - transform.position;

        if (Vector3.Distance(transform.position, targetObject.transform.position) <= minStandOffRange)
        {
            targetVector *= -1;
        }

        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);
        
        transform.rotation =
            Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
    }

    public override void SetTarget(GameObject target)
    {
        if (target != null)
        {
            isMovingToDestination = false;
            isMovingToTarget = true;

            targetObject = target;

            foreach (var turret in fixedTurret)
            {
                turret?.SetTarget(targetObject);
            }
            
            ActivateEngines();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            //Debug.DrawRay(transform.position, transform.forward * shipSO.lookAheadDistance, Color.blue);
            Gizmos.color = Color.red;
            
            Gizmos.DrawWireSphere(transform.position, minStandOffRange);
            Gizmos.DrawWireSphere(transform.position, maxStandOffRange);
        }
    }
}
