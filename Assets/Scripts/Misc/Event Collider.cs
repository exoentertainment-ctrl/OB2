using System;
using UnityEngine;
using UnityEngine.Events;

public class EventCollider : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] UnityEvent onCollide;

    [SerializeField] private bool freezeTime;
    [SerializeField] private bool spawnObject;
    [SerializeField] private bool isTimedEvent;
    
    [SerializeField] Transform spawnPoint;
    [SerializeField] private GameObject specificTarget;
    [SerializeField] private int eventTime;

    #endregion
    
    private Collider collider;
    private float timeCreated;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        timeCreated = Time.time;
    }

    private void Update()
    {
        if (isTimedEvent)
        {
            if(Time.time - timeCreated > eventTime)
                InvokeEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        onCollide?.Invoke();
        
        if(freezeTime)
            Time.timeScale = 0;
        
        if(specificTarget != null)
            if(specificTarget == other.transform.root.gameObject)
                onCollide?.Invoke();
    }

    public void PlaySound(AudioClipSO clip)
    {
        
    }

    public void SpawnObject(GameObject obj)
    {
        if(spawnPoint == null)
            Instantiate(obj, transform.position, Quaternion.identity);
        else
            Instantiate(obj, spawnPoint.position, Quaternion.identity);
    }

    void InvokeEvent()
    {
        onCollide?.Invoke();
        Destroy(gameObject, 3);
    }
}
