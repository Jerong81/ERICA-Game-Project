using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // 따라갈 대상 (하냥이)
    public float smoothSpeed = 0.125f; // 카메라가 따라가는 부드러움 정도
    public Vector3 offset;        // 하냥이와의 거리 (보통 Z값은 -10 유지)

    void FixedUpdate()
    {
        if (target == null) return;

        // 목표 위치 계산 (하냥이 위치 + 오프셋)
        Vector3 desiredPosition = target.position + offset;

        // 카메라를 목표 위치로 부드럽게 이동시킴 (Lerp 연산)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 최종 카메라 위치 적용
        transform.position = smoothedPosition;
    }
}