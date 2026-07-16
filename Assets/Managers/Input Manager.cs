using Forge3D;
using MystifyFX;
using MystifyFX.Demo;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] LayerMask leftMouseClickShipMask;
    [SerializeField] LayerMask leftMouseClickPlatformMask;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private LayerMask rightMouseClickMask;
    [SerializeField] LayerMask movementPlaneMask;

    [SerializeField] private GameObject movementPlane;
    
    [SerializeField] GameObject targetEffect;

    [SerializeField] GameObject platformUpgradeWindow;

    [SerializeField] private GameObject gameSlowText;
    
    #endregion
    
    //Left mouse click selects the ship at mouse cursor and sends to ship manager to store
    public void PrimaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit hitInfo;
            if (Physics.Raycast(screenRay, out hitInfo, Mathf.Infinity, leftMouseClickShipMask))
            {
                ShipManager.instance.SetSelectedShip(hitInfo.collider.transform.root.gameObject);
            }
            else if (Physics.Raycast(screenRay, out hitInfo, Mathf.Infinity, leftMouseClickPlatformMask))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    platformUpgradeWindow.SetActive(true);
                    platformUpgradeWindow.GetComponent<PlatformUpgradeWindow>().SetPlatform(hitInfo.collider.transform.root.gameObject);
                    platformUpgradeWindow.transform.position = new Vector3(Mouse.current.position.ReadValue().x + platformUpgradeWindow.GetComponent<RectTransform>().rect.width,  Mouse.current.position.ReadValue().y, 0);
                }
            }
        }
    }
    
    //Right mouse click sends coordinates of mouse click to ship manager to send to selected ship
    public void SecondaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit hitInfo;

            if (Physics.Raycast(screenRay, out hitInfo, Mathf.Infinity, enemyLayers))
            {
                if (hitInfo.collider != null)
                {
                    //movementPlane.GetComponent<InteractionLayer>().ActivateTargetEffect(movementPlaneHit.point);
                    ShipManager.instance.SetTarget(hitInfo.collider.gameObject);
                    ShipManager.instance.PlayTargetSetSound();
                    Vector3 targetEffectPosition = new  Vector3(hitInfo.collider.gameObject.transform.position.x, hitInfo.collider.gameObject.transform.position.y - 2, hitInfo.collider.gameObject.transform.position.z);
                    Instantiate(targetEffect, targetEffectPosition, targetEffect.transform.rotation);
                }
            }
            else if (Physics.Raycast(screenRay, out hitInfo, Mathf.Infinity, rightMouseClickMask))
            {
                if (hitInfo.collider.CompareTag("Movement Layer") && ShipManager.instance.IsShipSelected())
                {
                    movementPlane.GetComponent<InteractionLayer>().ActivateMovementEffect(hitInfo.point);
                    ShipManager.instance.SetMovePos(hitInfo.point);
                    ShipManager.instance.PlayMoveSound();
                }
            }
            // if (Physics.Raycast(screenRay, out hitInfo, Mathf.Infinity, rightMouseClickMask))
            // {
            //     if (hitInfo.collider.CompareTag("Movement Layer") && ShipManager.instance.IsShipSelected())
            //     {
            //         movementPlane.GetComponent<InteractionLayer>().ActivateMovementEffect(hitInfo.point);
            //         ShipManager.instance.SetMovePos(hitInfo.point);
            //     }
            //     else
            //     {
            //         Ray movementPlaneRay = new Ray(hitInfo.collider.gameObject.transform.position, -hitInfo.collider.gameObject.transform.up);
            //         RaycastHit movementPlaneHit;
            //
            //         if (Physics.Raycast(movementPlaneRay, out movementPlaneHit, Mathf.Infinity, movementPlaneMask) &&
            //             ShipManager.instance.IsShipSelected())
            //         {
            //             movementPlane.GetComponent<InteractionLayer>().ActivateTargetEffect(movementPlaneHit.point);
            //             ShipManager.instance.SetTarget(hitInfo.collider.gameObject);
            //         }
            //     }
            // }
        }
    }
    
    //Space bar slows/resumes game
    public void SlowGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0.25f;
                gameSlowText.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                gameSlowText.SetActive(false);
            }
        }
    }

    public void OpenPauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                GameObject window = (GameObject)Instantiate(Resources.Load("Pause Menu"));
            }
        }
    }
}
