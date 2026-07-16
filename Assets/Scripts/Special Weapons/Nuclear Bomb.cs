using System;
using System.Collections;
using UnityEngine;

public class NuclearBomb : MonoBehaviour
{
    #region --Serialize Field--

    [SerializeField] private float timer;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private float damageDelay;

    [SerializeField] private bool showGizmos;

    #endregion

    private bool hasExploded;
    
    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
            timer  -= Time.deltaTime;
        else
        {
            if(!hasExploded)
                StartCoroutine(Explode());
            
            hasExploded = true;
        }
    }

    IEnumerator Explode()
    {
        Instantiate(explosionPrefab,  transform.position, Quaternion.identity);
        
        yield return new WaitForSeconds(damageDelay);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, targetLayers);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                collider.TryGetComponent<IHealth>(out IHealth health);
                if(health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }
        
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if(showGizmos)
            Gizmos.DrawWireSphere(transform.position, range);
    }
}
