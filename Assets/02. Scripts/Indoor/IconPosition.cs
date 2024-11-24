using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconPosition : MonoBehaviour
{
    [Header("지도에 아이콘 표시")]
    [Tooltip("플레이어 위치")]
    [SerializeField]
    private Transform PlayerPosition;

    [Tooltip("아이콘 오브젝트")]
    [SerializeField]
    private GameObject IconObject;

    [Tooltip("아이콘 높이 조정")]
    [SerializeField]
    private float iconHeight = 2f;

    private void Update()
    {
        if (PlayerPosition != null && IconObject != null)
        {
            // 플레이어의 위치를 기반으로 아이콘을 설정
            Vector3 iconPosition = PlayerPosition.position;
            iconPosition.y += iconHeight; // 플레이어 위에 띄우기 위해 y 값을 설정

            // 아이콘을 플레이어 위치 위로 이동시킵니다.
            IconObject.transform.position = iconPosition;
        }
    }
}
