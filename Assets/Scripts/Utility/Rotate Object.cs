using UnityEngine;

public class RotateObject : MonoBehaviour
{
    #region --Serialize Field--

    [SerializeField] private bool spinXAxis;
    [SerializeField] private bool spinYAxis;
    [SerializeField] private bool spinZAxis;
    [SerializeField] private float rotateForce;

    #endregion
    
    Rigidbody rb;
    float xAngle, yAngle, zAngle;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        Rotate();
    }
    
    void Rotate()
    {
        if(spinXAxis)
            xAngle = Random.Range(0f, rotateForce) * Time.fixedDeltaTime;
        
        if(spinYAxis)
            yAngle = Random.Range(0f, rotateForce) * Time.fixedDeltaTime;
        
        if(spinZAxis)
            zAngle = Random.Range(0f, rotateForce) * Time.fixedDeltaTime;
        
        Quaternion deltaRotation = Quaternion.Euler(xAngle, yAngle, zAngle);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
