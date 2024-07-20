using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{
    public static int maxLives = 3;
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
        
        currentLives = maxLives;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost") && !playerController.canDodge)
        {
            audioSource.clip = deathSFX;
            audioSource.Play();

            playerController.canMove = false;
            selectSystem.SetActive(false); // Disable attacking.

            rb.velocity = Vector2.zero;
            Vector2 dif = (transform.position - collision.transform.position).normalized;
            Vector2 force = dif * 10f;
            rb.AddForce(force, ForceMode2D.Impulse);

            // Zoom the camera in.
            Camera.main.GetComponent<CamZoom>().zoomIn = true;

            animator.SetFloat(velocityHash, 0);
            StartCoroutine(GameOver());
        }
        else if(collision.gameObject.CompareTag("Ghost") && playerController.canDodge)
        {
            StartCoroutine(SlowMo());
        }
    }

    IEnumerator SlowMo()
    {
        // Slow down time.
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Slow down the BGM.
        float lastPitch = GameObject.FindGameObjectWithTag("Music Manager").GetComponent<AudioSource>().pitch;
        GameObject.FindGameObjectWithTag("Music Manager").GetComponent<AudioSource>().pitch = 0.6f;

        // Zoom the camera in.
        Camera.main.GetComponent<CamZoom>().zoomIn = true;

        yield return new WaitForSeconds(.5f);

        // Resume time.
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;

        // Return BGM to normal speed.
        GameObject.FindGameObjectWithTag("Music Manager").GetComponent<AudioSource>().pitch = lastPitch;

        // Zoom camera back out.
        Camera.main.GetComponent<CamZoom>().zoomIn = false;
    }

    IEnumerator GameOver()
    {
        if(currentLives-1 < 0)
        {
            Debug.Log("Game Over!");
            GameObject.FindGameObjectWithTag("Music Manager").GetComponent<MusicManager>().startMusic = false;
            GameObject.FindGameObjectWithTag("Music Manager").GetComponent<AudioSource>().Stop();

            yield return new WaitForSeconds(1f);
            
            maxLives = 3; // Reset lives.
            ScoreManager.instance.ResetScore(); // Reset score.
            ObjectiveManager.currentLevel = 0; // Reset current level.
            PauseMenu.autoPause = true; // Restart paused.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            maxLives--;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
