using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBall : MonoBehaviour
{
    [SerializeField]
    [Tooltip("볼 프리팹")]
    private GameObject _theBallPrefab;

    [SerializeField]
    [Tooltip("카메라 오브젝트")]
    private Transform _camObject;

    [SerializeField]
    [Tooltip("볼 발사 위치")]
    private Transform _shootPoint;

    [SerializeField]
    [Tooltip("최대 힘")]
    private float maxForce = 20.0f;

    [SerializeField]
    [Tooltip("최대 회전 힘")]
    private float maxSpin = 10f;

    [SerializeField]
    [Tooltip("풀의 크기")]
    private int poolSize = 10;

    private Queue<GameObject> _ballPool = new Queue<GameObject>(); // 오브젝트 풀
    private GameObject _currentBall;

    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;

    private Vector2 _startMousePosition;
    private Vector2 _endMousePosition;

    private void Start()
    {
        InitializePool(); // 풀 초기화
        CreateNewBall(); // 첫 번째 공 준비
    }

    private void Update()
    {
        // 터치 입력 처리 (모바일)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _endTouchPosition = touch.position;
                Vector2 swipeDirection = _endTouchPosition - _startTouchPosition;
                ShootBallWithSwipe(swipeDirection);
            }
        }

        // 마우스 입력 처리 (PC) - 테스트용
        if (Input.GetMouseButtonDown(0))
        {
            _startMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _endMousePosition = Input.mousePosition;
            Vector2 swipeDirection = _endMousePosition - _startMousePosition;
            ShootBallWithSwipe(swipeDirection);
        }
    }

    // 오브젝트 풀 초기화
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ball = Instantiate(_theBallPrefab);
            ball.SetActive(false); // 비활성화 상태로 생성
            _ballPool.Enqueue(ball);
        }
    }

    // 풀에서 공 가져오기
    private GameObject GetBallFromPool()
    {
        if (_ballPool.Count > 0)
        {
            GameObject ball = _ballPool.Dequeue();
            ball.SetActive(true);
            return ball;
        }
        else
        {
            Debug.LogWarning("풀에 공이 부족합니다. 새로 생성합니다.");
            return Instantiate(_theBallPrefab);
        }
    }

    // 공을 풀에 반환
    private void ReturnBallToPool(GameObject ball)
    {
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
        }

        ball.SetActive(false);
        ball.transform.position = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;
        _ballPool.Enqueue(ball);
    }

    // 새로운 공 준비
    private void CreateNewBall()
    {
        _currentBall = GetBallFromPool();
        _currentBall.transform.position = _shootPoint.position;
        _currentBall.transform.rotation = Quaternion.identity;
    }

    // 스와이프 방향으로 공 발사
    private void ShootBallWithSwipe(Vector2 swipeDirection)
    {
        if (_currentBall == null)
            return;

        float swipeDistance = swipeDirection.magnitude;

        Vector3 theVector = new Vector3(
            swipeDirection.x,
            swipeDirection.y,
            swipeDistance
        ).normalized;

        float force = Mathf.Clamp(swipeDistance, 0, maxForce);

        Rigidbody tR = _currentBall.GetComponent<Rigidbody>();

        tR.velocity = Vector3.zero;
        tR.angularVelocity = Vector3.zero;

        tR.AddForce(theVector * force, ForceMode.Impulse);

        Vector3 spin =
            new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized * maxSpin;

        tR.AddTorque(spin, ForceMode.Impulse);

        StartCoroutine(DeactivateBallAfterTime(_currentBall, 3f));
        _currentBall = null;
        StartCoroutine(WaitAndCreateNewBall());
    }

    // 일정 시간 후 공 비활성화
    private IEnumerator DeactivateBallAfterTime(GameObject ball, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnBallToPool(ball);
    }

    // 새로운 공 생성을 위한 대기 시간
    private IEnumerator WaitAndCreateNewBall()
    {
        yield return new WaitForSeconds(1.0f);
        CreateNewBall();
    }
}
