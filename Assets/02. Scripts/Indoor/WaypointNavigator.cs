using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{

    [SerializeField]
    [Tooltip("경로를 나타내는 웨이포인트들")]
    private Transform[] waypoints;

    [SerializeField]
    [Tooltip("플레이어의 위치를 나타내는 오브젝트")]
    private GameObject playerIcon;

    [SerializeField]
    [Tooltip("목적지 아이콘")]
    private GameObject destinationIcon;

    [SerializeField]
    [Tooltip("라인 랜더러")]
    private LineRenderer lineRenderer;

    [SerializeField]
    [Tooltip("라인 랜더러 높이")]
    private float lineRendererHeight = 1.5f;

    private int currentWaypointIndex = 0;


    private void Start()
    {
        if (waypoints.Length > 0)
        {
            UpdateDestinationIcon();
            DrawPath(); // LineRenderer로 경로 표시
        }
        else
        {
            Debug.LogError("경로를 나타내는 웨이포인트가 없습니다.");
        }
    }

    private void Update()
    {
        // AR 카메라 위치 업데이트 (가상 플레이어 위치에 반영)
        UpdatePlayerIcon();

        // 플레이어가 현재 Waypoint에 도달했는지 확인
        if (Vector3.Distance(playerIcon.transform.position, waypoints[currentWaypointIndex].position) < 1f)
        {
            Debug.Log("Waypoint " + currentWaypointIndex + "에 도달");
            MoveToNextWaypoint();
        }

    }


    private void UpdatePlayerIcon()
    {
        playerIcon.transform.position = Camera.main.transform.position;
    }

    private void UpdateDestinationIcon()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            destinationIcon.transform.position = waypoints[currentWaypointIndex].position;
            destinationIcon.SetActive(true);
        }
        else
        {
            destinationIcon.SetActive(false);
            Debug.Log("목적지에 도달했습니다!");
        }
    }

    private void MoveToNextWaypoint()
    {
        currentWaypointIndex++;
        if (currentWaypointIndex < waypoints.Length)
        {
            UpdateDestinationIcon();
        }
        else
        {
            Debug.Log("모든 Waypoints를 완료했습니다.");
        }

    }

    private void DrawPath()
    {
        lineRenderer.positionCount = waypoints.Length;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 adjustedPosition = waypoints[i].position;
            adjustedPosition.y = lineRendererHeight;
            lineRenderer.SetPosition(i, adjustedPosition);
        }
    }
}
