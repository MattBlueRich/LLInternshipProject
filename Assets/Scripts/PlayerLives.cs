using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{
    public int maxLives;
    public int currentLives;
    public GameObject selectSystem;

    private PlayerController playerController;
    private Rigidbody2D rb;
    private Animator animator;
    private int velocityHash; // This is the Animator's Velocity parameter.

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity"); // References the animator's idle - moving blend tree value.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ghost") && !playerController.canDodge)
        {
            Debug.Log("Hit! - 1 Life");
            playerController.canMove = false;
            selectSystem.SetActive(false); // Disable attacking.
            rb.velocity = Vector2.zero;
            Vector2 dif = (transform.position - collision.transform.position).normalized;
            Vector2 force = dif * 10f;
            rb.AddForce(force, ForceMode2D.Impulse);
            animator.SetFloat(velocityHash, 0);
            StartCoroutine(ReloadScene());
        }
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
