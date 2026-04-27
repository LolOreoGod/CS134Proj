using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetryLevel : MonoBehaviour
{
    public Image fadeLayer;
    public GameObject fadeObject;
    public float fadeDuration = 2f;
    private PersistantObject levelTracker;
    
    void Start() {
        levelTracker = PersistantObject.Instance;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadCurrentLevel() {
        fadeObject.SetActive(true);
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack() {
        float elapsed = 0f;
        Color color = fadeLayer.color;
        while(elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp(elapsed/fadeDuration, 0f, 1f);
            color.a = alpha;
            fadeLayer.color = color;

            yield return null;
        }
        color.a = 1f;
        fadeLayer.color = color;
        Debug.Log(levelTracker.GetLevelIndex());
        SceneManager.LoadScene(levelTracker.GetLevelIndex());
        //SceneManager.LoadScene(1);
    }

    IEnumerator FadeBackIn() {
        float elapsed = 0f;
        Color color = fadeLayer.color;
        while(elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp(elapsed/fadeDuration, 0f, 1f);
            color.a = alpha;
            fadeLayer.color = color;

            yield return null;
        }

        color.a = 0f;
        fadeLayer.color = color;

    }
    
}
