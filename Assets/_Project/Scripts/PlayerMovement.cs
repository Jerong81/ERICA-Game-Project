using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 5f;
    public TextMeshProUGUI scoreText;
    public int targetScore = 10; // 인스펙터에서 맵마다 다르게 수정 가능!

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private int score = 0;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        startPosition = transform.position;

        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            score += 10;
            if (scoreText != null)
                scoreText.text = "Score: " + score;

            UnityEngine.Debug.Log("현재 점수: " + score);
            Destroy(collision.gameObject);

            if (score >= targetScore)
            {
                UnityEngine.Debug.Log("목표 점수 달성! 다음 맵으로 이동합니다.");
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
    } // ← 여기서 괄호가 꼬였던 부분을 고쳤습니다.

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            UnityEngine.Debug.Log("장애물 충돌! 리스폰합니다.");
            transform.position = startPosition;
        }
    }
}