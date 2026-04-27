using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Exitlevel : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public Animator doorAnimator;
    public AudioSource doorSound;
    public LevelManager levelManager;
    private bool opened = false;

    void Start()
    {
        doorAnimator = GetComponentInChildren<Animator>();
        
    }
    // Update is called once per frame

    void Update()
    {
        if (opened) return;

        // Remove destroyed enemies (they become null) start from end of list to prevent errors in tracking location of i
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }
        Debug.Log("Enemies left: " + enemies.Count);
        
        if (enemies.Count == 0)
        {
            OpenDoor();
            opened = true;
        }
    }

    void OpenDoor()
    {
        if (doorAnimator != null)
        { 
            Debug.Log("Level complete. Door opening");
            doorAnimator.SetTrigger("Open");
            doorSound.Play();
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.CompleteLevel();
        }
    }
}
