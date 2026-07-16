using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] TMP_Text text;
    
    int upgradeCost;
    
    private void Update()
    {
        if(gameObject.activeSelf && ShipManager.instance.GetSelectedShip() != null)
            upgradeCost = ShipManager.instance.GetSelectedShip().GetComponent<UpgradeManager>().GetUpgradeCost();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionWindow.SetActive(true);

        if (upgradeCost == 0)
        {
            text.text = "At max upgrade.";
        }
        else
            text.text = "Upgrade Cost: " + upgradeCost + " credits";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionWindow.SetActive(false);
    }
}
