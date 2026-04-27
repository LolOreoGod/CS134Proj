using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    public CanvasGroup canvas;
    private float fadeIn = 0.03f;
    private float fadeOut = 0.15f;
    private Coroutine flashRoutine;
    public bool isFlashing;


    private void Awake() {
        canvas.alpha = 0f;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }

    public void Flash() {
        if (flashRoutine != null) {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine() {
        // Fade in
        isFlashing = true;
        float time = 0f;
        /*while (time < fadeIn) {
            time += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(0f, 0.45f, time/fadeIn);
            yield return null;
        }*/

        canvas.alpha = 0.45f;

        // Fade out
        time = 0f;
        while (time < fadeOut) {
            time += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(0.45f, 0f, time/fadeIn);
            yield return null;
        }
        canvas.alpha = 0f;

        isFlashing = false;
        flashRoutine = null;   
    }
}
