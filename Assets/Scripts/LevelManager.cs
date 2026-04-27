using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelCompleteUI levelCompleteUI;
    public GameObject GameOver;
    private float levelStartTime;
    private bool levelCompleted = false;
    private PersistantObject levelTracker;

    // Start is called before the first frame update
    void Start()
    {
        levelTracker = PersistantObject.Instance;
        levelStartTime = Time.time;

        if (levelCompleteUI != null)
            levelCompleteUI.gameObject.SetActive(false);

        if(GameOver != null)
            GameOver.SetActive(false);

        Time.timeScale = 1f;
    }

    public void CompleteLevel()
    {
        if (levelCompleted) return;

        levelCompleted = true;

        float timeSpent = Time.time - levelStartTime;

        if (levelCompleteUI != null)
        {
            levelCompleteUI.gameObject.SetActive(true);
            levelCompleteUI.Show(timeSpent, HasNextLevel());
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        
    }

    public void LoadNextLevel()
    {

        Time.timeScale = 1f;
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentIndex + 1) % totalScenes;
        levelTracker.SetLevelIndex(nextSceneIndex);
        Debug.Log("On scene: " + currentIndex + "out of " + totalScenes);
        if (nextSceneIndex == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }


    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();

        Debug.Log("Quit game");
    }

    private bool HasNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        return currentIndex + 1 < SceneManager.sceneCountInBuildSettings;
    }

    public void ShowDeathScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        SceneManager.LoadScene("GameOverScreen");
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
