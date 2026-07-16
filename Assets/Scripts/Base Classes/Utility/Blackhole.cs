using UnityEngine;

public class Blackhole : MonoBehaviour
{
    [SerializeField] UtilitySO utilitySO;
    [SerializeField] private float modifierIncrease;
    
    private float beginTime;
    private float currentModifier;
    
    private void Start()
    {
        beginTime = Time.time;
        currentModifier = utilitySO.modifier;
    }

    private void Update()
    {
        CheckDuration();
        DrawShipsIn();
    }

    void DrawShipsIn()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, utilitySO.range, utilitySO.targetLayers);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                collider.transform.root.position = Vector3.Lerp(collider.transform.root.position, transform.position, Time.deltaTime * currentModifier);
            }
        }
        
        currentModifier += modifierIncrease * Time.deltaTime;
    }
    
    void CheckDuration()
    {
        if (Time.time - beginTime > utilitySO.duration)
        {
            Destroy(gameObject);
        }
    }
}
