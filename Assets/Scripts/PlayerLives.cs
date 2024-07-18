using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{
    public int maxLives;
    public int currentLives;
    public GameObject selectSystem;
    public AudioClip deathSFX;
    private AudioSource audioSource;

    private PlayerController playerController;
    private Rigidbody2D rb;
    private Animator animator;
    private int velocityHash; // This is the Animator's Velocity parameter.

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity"); // References the animator's idle - moving blend tree value.
        playerController.canDodge = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost") && !playerController.canDodge)
        {
            Debug.Log("Hit! - 1 Life");

            audioSource.clip = deathSFX;
            audioSource.Play();

            playerController.canMove = false;
            selectSystem.SetActive(false); // Disable attacking.

            rb.velocity = Vector2.zero;
            Vector2 dif = (transform.position - collision.transform.position).normalized;
            Vector2 force = dif * 10f;
            rb.AddForce(force, ForceMode2D.Impulse);

            animator.SetFloat(velocityHash, 0);
            StartCoroutine(ReloadScene());
        }
        else if(collision.gameObject.CompareTag("Ghost") && playerController.canDodge)
        {
            StartCoroutine(SlowMo());
        }
    }

    IEnumerator SlowMo()
    {
        Time.timeScale = 0.4f;
        float lastPitch = GameObject.FindGameObjectWithTag("Music Manager").GetComponent<AudioSource>().pitch;
        GameObject.FindGameObjectWithTag("Music Manager").GetComponent<AudioSource>().pitch = 0.6f;
        yield return new WaitForSeconds(.5f);
        Time.timeScale = 1.0f;
        GameObject.FindGameObjectWithTag("Music Manager").GetComponent<AudioSource>().pitch = lastPitch;

    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
