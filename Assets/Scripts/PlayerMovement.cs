using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도


    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        Rotate();
        Move();
        float moveMagnitude = new Vector2(playerInput.moveX, playerInput.moveZ).magnitude;
        playerAnimator.SetFloat("Move", moveMagnitude);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move() {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDircetion = cameraForward * playerInput.moveZ + cameraRight * playerInput.moveX;
        moveDircetion = moveDircetion.normalized;

        Vector3 moveDistance = moveDircetion * moveSpeed * Time.deltaTime;

        playerRigidbody.MovePosition(playerRigidbody.position +  moveDistance);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, transform.position);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);

            Vector3 lookDirection = point - transform.position;

            lookDirection.y = 0;

            playerRigidbody.MoveRotation(Quaternion.LookRotation(lookDirection));
        }

    }
}