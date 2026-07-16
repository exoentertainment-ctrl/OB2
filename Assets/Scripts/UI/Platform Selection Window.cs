using System;
using UnityEngine;

public class PlatformSelectionWindow : MonoBehaviour
{
    private Vector3 buildPosition;
    
    public void GetBuildLocation(Vector3  position)
    {
        buildPosition = position;
    }

    public void PlacePlatform(PlatformScriptableObject platform)
    {
        if (ResourceManager.instance.GetCredits() >= platform.resourceCost)
        {
            Instantiate(platform.platformPrefab, new Vector3(buildPosition.x, GameObject.FindGameObjectWithTag("Movement Layer").GetComponent<InteractionLayer>()
                .ReturnOffset(), buildPosition.z), Quaternion.identity);
            
            ResourceManager.instance.DecreaseCredits(platform.resourceCost);
            //Play SFX
        }
        else
        {
            //Play SFX
        }

        CloseWindow();
    }

    public void CloseWindow()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
