using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ShipSelectionHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Serialized Fields
    
    [Header("Components")] 
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] string descriptionText;
    [SerializeField] TextMeshProUGUI descriptionTextWindow;

    #endregion


    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionWindow.SetActive(true);
        SetDescriptionText();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionWindow.SetActive(false);
    }

    void SetDescriptionText()
    {
        descriptionTextWindow.text = descriptionText;
    }
}
