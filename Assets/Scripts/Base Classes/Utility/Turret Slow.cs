using UnityEngine;

public class TurretSlow : MonoBehaviour
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
        TurretAttack[] turretAttack = transform.root.GetComponentsInChildren<TurretAttack>();

        foreach (TurretAttack turret in turretAttack)
            turret?.AdjustRateOfFire(utilitySO.modifier);
    }
    
    void CheckDuration()
    {
        if (Time.time - beginTime > utilitySO.duration)
        {
            TurretAttack[] turretAttack = transform.root.GetComponentsInChildren<TurretAttack>();
            
            foreach (TurretAttack turret in turretAttack)
                turret?.AdjustRateOfFire(1);
            
            Destroy(gameObject);
        }
    }
}
