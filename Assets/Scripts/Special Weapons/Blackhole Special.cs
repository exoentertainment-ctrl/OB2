using System;
using UnityEngine;

public class BlackholeSpecial : MonoBehaviour
{
    [SerializeField] LayerMask targetLayers;
    [SerializeField] private int range;
    [SerializeField] float duration;
    [SerializeField] private float attractionPower;
    [SerializeField] private float damage;
    [SerializeField] private bool showGizmos;
    [SerializeField] private float moveDuration;

    private bool isMoving;
    private float beginTime;
    
    private void Start()
    {
        beginTime = Time.time;
    }

    private void Update()
    {
        CheckDuration();
        DrawShipsIn();
        StopMoving();
    }

    void DrawShipsIn()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, targetLayers);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                collider.transform.root.position = Vector3.Lerp(collider.transform.root.position, transform.position, Time.deltaTime * attractionPower);
                
                collider.TryGetComponent<IHealth>(out IHealth health);
                if(health != null)
                {
                    health.TakeDamage(damage * Time.deltaTime);
                }
            }
        }
    }
    
    void CheckDuration()
    {
        if (Time.time - beginTime > duration)
        {
            Destroy(gameObject);
        }
    }

    void StopMoving()
    {
        moveDuration -= Time.deltaTime;

        if (moveDuration <= 0 && !isMoving)
        {
            isMoving = true;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
            Gizmos.DrawWireSphere(transform.position, range);
    }
}
