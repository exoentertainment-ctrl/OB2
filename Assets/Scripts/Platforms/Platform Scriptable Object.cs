using UnityEngine;

[CreateAssetMenu(fileName = "Platform SO", menuName = "Platform SO/ Platform SO")]
public class PlatformScriptableObject : BaseEntitySO
{
    public int resourceCost;
    public GameObject platformPrefab;
    public string platformDescription;
}