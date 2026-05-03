using UnityEngine;

public class PigeonPatrol : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 3f;

    private bool movingRight = true;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // 물리 설정 초기화
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            // Kinematic이면서 충돌을 감지하려면 아래 설정이 핵심입니다.
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.useFullKinematicContacts = true;
        }
    }

    void Update()
    {
        // 1. 현재 방향으로 계속 이동
        Vector3 direction = movingRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    // 2. 무언가(벽)에 부딪히면 즉시 실행
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 벽(Wall)이나 장애물(Environment)에 부딪히면 방향 전환
        // 팀장님이 만드신 투명 벽의 Tag가 "Wall"이어야 합니다.
        if (collision.CompareTag("Wall") || collision.CompareTag("Obstacle"))
        {
            ChangeDirection();
        }
    }

    // 혹시 Is Trigger가 아닌 일반 Collision을 쓰실 경우를 대비
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        movingRight = !movingRight;

        // 이미지 반전 (비둘기 이미지 방향에 따라 flipX를 조절하세요)
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = movingRight;
        }
    }
}