using System;
using System.Collections;
using MoreMountains.Tools;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainMenuCameraControl : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] private LayerMask targetMask;
    [SerializeField] CinemachineCamera cinemachineCamera;
    [SerializeField] MMAutoRotate autoRotate;
    [SerializeField] private int newTargetDelay;
    [SerializeField] private float cameraSlowDuration;
    [SerializeField] private float cameraSlowScale;

    #endregion

    private float lastFocusTime;

    private void Start()
    {
        lastFocusTime = Time.time;

        Collider[] potentialTargets = Physics.OverlapSphere(cinemachineCamera.transform.position, Mathf.Infinity, targetMask);

        if (potentialTargets.Length > 0)
        {
            cinemachineCamera.Follow = potentialTargets[Random.Range(0, potentialTargets.Length)].transform.root;
            cinemachineCamera.LookAt = cinemachineCamera.Follow;
            autoRotate.OrbitCenterTransform =  cinemachineCamera.Follow;
        }
    }

    // Update is called once per frame
    void Update()
    {
        cinemachineCamera.transform.LookAt(cinemachineCamera.Follow);
        FindNewTarget();
    }

    void FindNewTarget()
    {
        if (Time.time - lastFocusTime > newTargetDelay)
        {
            Collider[] potentialTargets = Physics.OverlapSphere(cinemachineCamera.transform.position, Mathf.Infinity, targetMask);

            if (potentialTargets.Length > 0)
            {
                cinemachineCamera.Follow = potentialTargets[Random.Range(0, potentialTargets.Length)].transform.root;
                cinemachineCamera.LookAt = cinemachineCamera.Follow;
                autoRotate.OrbitCenterTransform =  cinemachineCamera.Follow;
            }
            
            lastFocusTime = Time.time;
            StartCoroutine(SlowTimeRoutine());
        }
    }

    private IEnumerator SlowTimeRoutine()
    {
        Time.timeScale = cameraSlowScale;
        
        yield return new WaitForSeconds(cameraSlowDuration);

        Time.timeScale = 1f;
    }
}
