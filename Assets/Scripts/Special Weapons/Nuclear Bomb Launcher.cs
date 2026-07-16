using UnityEngine;

public class NuclearBombLauncher : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private int bombEjectionForce;

    private void OnEnable()
    {
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody>().AddForce(transform.forward * bombEjectionForce, ForceMode.Impulse);
    }
}
