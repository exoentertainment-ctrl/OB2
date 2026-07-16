using System;
using UnityEngine;
using System.Collections;

public class ProjectileMove : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] ProjectileSO projectileSO;

    #endregion

    private void FixedUpdate()
    {
        Move();
    }
    
    private void OnEnable()
    {
        StartCoroutine(DeactivateRoutine());
    }
    
    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(projectileSO.lifetime);
        
        gameObject.SetActive(false);
    }

    void Move()
    {
        transform.position += transform.forward * (projectileSO.moveSpeed * Time.fixedDeltaTime);
    }
}
