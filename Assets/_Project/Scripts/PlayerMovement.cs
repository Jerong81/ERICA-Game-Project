using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 5f;
    public TextMeshProUGUI scoreText;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private int score = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // 중력 0으로 설정

        // 시작할 때 점수판 초기화
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void Update()
    {
        // 입력 받기
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // 이동 처리
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    // 아이템(Is Trigger 체크된 것)과 부딪혔을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            score += 10;
            if (scoreText != null)
                scoreText.text = "Score: " + score; // 화면 점수 갱신

            Debug.Log("현재 점수: " + score);
            Destroy(collision.gameObject); // 아이템 삭제
        }
    }

    // 장애물(Is Trigger 체크 안 된 것)과 부딪혔을 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("장애물 충돌! 리스폰합니다.");
            transform.position = Vector2.zero; // 시작 지점으로 리스폰
        }
    }
}