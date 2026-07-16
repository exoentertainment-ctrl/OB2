using UnityEngine;
using UnityEngine.Events;

public class IncreaseCredits : MonoBehaviour
{
    [SerializeField] private int eventDelay;
    [SerializeField] private UnityEvent creditEvent;
    
    private float lastTimeInvoked;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastTimeInvoked = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastTimeInvoked > eventDelay)
        {
            creditEvent?.Invoke();
            lastTimeInvoked = Time.time;
        }
    }
}
