using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private BaseShipSO shipSO;
    [SerializeField] private bool needsName;
    [SerializeField] private GameObject selectedShipVFX;
    
    [SerializeField] private GameObject specialWeapon;
    [SerializeField] private int specialWeaponDelay;

    #endregion

    #region Variables

    ShipMovement shipMovement;
    TurretController turretController;
    private int profileImage;
    private int voiceIndex;

    #endregion
    
    GameObject currentTarget;
    private bool isWaitingForTarget;

    private void Awake()
    {
        shipMovement = GetComponent<ShipMovement>();
        turretController = GetComponent<TurretController>();
    }

    private void Start()
    {
        // if (needsName)
        // {
            SetProfileImage();
            SetShipName();
            SetVoiceIndex();
        // }
    }
    
    void SetProfileImage()
    {
        profileImage = Random.Range(0, shipSO.profileImages.Length);
    }

    void SetVoiceIndex()
    {
        voiceIndex = Random.Range(0, shipSO.movementVoices.Length);
    }

    public Sprite GetProfileImage()
    {
        return shipSO.profileImages[profileImage];
    }
    
    void SetShipName()
    {
        TextAsset shipFirstName = Resources.Load("shipFirstNames") as TextAsset;
        TextAsset shipSecondName = Resources.Load("shipSecondNames") as TextAsset;

        string[] firstNames = shipFirstName.text.Split(',');;
        string[] secondNames = shipSecondName.text.Split(',');
        
        gameObject.name = firstNames[Random.Range(0, firstNames.Length - 1)] + " " +  secondNames[Random.Range(0, secondNames.Length - 1)];
    }
    
    //When the ship is selected, turn on the selection VFX, the health bar, send ship data to the UI, play selection SFX
    public void ShipSelected()
    {
        selectedShipVFX?.SetActive(true);
        //turn on hp bar
        //send ship data to UI
        //play selection SFX
    }

    //When the ship is no longer selected, turn off the selection VFX, health bar
    public void ShipDeselected()
    {
        selectedShipVFX?.SetActive(false);
        //turn off hp bar
    }

    public void SetDestinationPos(Vector3 pos)
    {
        shipMovement?.SetDestinationPos(pos);
        //Play acknowledgement SFX
    }

    //Receive target info from input manager and pass it along to the currently selected ship movement and turret controller
    public void SetTarget(GameObject target)
    {
        isWaitingForTarget = true;
        
        shipMovement?.SetTarget(target);
        turretController?.SetTarget(target);
    }

    public void PlaySelectedSound()
    {
        if(AudioManager.instance != null)
            if(shipSO.selectedVoices[voiceIndex] != null)
                AudioManager.instance.PlaySound(shipSO.selectedVoices[voiceIndex]);
    }
    
    public void PlayMoveSound()
    {
        if(AudioManager.instance != null)
            if(shipSO.movementVoices[voiceIndex] != null)
                AudioManager.instance.PlaySound(shipSO.movementVoices[voiceIndex]);
    }

    public void PlayTargetSetSound()
    {
        if(AudioManager.instance != null)
            if(shipSO.targetSetVoices[voiceIndex] != null)
                AudioManager.instance.PlaySound(shipSO.targetSetVoices[voiceIndex]);
    }
    
    public void PlayShieldDownSound()
    {
        if(AudioManager.instance != null)
            if(shipSO.shieldDownVoices[voiceIndex] != null)
                AudioManager.instance.PlaySound(shipSO.shieldDownVoices[voiceIndex]);
    }
    
    public void PlayLowHullSound()
    {
        if(AudioManager.instance != null)
            if(shipSO.movementVoices[voiceIndex] != null)
                AudioManager.instance.PlaySound(shipSO.lowHullVoices[voiceIndex]);
    }

    public void ActivateSpecialWeapon()
    {
        specialWeapon.SetActive(true);

        StartCoroutine(ActivateSpecialWeaponRoutine());
    }

    IEnumerator ActivateSpecialWeaponRoutine()
    {
        yield return new WaitForSeconds(specialWeaponDelay);
        
        UISelectedShip.instance.ActivateSpecialButton(gameObject);
    }
}
