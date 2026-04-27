using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelCompleteUI : MonoBehaviour
{
    public TMP_Text timeText;
    public Button nextLevelButton;
    public Button quitButton;

    public LevelManager levelManager;

    public void Show(float timeSpent, bool hasNextLevel)
    {
        int minutes = Mathf.FloorToInt(timeSpent / 60f);
        int seconds = Mathf.FloorToInt(timeSpent % 60f);

        timeText.text = "Completion Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");

        nextLevelButton.interactable = hasNextLevel;

        nextLevelButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();

        nextLevelButton.onClick.AddListener(levelManager.LoadNextLevel);
        quitButton.onClick.AddListener(levelManager.QuitToMenu);
    }
}
