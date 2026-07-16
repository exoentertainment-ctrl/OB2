using UnityEngine;

public class TurretUtility : MonoBehaviour
{
    [SerializeField] private UtilitySO utilitySO;
    [SerializeField] private bool isChild;
    [SerializeField] private bool showGizmos;
    
    private GameObject target;
    float lastFireTime;
    private float rofUpgrade = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastFireTime = Time.time;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(target == null)
            SearchForTarget();
        
        if(target != null && target.activeSelf)
            Fire();
    }

    void Fire()
    {
        if ((Time.time - lastFireTime) > (utilitySO.rateOfFire * rofUpgrade))
        {
            if(isChild)
                Instantiate(utilitySO.utilityEffect, target.transform);
            else
                Instantiate(utilitySO.utilityEffect, target.transform.position, Quaternion.identity);
            
            lastFireTime = Time.time;
            
            if (utilitySO.dischargePrefab != null)
                Instantiate(utilitySO.dischargePrefab, transform.position, Quaternion.identity);
        }
    }
    
    void SearchForTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, utilitySO.range, utilitySO.targetLayers);
        GameObject possibleTarget = null;

        if (possibleTargets.Length > 0)
        {
            float closestEnemy = Mathf.Infinity;

            for (int x = 0; x < possibleTargets.Length; x++)
            {
                float distanceToEnemy = Vector3.Distance(possibleTargets[x].transform.position, transform.position);
                
                    if (distanceToEnemy < closestEnemy)
                    {
                        closestEnemy = distanceToEnemy;
                        
                        possibleTarget = possibleTargets[x].transform.root.gameObject;
                    }
            }
        }

        if (possibleTarget != null)
            target = possibleTarget;
    }
    
    public void Upgrade()
    {
        rofUpgrade -= utilitySO.rofUpgradeAmount;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, utilitySO.range);
        }
    }
}
