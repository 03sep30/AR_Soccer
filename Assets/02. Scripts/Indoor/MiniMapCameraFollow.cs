using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    [SerializeField]
    [Tooltip("AR 카메라의 TRansform을 할당")]
    private Transform player;

    [SerializeField]
    [Tooltip("카메라의 고정된 넢이 및 오프셋")]
    private Vector3 offset;

    [SerializeField]
    [Tooltip("카메라의 XY 평면 이동")]
    private Vector2 cameraShift;

    [SerializeField]
    [Tooltip("카메라의 기본 회전 값")]
    private Vector3 cameraRotationEuler;

    private void LateUpdate()
    {
        if (player != null)
        {
            // 플레이어의 위치를 기반으로 카메라 위치를 업데이트
            Vector3 newPosition = player.position + offset;
            transform.position = newPosition;

            // XY 평면에서 오프셋 적용 (오른쪽/위로 이동)
            transform.position += new Vector3(cameraShift.x, 0f, cameraShift.y);

            // 카메라 회전 설정
            transform.rotation = Quaternion.Euler(cameraRotationEuler);
        }
    }
}
