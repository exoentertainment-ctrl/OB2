using System;
using UnityEngine;

public class ShipCounter : MonoBehaviour
{
    #region --Serialize Field--

    [SerializeField] int minSuccess;

    [SerializeField] GameObject levelWinWindow;
    [SerializeField]  GameObject levelLoseWindow;

    #endregion

    
    static public ShipCounter instance;
    
    int numEscapedShips;

    private void Awake()
    {
        instance = this;
    }

    public void IncreaseNumEscapedShips()
    {
        numEscapedShips++;
    }

    public void CheckShipsEscaped()
    {
        if (numEscapedShips >= minSuccess)
        {
            levelWinWindow.SetActive(true);
        }
        else
        {
            levelLoseWindow.SetActive(true);
        }
    }
}
