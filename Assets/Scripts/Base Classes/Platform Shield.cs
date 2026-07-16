using UnityEngine;

public class PlatformShield : BaseShield
{
    #region Serialized Fields

    [Header("Components")] 
    [SerializeField] private GameObject mainShield;
    [SerializeField] private GameObject lowShield;
    
    [SerializeField] PlatformScriptableObject platformSO;

    #endregion

    #region Variables
    
    private float currentShield;
    float shieldUpgrade;
    private bool isHit;
    private bool isLowShield;
    
    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentShield = platformSO.maxShield;
    }

    private void Update()
    {
        isHit = false;
    }
    
    public override void TakeDamage(float damage)
    {
        if (!isHit)
        {
            isHit = true;
        
            currentShield -= damage;
            
            if (currentShield > 0)
            {
                if (currentShield < platformSO.maxShield * platformSO.lowShieldPercentage && !isLowShield)
                {
                    isLowShield = true;
                    mainShield.GetComponent<Collider>().enabled = false;
                    mainShield.SetActive(false);
                    lowShield.SetActive(true);
                }
            }
            else
            {
                lowShield.GetComponent<MeshCollider>().enabled = false;
                lowShield.SetActive(false);
            }
        }
    }

    public float GetHealth()
    {
        return 0;
    }

    public float GetShield()
    {
        return 0;
    }
    
    public void Upgrade()
    {
        shieldUpgrade += platformSO.maxShield * platformSO.upgradeAmount;
        currentShield = platformSO.maxHealth + shieldUpgrade;
    }
}
