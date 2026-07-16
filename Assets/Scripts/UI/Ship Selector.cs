using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipSelector : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private GameObject[] generalistShips;
    [SerializeField] private GameObject[] fireSupportShips;
    [SerializeField] private GameObject[] brawlerShips;
    [SerializeField] private GameObject[] lineBreakerShips;
    
    [SerializeField] int numberOfShips;
    [SerializeField] Transform[] spawnPoints;

    #endregion

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void SelectGeneralist()
    {
        for(int x = 0; x < numberOfShips; x++)
            Instantiate(generalistShips[Random.Range(0, generalistShips.Length)], spawnPoints[x].position, Quaternion.identity);
        
        CloseWindow();
    }

    public void SelectFireSupport()
    {
        for(int x = 0; x < numberOfShips; x++)
            Instantiate(fireSupportShips[Random.Range(0, fireSupportShips.Length)], spawnPoints[x].position, Quaternion.identity);
        
        CloseWindow();
    }

    public void SelectBrawler()
    {
        for(int x = 0; x < numberOfShips; x++)
            Instantiate(brawlerShips[Random.Range(0, brawlerShips.Length)], spawnPoints[x].position, Quaternion.identity);
        
        CloseWindow();
    }

    public void SelectLineBreaker()
    {
        for(int x = 0; x < numberOfShips; x++)
            Instantiate(lineBreakerShips[Random.Range(0, lineBreakerShips.Length)], spawnPoints[x].position, Quaternion.identity);
        
        CloseWindow();
    }

    public void SelectRandom()
    {
        GameObject[] allShips = new GameObject[generalistShips.Length + brawlerShips.Length + lineBreakerShips.Length + fireSupportShips.Length];
        allShips[0] = generalistShips[0];
        allShips[1] = generalistShips[1];
        allShips[2] = brawlerShips[0];
        allShips[3] = brawlerShips[1];
        allShips[4] = lineBreakerShips[0];
        allShips[5] = lineBreakerShips[1];
        allShips[6] = fireSupportShips[0];
        allShips[7] = fireSupportShips[1];
        
        for(int x = 0; x < numberOfShips; x++)
            Instantiate( allShips[Random.Range(0, allShips.Length)], spawnPoints[x].position, Quaternion.identity);
        
        CloseWindow();
    }

    void CloseWindow()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
