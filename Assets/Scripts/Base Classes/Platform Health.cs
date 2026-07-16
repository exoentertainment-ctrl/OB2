using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Events;
using System.Collections;
public class PlatformHealth : MonoBehaviour, IHealth
{
    #region Serialized Fields

    [SerializeField] private PlatformScriptableObject platformSO;
    [SerializeField] UnityEvent onDeath;
    [SerializeField] MMFeedbacks deathFeedbacks;

    #endregion

    #region Variables
    
    private float currentHealth;
    float healthUpgrade = 1;
    private bool isHit;
    private bool isDead;

    #endregion
    
    void Start()
    {
        currentHealth = platformSO.maxHealth;
    }

    private void Update()
    {
        isHit = false;
    }
    
    public void TakeDamage(float damage)
    {
        if(!isHit)
        {
            isHit = true;
            currentHealth -= damage;
            
                
            if (currentHealth <= 0 && !isDead)
            {
                isDead = true;
                onDeath?.Invoke();
                StartCoroutine(ExplodeRoutine());
            }
        }
    }
    
    IEnumerator ExplodeRoutine()
    {
        Collider shipCollider = gameObject.GetComponentInChildren<Collider>();
        
        for (int x = 0; x < platformSO.numExplosions; x++)
        {
            yield return new WaitForSeconds(platformSO.explosionFrequency);
            
            Vector3 randomSpot = new Vector3(
                Random.Range(shipCollider.bounds.center.x - shipCollider.bounds.size.x / 2,
                    shipCollider.bounds.center.x + shipCollider.bounds.size.x / 2),
                Random.Range(shipCollider.bounds.center.y - shipCollider.bounds.size.y / 2,
                    shipCollider.bounds.center.y + shipCollider.bounds.size.y / 2),
                Random.Range(shipCollider.bounds.center.z - shipCollider.bounds.size.z / 2,
                    shipCollider.bounds.center.z + shipCollider.bounds.size.z / 2));
            
            Instantiate(platformSO.explosionPrefab,  randomSpot, Quaternion.identity);
            deathFeedbacks?.PlayFeedbacks();
        }
        
        Destroy(transform.root.gameObject);
    }
    
    public void Upgrade()
    {
        healthUpgrade += platformSO.maxHealth * platformSO.upgradeAmount;
        currentHealth = platformSO.maxHealth + healthUpgrade;
    }
    
    public float GetHealth()
    {
        return 0;
    }

    public float GetShield()
    {
        return 0;
    }
}
