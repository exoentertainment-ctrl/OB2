using System;
using MystifyFX;
using UnityEngine;

public class MystifyTrigger : MonoBehaviour
{
    MystifyEffect mystifyEffect;

    private void Awake()
    {
        mystifyEffect = GetComponent<MystifyEffect>();
    }

    private void OnTriggerEnter(Collider other)
    {
        mystifyEffect.HitFX(other.transform.position);
    }
}
