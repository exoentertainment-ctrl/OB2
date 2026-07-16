using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlatformUpgradeWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] TMP_Text upgradeText;
    [SerializeField] TMP_Text sellText;
    
    [SerializeField] float activationDelay;
    [SerializeField] private int sellDiscount;
    
    private GameObject selectedPlatform;
    private bool isActive;

    private void OnEnable()
    {
        StartCoroutine(ActivationDelayRoutine());
    }

    private void OnDisable()
    {
        isActive = false;
    }

    public void SetPlatform(GameObject platform)
    {
        selectedPlatform = platform;
    }

    public void Upgrade()
    {
        if (isActive)
        {
            selectedPlatform.GetComponent<UpgradeManager>().Upgrade();
            gameObject.SetActive(false);
        }
    }

    public void Sell()
    {
        if (isActive)
        {
            ResourceManager.instance.IncreaseCredits(selectedPlatform.GetComponent<UpgradeManager>().GetUpgradeCost()/sellDiscount);
            Destroy(selectedPlatform);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionWindow.SetActive(true);
        //descriptionWindow.transform.position =  Input.mousePosition;

        if (selectedPlatform.GetComponent<UpgradeManager>().GetUpgradeCost() == 0)
        {
            upgradeText.text = "At max upgrade.";
        }
        else
            upgradeText.text = "Upgrade Cost: " + selectedPlatform.GetComponent<UpgradeManager>().GetUpgradeCost() + " credits";
        
        sellText.text = "Sell Cost: " + selectedPlatform.GetComponent<UpgradeManager>().GetUpgradeCost()/sellDiscount +  " credits";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionWindow.SetActive(false);
    }
    
    IEnumerator ActivationDelayRoutine()
    {
        yield return new WaitForSeconds(activationDelay);
        
        isActive = true;
    }
}
