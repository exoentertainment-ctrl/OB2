using System;
using System.Collections;
using UnityEngine;
using DamageNumbersPro;

public class ShieldHealth : BaseShield
{
    #region Serialized Fields

    [Header("Components")] 
    [SerializeField] private GameObject mainShield;
    [SerializeField] private GameObject lowShield;
    
    [SerializeField] BaseShipSO shipSO;
    [SerializeField] private DamageNumber numberPrefab;
    [SerializeField] private DamageNumber shieldDownPrefab;

    #endregion

    #region Variables
    
    UISelectedShip uiSelectedShip;
    private float currentShield;
    float shieldUpgrade;
    bool isShieldRecharging;
    private bool isHit;
    private bool isLowShield;
    private bool isSelected;
    private bool shieldDownSFX;
    private bool isShieldUp;
    
    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentShield = shipSO.maxShield;
        
        if(GameObject.FindGameObjectWithTag("UISelectedShip") != null)
            uiSelectedShip = GameObject.FindGameObjectWithTag("UISelectedShip").GetComponent<UISelectedShip>();
    }

    private void Update()
    {
        isHit = false;
        
        Recharge();
    }

    public override void TakeDamage(float damage)
    {
        if (!isHit)
        {
            isHit = true;
        
            currentShield -= damage;
            
            if (currentShield > 0)
            {
                if (numberPrefab != null)
                {
                    DamageNumber damageNumber = numberPrefab.Spawn(transform.position, damage);
                }
                
                if (currentShield < shipSO.maxShield * shipSO.lowShieldPercentage && !isLowShield)
                {
                    isLowShield = true;
                    mainShield.GetComponent<MeshCollider>().enabled = false;
                    mainShield.SetActive(false);
                    lowShield.SetActive(true);
                }
            }
            else
            {
                isShieldUp = false;
                StartCoroutine(ShieldDownRoutine());
                
                if (!shieldDownSFX)
                {
                    transform.root.TryGetComponent<ShipController>(out ShipController controller);
                    controller?.PlayShieldDownSound();
                    shieldDownSFX = true;
                }

                lowShield.GetComponent<MeshCollider>().enabled = false;
                lowShield.SetActive(false);
                
                if (numberPrefab != null)
                {
                    DamageNumber damageNumber = numberPrefab.Spawn(transform.position, "Shield Down");
                }
            }
        }
    }

    public float GetHealth()
    {
        return 0;
    }

    void Recharge()
    {
        if (isShieldUp)
        {
            if (currentShield < shipSO.maxShield)
            {
                currentShield += shipSO.maxShield * (shipSO.shieldRechargeRate * Time.deltaTime);
            }

            if (currentShield > shipSO.maxShield * shipSO.lowShieldPercentage && isLowShield)
            {
                shieldDownSFX = false;
                isLowShield = false;
                lowShield.SetActive(false);
                mainShield.SetActive(true);
            }
        }
    }
    
    public void SetSelected()
    {
        isSelected = !isSelected;
    }

    public float GetShield()
    {
        return currentShield/shipSO.maxShield;
    }
    
    public void Upgrade()
    {
        shieldUpgrade += shipSO.maxShield * shipSO.upgradeAmount;
        currentShield = shipSO.maxHealth + shieldUpgrade;
    }

    IEnumerator ShieldDownRoutine()
    {
        yield return new WaitForSeconds(shipSO.shieldDownDuration);
        
        isShieldUp = true;
    }
}
