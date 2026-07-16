using System;
using UnityEngine;

public class CWIStest : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Missile")
            Debug.Log("missile hit");
    }
}
