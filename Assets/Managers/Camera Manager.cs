using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private CinemachineCamera defaultCamera;
    [SerializeField] private float panSpeed;

    [SerializeField] int cameraFocusDelay;
    
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxZoomOut;

    #endregion

    #region Variables

    Vector3 lastCameraPos;
    private float startingYPos;
    bool isPanningLeft;
    bool isPanningRight;
    bool isPanningUp;
    bool isPanningDown;

    int scrollDirection;
    private float currentZoom;

    public static CameraManager instance;
    
    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        currentZoom = 0;
        startingYPos = defaultCamera.transform.position.y;
    }

    private void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        if (Time.timeScale == 1)
        {
            if (isPanningLeft)
            {
                ResetTrackingTarget();
                PanCameraLeft();
            }

            if (isPanningRight)
            {
                ResetTrackingTarget();
                PanCameraRight();
            }

            if (isPanningUp)
            {
                ResetTrackingTarget();
                PanCameraUp();
            }

            if (isPanningDown)
            {
                ResetTrackingTarget();
                PanCameraDown();
            }

            if (scrollDirection != 0)
                AdjustZoom();
        }
    }

    public void MoveCameraTo(Transform movePos)
    {
        Time.timeScale = 0;
        lastCameraPos = defaultCamera.transform.position;
    }

    public void ReturnCameraPos()
    {
        defaultCamera.transform.position =  lastCameraPos;
        
        Time.timeScale = 1;
    }

    public void FocusCamera()
    {
        GameObject target = ShipManager.instance.GetSelectedShip();
        
        if(target != null)
            defaultCamera.Target.TrackingTarget =  target.transform;
    }

    public void FocusCamera(GameObject target)
    {
        if (target != null)
        {
            Vector3 cameraPos =  defaultCamera.transform.position;
            defaultCamera.Target.TrackingTarget = target.transform;
            StartCoroutine(ResetCameraRoutine(cameraPos));
        }
    }

    void ResetCamera(Vector3 cameraPos)
    {
        defaultCamera.Target.TrackingTarget = null;
        defaultCamera.transform.position = cameraPos;
    }

    IEnumerator ResetCameraRoutine(Vector3 cameraPos)
    {
        yield return new WaitForSeconds(cameraFocusDelay);
        
        ResetCamera(cameraPos);
    }
    
    void ResetTrackingTarget()
    {
        defaultCamera.Target.TrackingTarget = null;
    }
    
    #region Movement Methods

    void PanCameraLeft()
    {
        Vector3 direction = Vector3.left;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.unscaledDeltaTime * panSpeed), Space.World);
    }
    
    void PanCameraRight()
    {
        Vector3 direction = Vector3.right;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.unscaledDeltaTime * panSpeed), Space.World);
    }
    
    void PanCameraDown()
    {
        Vector3 direction = Vector3.back;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.unscaledDeltaTime * panSpeed), Space.World);
    }
    
    void PanCameraUp()
    {
        Vector3 direction = Vector3.forward;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.unscaledDeltaTime * panSpeed), Space.World);
    }

    void AdjustZoom()
    {
        if (scrollDirection > 0)
        {
            Vector3 newPos = defaultCamera.transform.position + (defaultCamera.transform.forward * (Time.unscaledDeltaTime * zoomSpeed));

            if (newPos.y > startingYPos)
            {
                defaultCamera.transform.position += defaultCamera.transform.forward * (Time.unscaledDeltaTime * zoomSpeed);
                currentZoom += Time.deltaTime * zoomSpeed;
            }
        }
        else if (scrollDirection < 0)
        {
            Vector3 newPos = defaultCamera.transform.position - (defaultCamera.transform.forward * (Time.unscaledDeltaTime * zoomSpeed));
            if (newPos.y < maxZoomOut)
            {
                defaultCamera.transform.position -= defaultCamera.transform.forward * (Time.unscaledDeltaTime * zoomSpeed);
                currentZoom -= Time.deltaTime * zoomSpeed;
            }
        }
    }
    
    #endregion

    #region Input Methods

    public void PanLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningLeft = true;
        }
        else if (context.canceled)
        {
            isPanningLeft = false;
        }
    }
    
    public void PanRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningRight = true;
        }
        else if (context.canceled)
        {
            isPanningRight = false;
        }
    }
    
    public void PanUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningUp = true;
        }
        else if (context.canceled)
        {
            isPanningUp = false;
        }
    }
    
    public void PanDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningDown = true;
        }
        else if (context.canceled)
        {
            isPanningDown = false;
        }
    }

    public void ZoomControl(InputAction.CallbackContext context)
    {
        float scrollY = context.ReadValue<float>();

        if (scrollY > 0)
        {
            scrollDirection = 1;
        }    
        else if (scrollY < 0)
        {
            scrollDirection = -1;
        }
        else if (scrollY == 0)
        {
            scrollDirection = 0;
        }
    }
    
    #endregion
}
