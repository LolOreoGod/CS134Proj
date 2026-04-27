using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    public AudioSource attackSound;
    public AudioSource swingSound;
    public AudioSource investigateSound;
    public AudioSource stunnedSound;
    public AudioSource deathSound;
    public AudioSource neckSound;
    public AudioSource bodySound;

    // These functions are called by an Animation Event on the Enemy's animations
    public void PlayAttackSound() {
        attackSound.Play();
        swingSound.Play();
    }

    public void PlayInvestigateSound() {
        investigateSound.Play();
    }

    public void PlayStunnedSound() {
        stunnedSound.Play();
    }

    public void PlayNeckSound() {
        neckSound.Play();
    }

    public void PlayDeathSound() {
        deathSound.Play();
    }

    public void PlayBodySound() {
        bodySound.Play();
    }
}
