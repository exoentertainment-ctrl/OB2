using System;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] TMP_Text creditText;
    
    [SerializeField] Animator resourceTextAnimator;
    [SerializeField] TextMeshProUGUI animatedCreditText;
    [SerializeField] Color32 increasedCreditColor;
    [SerializeField] Color32 decreasedCreditColor;
    
    [SerializeField] private int currentCredits;

    #endregion

    public static ResourceManager instance;
    
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        creditText.text = currentCredits.ToString();
    }

    public void IncreaseCredits(int amount)
    {
        currentCredits += amount;
        creditText.text = currentCredits.ToString();
        
        animatedCreditText.text = amount.ToString();
        animatedCreditText.faceColor = increasedCreditColor;
        resourceTextAnimator.SetTrigger("Move");
    }

    public void DecreaseCredits(int amount)
    {
        currentCredits -= amount;
        creditText.text = currentCredits.ToString();
        
        animatedCreditText.text = amount.ToString();
        animatedCreditText.faceColor = decreasedCreditColor;
        resourceTextAnimator.SetTrigger("Move");
    }

    public int GetCredits()
    {
        return currentCredits;
    }
}
