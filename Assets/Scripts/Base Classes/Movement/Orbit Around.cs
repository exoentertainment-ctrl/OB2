using UnityEngine;
using MoreMountains.Tools;

public class OrbitAround : ShipMovement
{
    [SerializeField] MMAutoRotate autoRotate;
    [SerializeField] private int orbitDistance;
    //[ShowIfGroup("Orbit Variables/movementBehavior", Value = MovementBehavior.OrbitAround)]
    [SerializeField] private int orbitSpeed;
    
    private bool isOrbitting;
    
    private int orbitDirection = 1;
    private float orbitStep = .1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (targetObject == null)
        {
            if (autoRotate != null && autoRotate.isActiveAndEnabled)
            {
                autoRotate.OrbitCenterTransform = null;
                autoRotate.Orbiting = false;
                autoRotate.enabled = false;
            }
        }
        
        if (targetObject != null && !isMovingToDetour && !isMovingToDestination && isAggressive)
        {
            if (!isOrbitting)
            {
                RotateTowardsTargetPos();
                MoveShip();
                CheckDistanceToOrbitPoint();
            }
            else
            {
                PointTowardsTarget();
                ChangeOrbitAxis();
            }
        }
    }
    
    private void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    void RotateTowardsTargetPos()
    {
            Vector3 targetVector = targetObject.transform.position - transform.position;
            targetVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);
            
            transform.rotation =
                Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
    }
    
    void CheckDistanceToOrbitPoint()
    {
        if (Vector3.Distance(transform.position, targetObject.transform.position) <= orbitDistance)
        {
            autoRotate.enabled = true;
            autoRotate.Orbiting = true;
            autoRotate.OrbitCenterTransform = targetObject.transform;
            autoRotate.OrbitRadius = orbitDistance;
            
            isOrbitting = true;
        }
    }
    
    //Slowly adjust the orbit axis over time and switch the Z axis once it reaches (-)3
    void ChangeOrbitAxis()
    {
        autoRotate.OrbitRotationAxis.x += orbitStep * Time.deltaTime;
        autoRotate.OrbitRotationAxis.z += (orbitStep * Time.deltaTime) * orbitDirection;

        if (autoRotate.OrbitRotationAxis.z > 3 || autoRotate.OrbitRotationAxis.z < -3)
            orbitDirection *= -1;
    }
    
    //Keep the nose of the ship pointed towards the target
    void PointTowardsTarget()
    {
        transform.LookAt(targetObject.transform);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            //Debug.DrawRay(transform.position, transform.forward * shipSO.lookAheadDistance, Color.blue);
            Gizmos.color = Color.red;
            
            Gizmos.DrawWireSphere(transform.position, orbitDistance);
        }
    }
}
