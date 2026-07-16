using System;
using UnityEngine;
using System.Collections;

public class LightFlash : MonoBehaviour
{
    private Light explosionLight;
    public float maxIntensity = 10f;
    public float maxRange = 15f;
    public float duration = 0.5f;

    void Awake()
    {
        explosionLight = GetComponent<Light>();
        Debug.Log(explosionLight.intensity);
        explosionLight.intensity = 0f;
        explosionLight.range = 0f;
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float timer = 0f;

        // Flash in
        while (timer < duration / 4f)
        {
            timer += Time.deltaTime;
            explosionLight.intensity = Mathf.Lerp(0f, maxIntensity, timer / (duration / 4f));
            explosionLight.range = Mathf.Lerp(0f, maxRange, timer / (duration / 4f));
            yield return null;
        }

        
        // Fade out
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = (timer - (duration / 4f)) / (duration * 0.75f);
            explosionLight.intensity = Mathf.Lerp(maxIntensity, 0f, progress);
            explosionLight.range = Mathf.Lerp(maxRange, 0f, progress);
            yield return null;
        }

        Destroy(gameObject);
    }
}
