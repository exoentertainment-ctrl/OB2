using System;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class SpecialWeaponHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] TMP_Text text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionWindow.SetActive(true);

        text.text = ShipManager.instance.GetSpecialWeaponDescription();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionWindow.SetActive(false);
    }
    
    
}
