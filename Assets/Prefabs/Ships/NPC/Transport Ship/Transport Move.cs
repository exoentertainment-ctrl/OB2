using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TransportMove : MonoBehaviour
{
    [SerializeField] private BaseShipSO shipSO;
    [SerializeField] private int destinationDistance;
    
    GameObject target;
    Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindDestination();
    }

    private void Update()
    {
        CheckDistance();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void FindDestination()
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag("Destination Object");

        if (possibleTargets.Length > 0)
        {
            target = possibleTargets[Random.Range(0, possibleTargets.Length)];
        }
    }

    void Move()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position);
            Vector3 rotateDir = Vector3.RotateTowards(transform.forward, direction,
                Time.deltaTime * shipSO.turnSpeed, 0.0f);

            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.LookRotation(rotateDir),
                Time.fixedDeltaTime * shipSO.turnSpeed);
        }

        rb.linearVelocity = transform.rotation * Vector3.forward * (shipSO.moveSpeed * Time.fixedDeltaTime);
    }

    void CheckDistance()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < destinationDistance)
        {
            ShipCounter.instance.IncreaseNumEscapedShips();
            gameObject.SetActive(false);
        }
    }
}
