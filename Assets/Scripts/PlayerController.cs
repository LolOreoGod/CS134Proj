using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public InputActionReference interactAction;
    public int maxHealth = 100;
    public int currentHealth;
    private int prevHealth;
    private GameObject currentDoor;
    private bool isDoorOpen = false;

    public AudioSource doorSound;
    public AudioSource doorCloseSound;

    public LevelManager levelManager;
    public AudioSource hitSound;
    public DamageFlash damageUI;
    public Slider healthBar;
    public Image healthBarImage;
    public GameObject damageFilter;
    public Image damageFilterImage;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 100;
        prevHealth = currentHealth;
        UpdateHealthBar();
    }


    void OnEnable()
    {
        interactAction.action.Enable();
    }

    void OnDisable()
    {
        interactAction.action.Disable();
    }



    // Update is called once per frame
    void Update()
    {
        if (currentDoor != null && interactAction.action.triggered)
        {
            Animator anim = currentDoor.GetComponent<Animator>();
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            // Block input while animation is playing
            if (stateInfo.normalizedTime < 1f)
            {
                return;
            }
            Debug.Log("Button pressed");
            
            if (anim != null)
            {
                if (isDoorOpen)
                {
                    anim.SetTrigger("Close");
                    anim.ResetTrigger("Open");
                    isDoorOpen = false;
                    StartCoroutine(PlayCloseSound());
                }
                else
                {
                    anim.SetTrigger("Open");
                    anim.ResetTrigger("Close");
                    isDoorOpen = true;
                    doorSound.Play();
                }
            }
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Door"))
        {
            Debug.Log("In door");
            currentDoor = col.gameObject;
        }
        if(col.CompareTag("ExitDoor"))
        {
            Debug.Log("DISPLAY END LEVEL SCREEN");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Door"))
        {
            currentDoor = null;
        }
    }

    //Mace.cs calls this method
    public void TakeDamage(int damage)
    {
        //if(!damageUI.isFlashing) damageUI.Flash();
        hitSound.time = 0.09f;
        hitSound.Play();
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player HP: " + currentHealth);
        StartCoroutine(UpdateHealth());
    }

    void Die()
    {
        //call deathcam in camerapivot
        SceneManager.LoadScene("GameOverScreen");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator PlayCloseSound() {
        yield return new WaitForSeconds(1.5f);
        doorCloseSound.Play();
    }

    private void UpdateHealthBar() {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        if (currentHealth <= 25) {
            healthBarImage.color = new Color32(255, 0, 0, 255);
            damageFilter.SetActive(true);
        } else {
            healthBarImage.color = new Color32(33, 255, 0, 255);
        }
    }

    IEnumerator UpdateHealth() {
        while (healthBar.value > currentHealth) {
            prevHealth -= 1;
            healthBar.value = prevHealth;
            yield return null;

            if (healthBar.value <= 0) {
                StartCoroutine(FadeAfterDeath());
            }
        }
        UpdateHealthBar();
    }

    IEnumerator FadeAfterDeath() {
        float fadeDuration = 0.5f;
        float elapsed = 0f;
        Color color = damageFilterImage.color;
        float startingAlpha = damageFilterImage.color.a;
        while(elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp(startingAlpha + elapsed/fadeDuration, 0f, 1f);
            color.a = alpha;
            damageFilterImage.color = color;

            yield return null;
        }
        color.a = 1f;
        damageFilterImage.color = color;
        Die();

    }
}
