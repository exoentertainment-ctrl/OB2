using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
using UnityEngine.Events;

public class ShipUpgrader : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private List<UpgradeType> upgrades;
    [SerializeField] private UnityEvent onUpgrade;

    #endregion

    #region Variables

    private int currentUpgradeLevel;

    public enum UpgradeType
    {
        UpgradeStat,
        UpgradeObject
    }
    
    #endregion
}
