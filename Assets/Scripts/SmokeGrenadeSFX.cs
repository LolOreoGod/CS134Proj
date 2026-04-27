using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeSFX : MonoBehaviour
{
    public AudioSource smokeSound;
    void Start()
    {
        StartCoroutine(PlaySmokeSound());
    }

    IEnumerator PlaySmokeSound() {
        smokeSound.time = 0.4f;
        smokeSound.Play();
        float startVolume = smokeSound.volume;
        while (smokeSound.volume > 0) {
            smokeSound.volume -= startVolume * Time.deltaTime/5.878f;
            yield return null;
        }
        smokeSound.Stop();
        Destroy(gameObject);
    }
    
}
