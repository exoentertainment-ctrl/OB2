using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private UnityEvent upgradeListeners;
    [SerializeField] UnityEvent maxUpgradeListeners;
    [SerializeField] private int maxUpgrades;
    [SerializeField] int upgradeCostMultiplier;

    #endregion

    private int numUpgrades = 1;
    private bool isUpgradeable;
    
    public void Upgrade()
    {
        if (numUpgrades <= maxUpgrades)
        {
            if (ResourceManager.instance.GetCredits() >= upgradeCostMultiplier * numUpgrades)
            {
                ResourceManager.instance.DecreaseCredits(upgradeCostMultiplier * numUpgrades);
                numUpgrades++;
                upgradeListeners?.Invoke();

                if(AudioManager.instance != null)
                    AudioManager.instance.PlayUISelect();
            }
            else
            {
                if(AudioManager.instance != null)
                    AudioManager.instance.PlayInsufficientCredits();
            }
        }
        else if (numUpgrades == maxUpgrades + 1 && !isUpgradeable)
        {
            if (ResourceManager.instance.GetCredits() >= upgradeCostMultiplier * numUpgrades)
            {
                ResourceManager.instance.DecreaseCredits(upgradeCostMultiplier * numUpgrades);
                isUpgradeable = true;
                UISelectedShip.instance.ActivateSpecialButton(gameObject);
                maxUpgradeListeners?.Invoke();

                if(AudioManager.instance != null)
                    AudioManager.instance.PlayUISelect();
            }
            else
            {
                if(AudioManager.instance != null)
                    AudioManager.instance.PlayInsufficientCredits();
            }
        }
        else
        {
            if(AudioManager.instance != null)
                AudioManager.instance.PlayUIError();
        }
    }

    public bool IsSpecialActive()
    {
        if(numUpgrades ==  maxUpgrades + 1)
            return true;
        else
            return false;
    }

    public int GetUpgradeLevel()
    {
        return numUpgrades;
    }

    public int GetUpgradeCost()
    {
        if(numUpgrades <= maxUpgrades + 1)
            return numUpgrades *  upgradeCostMultiplier;
        else
        {
            return 0;
        }
    }
    
    public void WeaponUpgrade()
    {
        
    }

    public void DefenseUpgrade()
    {
        
    }

    public void UtilityUpgrade()
    {
        
    }
}
