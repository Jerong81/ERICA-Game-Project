using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 5f;
    public int targetScore = 80;

    [Header("피곤도 설정")]
    private int fatigue = 100;
    public float knockbackForce = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private int score = 0;
    private Vector3 startPosition;
    private bool isLevelComplete = false;
    private bool isKnockback = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        startPosition = transform.position;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateFatigueUI(fatigue);
        }
    }

    void Update()
    {
        if (isLevelComplete || isKnockback)
        {
            moveInput = Vector2.zero;
        }
        else
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
        }

        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);
    }

    void FixedUpdate()
    {
        if (!isKnockback)
        {
            rb.linearVelocity = moveInput.normalized * moveSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            ItemInfo info = collision.GetComponent<ItemInfo>();
            if (info != null && UIManager.Instance != null)
            {
                UIManager.Instance.AddPiece(info.pieceIndex);
                score += 10;
                Destroy(collision.gameObject);
            }

            if (score >= targetScore && !isLevelComplete)
            {
                isLevelComplete = true;
                if (UIManager.Instance != null)
                    UIManager.Instance.ShowCompleteUI();
                StartCoroutine(WaitAndNextScene());
            }
        }

        if (collision.CompareTag("Obstacle"))
        {
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
            TakeDamage(knockbackDir);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
            TakeDamage(knockbackDir);
        }
    }

    void TakeDamage(Vector2 direction)
    {
        fatigue -= 10;

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateFatigueUI(fatigue);

        if (fatigue <= 0)
        {
            RestartGame();
        }
        else
        {
            StartCoroutine(ApplyKnockback(direction));
        }
    }

    IEnumerator ApplyKnockback(Vector2 direction)
    {
        isKnockback = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);

        isKnockback = false;
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator WaitAndNextScene()
    {
        yield return new WaitForSeconds(2.0f);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("06_Ending");
        }
    }
}