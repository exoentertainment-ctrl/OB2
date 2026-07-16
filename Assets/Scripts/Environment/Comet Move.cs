using UnityEngine;

public class CometMove : MonoBehaviour
{
    [SerializeField] GameObject impactEffectPrefab;
    [SerializeField] private int moveSpeed;
    [SerializeField] private int lifetime;
    [SerializeField] private int impactEffectDistanceRemove;

    private Vector3 targetPos;
    GameObject impactEffect;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AlignTowardsTarget();
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckDistanceToTarget();
    }

    void AlignTowardsTarget()
    {
        Collider target = GameObject.FindGameObjectWithTag("Movement Layer").GetComponent<Collider>();
        // targetPos = new Vector3(Random.Range(target.transform.position.x - (target.bounds.size.x/2), target.transform.position.x + (target.bounds.size.x/2)),
        //     Random.Range(target.transform.position.z - (target.bounds.size.z/2), target.transform.position.z + (target.bounds.size.z/2)), Random.Range(target.transform.position.y + (target.bounds.size.y/2), target.transform.position.y + (target.bounds.size.y/2)));
        
        targetPos = new Vector3(Random.Range(target.transform.position.x - (target.bounds.size.x/2), target.transform.position.x + (target.bounds.size.x/2)),
            0, Random.Range(target.transform.position.z - (target.bounds.size.z/2), target.transform.position.z + (target.bounds.size.z/2)));
        // impactEffect = Instantiate(impactEffectPrefab, new Vector3(targetPos.x, targetPos.z, targetPos.y), Quaternion.Euler(90, 0, 0));
        impactEffect = Instantiate(impactEffectPrefab, targetPos, Quaternion.Euler(90, 0, 0));
        
        transform.LookAt(targetPos);
    }

    void CheckDistanceToTarget()
    {
        if(Vector3.Distance(transform.position, targetPos) < impactEffectDistanceRemove)
            Destroy(impactEffect);
            
    }
    
    void Move()
    {
        transform.position += transform.forward * (moveSpeed * Time.fixedDeltaTime);
    }
}
