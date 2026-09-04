using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BehaviorToggleHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] TMP_Text text;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionWindow.SetActive(true);

        if (ShipManager.instance.GetSelectedShip() != null)
        {
            if (ShipManager.instance.GetSelectedShip().GetComponent<ShipMovement>().GetBehavior())
                text.text = "Find new target";
            else
                text.text = "Don't find new target";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionWindow.SetActive(false);
    }
}
