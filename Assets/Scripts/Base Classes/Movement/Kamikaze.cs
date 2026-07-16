using System;
using UnityEngine;

public class Kamikaze : ShipMovement
{
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
            RotateTowardsTarget();
            MoveShip();
        }
    }
    
    private void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void RotateTowardsTarget()
    {
        Vector3 targetVector = targetObject.transform.position - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);


        transform.rotation =
            Quaternion.SlerpUnclamped(transform.rotation, targetRotation, shipSO.turnSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.root.gameObject == targetObject.transform.root.gameObject)
        {
            other.gameObject.TryGetComponent<IHealth>(out IHealth health);
            health?.TakeDamage(shipSO.maxHealth);
            
            if(shipSO.explosionPrefab  != null)
                Instantiate(shipSO.explosionPrefab, other.contacts[0].point, Quaternion.identity);
            
            if(AudioManager.instance != null)
                if(shipSO.finalExplosion != null)
                    AudioManager.instance.PlaySound(shipSO.finalExplosion, transform.position);
            
            Destroy(gameObject);
        }
    }
}
