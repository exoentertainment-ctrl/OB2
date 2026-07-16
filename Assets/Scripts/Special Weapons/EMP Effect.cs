using UnityEngine;

public class EMPEffect : MonoBehaviour
{
    #region --Serialize Field--

    [SerializeField] private int duration;

    #endregion
    
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
        
        if(shipMovement != null)
            shipMovement.enabled = false;

        TurretAttack[] turretAttack = transform.root.GetComponentsInChildren<TurretAttack>();

        foreach (TurretAttack turret in turretAttack)
        {
            turret.enabled = false;
        }
    }
    
    void CheckDuration()
    {
        if (Time.time - beginTime > duration)
        {
            ShipMovement shipMovement = GetComponentInParent<ShipMovement>();
        
            if(shipMovement != null)
                shipMovement.enabled = true;

            TurretAttack[] turretAttack = GetComponentsInChildren<TurretAttack>();

            foreach (TurretAttack turret in turretAttack)
            {
                turret.enabled = true;
            }
        }
    }
}
