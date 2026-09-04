using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPhaseOut : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] private float alphaChangeAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color c = text.color;
        c.a -= alphaChangeAmount * Time.deltaTime;;
        text.color= c;
    }
}
