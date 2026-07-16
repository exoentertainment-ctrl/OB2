using System;
using UnityEngine;

public class LaserMovement : MonoBehaviour
{
   #region Serialized Fields

   [SerializeField] ProjectileSO projectileSO;
   [SerializeField] LineRenderer lineRenderer;
   [SerializeField] private SphereCollider collider;

   [SerializeField] private LayerMask targetLayers;
   [SerializeField] float laserDuration;
   [SerializeField] private int maxLength;
   
   #endregion

   #region Variables

   private float damageUpgrade = 1;

   private float beginTime;

   private GameObject target;

   #endregion

   private void OnEnable()
   {
      beginTime = Time.time;
   }

   private void OnDisable()
   {
      target = null;
   }

   private void Update()
   {
      SetLineRendererPosition();
      
      if((Time.time - beginTime) >= laserDuration)
         gameObject.SetActive(false);
   }


   void SetLineRendererPosition()
   {
      lineRenderer.SetPosition(0, transform.position);
      
      RaycastHit hit;
      if (Physics.Raycast(transform.position, transform.forward, out hit, maxLength, targetLayers))
      {
         lineRenderer.SetPosition(1, hit.point);

         collider.center = transform.InverseTransformPoint(hit.point);
         hit.transform.TryGetComponent<IHealth>(out IHealth health);
         health?.TakeDamage((projectileSO.damage * Time.deltaTime) * damageUpgrade);
      }
      else
      {
         lineRenderer.SetPosition(1, transform.position + (transform.forward * maxLength));
      }
   }

   public void Upgrade()
   {
      damageUpgrade += projectileSO.damageUpgradeAmount;
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.DrawLine(transform.position, transform.position + (transform.forward * maxLength));
   }
}
