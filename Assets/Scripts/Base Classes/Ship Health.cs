using System;
using System.Collections;
using DamageNumbersPro;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class ShipHealth : MonoBehaviour, IHealth
{
    #region Serialized Fields

    [SerializeField] private BaseShipSO shipSO;
    [SerializeField] UnityEvent onShipDeath;
    [SerializeField] MMFeedbacks deathFeedbacks;
    [SerializeField] private DamageNumber numberPrefab;
    [SerializeField] private bool focusCameraOnDeath;

    #endregion

    #region Variables

    //UISelectedShip uiSelectedShip;
    private float currentHealth;
    float adjustedHealth;
    private bool isHit;
    private bool isDead;
    private bool isSelected;

    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = shipSO.maxHealth;
        //uiSelectedShip = GameObject.FindGameObjectWithTag("UISelectedShip").GetComponent<UISelectedShip>();
    }

    private void Update()
    {
        isHit = false;
    }
    
    public void SetSelected()
    {
        isSelected = !isSelected;
    }

    public void TakeDamage(float damage)
    {
        if(!isHit)
        {
            isHit = true;
            currentHealth -= damage;

            if (numberPrefab != null)
            {
                DamageNumber damageNumber = numberPrefab.Spawn(transform.position, damage);
            }

            if (currentHealth <= 0 && !isDead)
            {
                if(ResourceManager.instance != null)
                    ResourceManager.instance.IncreaseCredits(shipSO.pointValue);
                
                isDead = true;
                onShipDeath?.Invoke();
                
                if(focusCameraOnDeath && GameObject.FindGameObjectWithTag("Camera") != null)
                    GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraManager>().FocusCamera(gameObject);
                
                StartCoroutine(ExplodeRoutine());
            }
        }
    }

    IEnumerator ExplodeRoutine()
    {
        Collider shipCollider = gameObject.GetComponentInChildren<Collider>();
        
        for (int x = 0; x < shipSO.numExplosions; x++)
        {
            yield return new WaitForSeconds(shipSO.explosionFrequency);
            
            Vector3 randomSpot = new Vector3(
                Random.Range(shipCollider.bounds.center.x - shipCollider.bounds.size.x / 2,
                    shipCollider.bounds.center.x + shipCollider.bounds.size.x / 2),
                Random.Range(shipCollider.bounds.center.y - shipCollider.bounds.size.y / 2,
                    shipCollider.bounds.center.y + shipCollider.bounds.size.y / 2),
                Random.Range(shipCollider.bounds.center.z - shipCollider.bounds.size.z / 2,
                    shipCollider.bounds.center.z + shipCollider.bounds.size.z / 2));
            
            Instantiate(shipSO.explosionPrefab,  randomSpot, Quaternion.identity);
            
            if(AudioManager.instance != null)
                if(shipSO.smallExplosion  != null)
                    AudioManager.instance.PlaySound(shipSO.smallExplosion, transform.position);
                    
            deathFeedbacks?.PlayFeedbacks();

            if (x == (shipSO.numExplosions - 1))
            {
                yield return new WaitForSeconds(shipSO.explosionFrequency);
                Instantiate(shipSO.finalExplosionPrefab, transform.position, Quaternion.identity);
                
                if(AudioManager.instance != null)
                    if(shipSO.finalExplosion  != null)
                        AudioManager.instance.PlaySound(shipSO.finalExplosion, transform.position);
            }
        }
        
        Instantiate(shipSO.debrisPrefab[Random.Range(0, shipSO.debrisPrefab.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public float GetHealth()
    {
        return currentHealth/shipSO.maxHealth;
    }

    public float GetShield()
    {
        return 0;
    }
    
    public void Upgrade()
    {
        adjustedHealth += shipSO.maxHealth + (shipSO.maxHealth * shipSO.upgradeAmount);
        currentHealth = shipSO.maxHealth + (shipSO.maxHealth * shipSO.upgradeAmount);
    }
}
