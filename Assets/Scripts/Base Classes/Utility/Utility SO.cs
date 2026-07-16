using UnityEngine;

[CreateAssetMenu(fileName = "Utility SO", menuName = "Utility SO/Base Utility")]
public class UtilitySO : ScriptableObject
{
    public GameObject utilityEffect;
    public float modifier;
    public int duration;
    public int range;
    public float rateOfFire;
    public LayerMask targetLayers;
    
    public GameObject dischargePrefab;
    public AudioClipSO fireSFX;
    
    public float rofUpgradeAmount;
}
