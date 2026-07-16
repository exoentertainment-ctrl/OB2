using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISelectedShip : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private TMP_Text shipName;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider shieldSlider;
    [SerializeField] private GameObject behaviorToggle;
    [SerializeField] GameObject upgradeButton;
    [SerializeField] private Image profileImage;
    [SerializeField] private GameObject specialButton;

    #endregion
    
    GameObject selectedShip;
    bool isShipSelected;

    public static UISelectedShip instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
    
    private void Update()
    {
        CheckSelectedShip();  
        UpdateHealth();
        UpdateShield();
    }

    void CheckSelectedShip()
    {
        if (selectedShip == null && isShipSelected)
        {
            isShipSelected = false;

            shipName.text = "";
            healthSlider.value = 1;
            shieldSlider.value = 1;
            profileImage.sprite = null;
        }
    }
    
    public void SetSelectedShip(GameObject ship)
    {
        selectedShip = ship;
        isShipSelected = true;
        
        selectedShip.TryGetComponent<UpgradeManager>(out UpgradeManager upgradeManager);

        if (upgradeManager != null)
            shipName.text = ship.name + " Lvl: " + selectedShip.GetComponent<UpgradeManager>().GetUpgradeLevel();
        else
            shipName.text = ship.name;
        
        selectedShip.TryGetComponent<ShipController>(out ShipController shipController);
        profileImage.sprite = shipController?.GetProfileImage();

        if (selectedShip.CompareTag("Capital Ship"))
        {
            behaviorToggle.SetActive(false);
            upgradeButton.SetActive(false);
        }
        else
        {
            behaviorToggle.SetActive(true);
            upgradeButton.SetActive(true);
        }
        
        behaviorToggle.GetComponent<BehaviorToggle>().SetBehavior(selectedShip.GetComponent<ShipMovement>().GetBehavior());
        
        if(selectedShip.GetComponent<UpgradeManager>().IsSpecialActive() == true)
            specialButton.SetActive(true);
        else
            specialButton.SetActive(false);
    }

    void UpdateHealth()
    {
        if(selectedShip != null)
            healthSlider.value = selectedShip.GetComponent<IHealth>().GetHealth();
    }

    void UpdateShield()
    {
        if(selectedShip != null)
            shieldSlider.value =  selectedShip.GetComponentInChildren<ShieldHealth>().GetShield();
    }
    
    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }
    
    public void SetShield(float shield)
    {
        shieldSlider.value = shield;
    }

    public void SetProfileImage(Sprite image)
    {
        profileImage.sprite = image;
    }
    
    public void ChangeBehavior()
    {
        if (selectedShip != null)
        {
            selectedShip.TryGetComponent<ShipMovement>(out ShipMovement shipMovement);
            shipMovement?.ChangeBehavior();
            
            if(AudioManager.instance != null)
                AudioManager.instance.PlayUIClick();
        }
    }

    public void UpgradeShip()
    {
        selectedShip.TryGetComponent<UpgradeManager>(out UpgradeManager upgradeManager);

        if (upgradeManager != null)
        {
            upgradeManager?.GetComponent<UpgradeManager>().Upgrade();
            shipName.text = selectedShip.name + " Lvl: " +
                            selectedShip.GetComponent<UpgradeManager>().GetUpgradeLevel();
        }
    }

    public void ActivateSpecialButton(GameObject ship)
    {
        if(ship == selectedShip)
            specialButton.SetActive(true);
    }
}
