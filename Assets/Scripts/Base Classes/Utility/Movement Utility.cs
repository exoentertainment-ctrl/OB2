using System;
using UnityEngine;

public class MovementUtility : MonoBehaviour
{
    [SerializeField] UtilitySO utilitySO;
    
    private float beginTime;
    
    private void Start()
    {
        transform.position = transform.root.position;
        beginTime = Time.time;
        ActivateEffect();
    }

    private void Update()
    {
        CheckDuration();
    }

    void ActivateEffect()
    {
        ShipMovement shipMovement = GetComponentInParent<ShipMovement>();
        shipMovement?.AdjustMovementModifier(utilitySO.modifier);
    }
    
    void CheckDuration()
    {
        if (Time.time - beginTime > utilitySO.duration)
        {
            ShipMovement shipMovement = GetComponentInParent<ShipMovement>();
            shipMovement?.AdjustMovementModifier(1);
            Destroy(gameObject);
        }
    }
}
