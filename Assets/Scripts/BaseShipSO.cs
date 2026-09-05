using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Base Ship SO", menuName = "Ships SO/Base Ship")]
public class BaseShipSO : BaseEntitySO
{
    #region Health Variables
    
    public float shieldRechargeRate;
    public int shieldDownDuration;
    
    #endregion

    #region Movement Variables

    public float moveSpeed;
    public float turnSpeed;
    public int lookAheadDistance;
    public LayerMask obstacleLayerMask;
    public LayerMask targetLayerMask;

    #endregion

    #region SFX Variables

    public AudioClipSO smallExplosion;
    public AudioClipSO finalExplosion;
    public AudioClipSO[] movementVoices;
    public AudioClipSO[] shieldDownVoices;
    public AudioClipSO[] lowHullVoices;
    public AudioClipSO[] selectedVoices;
    public AudioClipSO[] targetSetVoices;
 
    #endregion

    #region Explosion  Variables
    
    public GameObject finalExplosionPrefab;
    public GameObject[] debrisPrefab;

    #endregion

    public Sprite[] profileImages;
    public int pointValue;
}
