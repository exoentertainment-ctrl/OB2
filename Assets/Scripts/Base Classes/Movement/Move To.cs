using System;
using UnityEngine;

public class MoveTo : ShipMovement
{
    #region --Serialize Field--
    
    [SerializeField] Transform endPos;

    #endregion

    void Start()
    {
        isMovingToDestination = true;
    }
    
    // Update is called once per frame
    void Update()
    {
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
        
        MoveShip();
        RotateTowardsDestination();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    protected override void RotateTowardsDestination()
    {
        Vector3 targetVector = endPos.position - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);


        transform.rotation =
            Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
    }
}
