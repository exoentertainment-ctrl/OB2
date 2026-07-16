using UnityEngine;
using UnityEngine.UI;

public class BehaviorToggle : MonoBehaviour
{
    [SerializeField] Toggle behaviorToggle;
    
    public void ChangeBehavior()
    {
        Vector3 toggleScale = behaviorToggle.transform.localScale;
        toggleScale.x *= -1;
        behaviorToggle.transform.localScale = toggleScale;
    }

    public void SetBehavior(bool behavior)
    {
        if (behavior)
        {
            Vector3 toggleScale = behaviorToggle.transform.localScale;
            toggleScale.x = 1;
            behaviorToggle.transform.localScale = toggleScale;
        }
        else
        {
            Vector3 toggleScale = behaviorToggle.transform.localScale;
            toggleScale.x = -1;
            behaviorToggle.transform.localScale = toggleScale;
        }
            
    }
}
