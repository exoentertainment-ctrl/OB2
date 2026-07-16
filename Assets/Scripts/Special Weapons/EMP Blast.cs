using System;
using UnityEngine;

public class EMPBlast : MonoBehaviour
{
    #region --Serialize Field--

    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float expandDuration;
    [SerializeField] private float expandAmount;
    [SerializeField] private int empRange;
    [SerializeField] private GameObject empEffectObject;
    [SerializeField] private GameObject empEffectPrefab;

    [SerializeField] private bool showGizmos;
    
    #endregion

    private bool isExpanding;
    private float lastExpandTime;

    // Update is called once per frame
    void Update()
    {
        Expand();
    }

    private void OnEnable()
    {
        empEffectObject.SetActive(true);
        lastExpandTime = Time.time;
        isExpanding = true;
        AffectEnemies();
    }

    void Expand()
    {
        if (isExpanding)
        {
            Vector3 newScale;
            newScale.x =  empEffectObject.transform.localScale.x + (expandAmount * Time.deltaTime);
            newScale.y =  empEffectObject.transform.localScale.y + (expandAmount * Time.deltaTime);
            newScale.z =  empEffectObject.transform.localScale.z + (expandAmount * Time.deltaTime);
            
            empEffectObject.transform.localScale = newScale;

            if (Time.time - lastExpandTime > expandDuration)
            {
                empEffectObject.transform.localScale = Vector3.one;
                empEffectObject.SetActive(false);
                isExpanding = false;
            }
        }
    }

    void AffectEnemies()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, empRange, targetLayers);
        
        if (possibleTargets.Length > 0)
        {
            foreach (Collider target in possibleTargets)
            {
                Instantiate(empEffectPrefab, target.transform.root);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if(showGizmos)
            Gizmos.DrawWireSphere(transform.position, empRange);
    }
}
