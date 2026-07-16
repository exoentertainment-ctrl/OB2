using UnityEngine;

public class IncreaseMaterialLight : MonoBehaviour
{
    [SerializeField] float maxLightIntensity;
    [SerializeField] private float lightIncreaseAmount;
    [SerializeField] private float lightDuration;
    
    Material material;
    float startingLightIntensity;
    float currentLightIntensity;
    private bool isIncreasing = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;    
        startingLightIntensity = material.GetFloat("_Lights");
        currentLightIntensity = startingLightIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (isIncreasing)
        {
            currentLightIntensity += Time.deltaTime * lightIncreaseAmount;
            
            if (currentLightIntensity >= maxLightIntensity)
                isIncreasing = false;
        }
        else
        {
            currentLightIntensity -= Time.deltaTime * lightIncreaseAmount;
            
            if (currentLightIntensity <= startingLightIntensity)
                isIncreasing = true;
        }

        material.SetFloat("_Lights", currentLightIntensity);
    }
}
