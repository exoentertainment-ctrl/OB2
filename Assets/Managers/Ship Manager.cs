using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] UISelectedShip uiSelectedShip;

    #endregion

    GameObject[] shipPool = new GameObject[2];
    int shipPoolIndex;
    private GameObject selectedShip;

    public static ShipManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    //Input manager calls this method and passes along the clicked on ship. Manager stores the passed object locally
    public void SetSelectedShip(GameObject ship)
    {
        if (selectedShip != ship && selectedShip != null)
        {
            selectedShip.GetComponent<ShipController>().ShipDeselected();
            selectedShip.GetComponent<ShipHealth>().SetSelected();
        }

        selectedShip = ship.transform.root.gameObject;
        selectedShip?.GetComponent<ShipHealth>().SetSelected();
        selectedShip?.GetComponentInChildren<ShieldHealth>().SetSelected();
        selectedShip?.GetComponent<ShipController>().PlaySelectedSound();

        //Send ship info to UI
        uiSelectedShip?.SetSelectedShip(selectedShip);

        //Turn on circular selected VFX around ship
        selectedShip?.GetComponent<ShipController>().ShipSelected();
    }

    //Send movement coordinates to selected ship
    public void SetMovePos(Vector3 movePos)
    {
        if (selectedShip != null)
        {
            //Send move coordinates to selected ship
            selectedShip.GetComponent<ShipController>().SetDestinationPos(movePos);
        }
    }

    public bool IsShipSelected()
    {
        if (selectedShip == null)
            return false;
        else
            return true;
    }

    public void SetTarget(GameObject target)
    {
        selectedShip.GetComponent<ShipController>().SetTarget(target);
    }

    public GameObject GetSelectedShip()
    {
        return selectedShip;
    }

    public string GetSpecialWeaponDescription()
    {
        if (selectedShip != null)
            return selectedShip.GetComponent<TurretController>().GetSpecialWeaponDescription();
        else
            return null;
    }
    
    public void SetShipPool()
    {
        GameObject[] allShips =  GameObject.FindGameObjectsWithTag("Player");
        
        for (int x = 0; x < allShips.Length; x++)
        {
            shipPool[x] = allShips[x].transform.root.gameObject;
        }
    }

    public void SelectNextShip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            shipPoolIndex++;
            if (shipPoolIndex >= shipPool.Length)
                shipPoolIndex = 0;

            if (shipPool[shipPoolIndex] != null)
                SetSelectedShip(shipPool[shipPoolIndex]);
        }
    }

    public void SelectPreviousShip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            shipPoolIndex--;
            if (shipPoolIndex < 0)
                shipPoolIndex = shipPool.Length - 1;

            if (shipPool[shipPoolIndex] != null)
                SetSelectedShip(shipPool[shipPoolIndex]);
        }
    }

    public void PlayMoveSound()
    {
        selectedShip.GetComponent<ShipController>().PlayMoveSound();
    }

    public void PlayTargetSetSound()
    {
        selectedShip.GetComponent<ShipController>().PlayTargetSetSound();    
    }

    public void ActivateSpecialWeapon()
    {
        selectedShip.GetComponent<ShipController>().ActivateSpecialWeapon();    
    }
    
    public void UpdateUIHealthBar(float health)
    {
        
    }

    public void UpdateUIShieldBar(float shield)
    {
        
    }
}
