using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;

public class CameraFocus : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraManager>().FocusCamera(gameObject);

    }
}
