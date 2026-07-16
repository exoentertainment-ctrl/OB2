using UnityEngine;
using MystifyFX;

public class TriggerTargettedVFX : MonoBehaviour
{
    [SerializeField] MystifyEffect mystifyEffect;
    [SerializeField] private int duration;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mystifyEffect.HitFX(transform.position);
        Destroy(gameObject, duration);
    }
}
