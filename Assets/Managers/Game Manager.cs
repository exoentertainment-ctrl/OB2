using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private GameObject levelEndWindow;
    [SerializeField] private GameObject levelLostWindow;
    [SerializeField] private GameObject levelEndingObject;
    
    #endregion

    private bool hasLevelEndingObject;
    
    private bool checkForEnemies;
    private void Start()
    {
        if(AudioManager.instance != null)
            AudioManager.instance.PlayRandomMusic();
        
        if(levelEndingObject != null)
            hasLevelEndingObject = true;
    }

    private void Update()
    {
        if(checkForEnemies)
            CheckForLastEnemy();
        
        CheckLevelCriticalObject();
    }

    public void CheckForLastEnemy()
    {
        checkForEnemies = true;
        
        if (Physics.OverlapSphere(transform.position, Mathf.Infinity, LayerMask.GetMask("Enemy Ship")).Length < 1)
        {
            levelEndWindow.SetActive(true);
            
            if(AudioManager.instance != null)
                AudioManager.instance.PlayLevelEndSFX();
        }
    }

    void CheckLevelCriticalObject()
    {
        if (levelEndingObject == null && hasLevelEndingObject)
        {
            levelLostWindow.SetActive(true);
            
            if(AudioManager.instance != null)
                AudioManager.instance.PlayLevelLost();
        }
    }
    
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadSettings()
    {
        GameObject window = (GameObject)Instantiate(Resources.Load("Settings Menu"));
    }
}
