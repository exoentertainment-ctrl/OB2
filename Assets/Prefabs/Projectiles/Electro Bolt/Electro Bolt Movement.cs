using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ElectroBoltMovement : MonoBehaviour, IUpgrade
{
   #region Serialized Fields

   [SerializeField] ProjectileSO projectileSO;
   [SerializeField] LineRenderer lineRenderer;
   [SerializeField] private SphereCollider collider;

   [SerializeField] private LayerMask targetLayers;
   [SerializeField] private int maxLength;
   [SerializeField] private int maxNumBolts;
   [SerializeField] private int secondanryBoltRange;
   [SerializeField] float timeBetweenBolts;

   #endregion

   #region Variables

   private float damageUpgrade = 1;
   
   private float beginTime;
   private float lastBoltTime;
   private int numBolts;

   private GameObject target;
   private GameObject sourceObject;

   #endregion


   private void OnEnable()
   {
      if(sourceObject == null)
         sourceObject = transform.parent.gameObject;
      
      numBolts = 0;
      lastBoltTime = Time.time;
      transform.position = sourceObject.transform.position;
   }

   private void OnDisable()
   {
      target = null;
   }

   private void Start()
   {
      
   }

   private void Update()
   {
      lineRenderer.SetPosition(0, transform.position);

      // if (target != null)
      // {
         SetLineRendererPosition();
         // target.transform.root.TryGetComponent<IHealth>(out IHealth health);
         // health?.TakeDamage((projectileSO.damage * damageUpgrade) * Time.deltaTime);

         if ((Time.time - timeBetweenBolts) > lastBoltTime)
         {
            if (numBolts < maxNumBolts)
            {
               if (target != null)
               {
                  FindNearbyTargets();
                  lastBoltTime = Time.time;
                  numBolts++;
               }
               else
               {
                  gameObject.SetActive(false);
               }
            }
            else
            {
               gameObject.SetActive(false);
            }
         }
      //}
   }


   void SetLineRendererPosition()
   {
      RaycastHit hit;
      if (Physics.Raycast(transform.position, transform.forward, out hit, maxLength, targetLayers))
      {
         target = hit.collider.transform.gameObject;
         lineRenderer.SetPosition(1, target.transform.position);
         collider.center = transform.InverseTransformPoint(hit.point);

         target.transform.TryGetComponent<IHealth>(out IHealth health);
         health?.TakeDamage((projectileSO.damage * damageUpgrade) * Time.deltaTime);
         
         beginTime = Time.time;
      }
      else
      {
         target = null;
         //lineRenderer.SetPosition(1, transform.position + (transform.forward * maxLength));
      }
   }

   void FindNearbyTargets()
   {
      Collider[] secondaryTargets =  Physics.OverlapSphere(target.transform.position, secondanryBoltRange, targetLayers);
      bool targetFound = false;
      int numTries = 0;

      if (secondaryTargets.Length > 1)
      {
         if (secondaryTargets[0].transform.root.gameObject != target.transform.root.gameObject)
         {
            while (!targetFound && numTries < 20)
            {
               transform.position = target.transform.position;
               int newTarget = Random.Range(0, secondaryTargets.Length);
               numTries++;

               if (secondaryTargets[newTarget].transform.root.gameObject != target)
               {
                  target = secondaryTargets[newTarget].transform.root.gameObject;
                  targetFound = true;
               }
            }
         }

         if (target != null)
         {
            
         }
      }
      else
      {
         gameObject.SetActive(false);
      }
   }
   
   public void Upgrade()
   {
      damageUpgrade += projectileSO.damageUpgradeAmount;
   }
   
   private void OnDrawGizmosSelected()
   {
      Gizmos.DrawWireSphere(transform.position, secondanryBoltRange);
   }
}
