using UnityEngine;

public class EnableObject : MonoBehaviour
{
    [SerializeField] GameObject[] targetObjects;
    
    public void EnableTarget()
    {
        foreach (var target in targetObjects)
        {
            target.SetActive(true);
        }
    }
}
