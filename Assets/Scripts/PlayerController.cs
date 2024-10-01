using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;

    public float walkingSpeed = 7f;
    public float mouseSens = 10f;

    // 카메라 
    public Transform cameraTransform;
    // 회전 각도 제한
    private float verticalAngle;
    private float horizontalAngle;

    float verticalSpeed;


    // Start is called before the first frame update
    void Start()
    {
        // 커서 처리 : 화면을 못 벗어나게 
        Cursor.lockState = CursorLockMode.Locked;
        // 커서 포인터를 안보이게 처리 
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();

        verticalSpeed = 0;
        verticalAngle = 0;
        horizontalAngle = transform.localEulerAngles.y; // player 캐릭터의 y축 각도를 가져옴 (좌우 어느 방향을 보고있는지 )
    }

    private void Update()
    {
        // 방향 입력받기
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        move = transform.TransformDirection(move);
        move = move * walkingSpeed * Time.deltaTime;

        // 이동 
        characterController.Move(move);


        // 좌우 마우스 
        float turnPlayer = Input.GetAxis("Mouse X") * mouseSens;
        horizontalAngle += turnPlayer;

        if (horizontalAngle > 360) horizontalAngle -= 360;
        if (horizontalAngle < 0) horizontalAngle += 360;


        // .. ? 41강 컨트롤러 만들기 .. 이해 안됨 
        Vector3 currentAngle = transform.localEulerAngles;
        currentAngle.y = horizontalAngle;
        transform.localEulerAngles = currentAngle;



        // 상하 마우스
        float turnCam = Input.GetAxis("Mouse Y") * mouseSens;
        verticalAngle -= turnCam;
        verticalAngle = Mathf.Clamp(verticalAngle, -89f, 89f);
        currentAngle = cameraTransform.localEulerAngles;
        currentAngle.x = verticalAngle;
        cameraTransform.localEulerAngles = currentAngle;



        // 42강. 낙하
        // - 가 붙으면 아래로 떨어진다.
        // 중력 가속도는 10으로 고정 (떨어지는 속도를 주려고 하는건가본데? )
        verticalSpeed -= 10 * Time.deltaTime;
        if (verticalSpeed < -10)
        {
            // 중력가속도는 -10으로 고정
            // 물체가 추락할 때 속도가 점점 빨라지다가 특정 속도에 도달하면 그 속도를 유지한다 . 그 작업을 해주는 것
            // 만약 중력 가속도가 계속해서 증가한다면 떨어지는 빗방울도 무한대로 빨라질거야 
            verticalSpeed =-10;
        }

        Vector3 verticalMove = new Vector3(0, verticalSpeed, 0);
        verticalMove = verticalMove * Time.deltaTime;

        // 이동하고 return값으로 CollisionFlags를 받아온다 
        // CollisionFlags: 어딘가에 부딪혔는지 아닌지를 알 수 있다 ! (충돌정보를 리턴하나봄 ) 
        CollisionFlags flags = characterController.Move(verticalMove);

        // flags와  CollisionFlags.Below을 AND 연산 한다 .
        // flags안에 below가 있는가 ? 를 체크 
        if ((flags & CollisionFlags.Below) != 0)
        {
            // 바닥하고 부딪혔다면 speed = 0
            verticalSpeed = 0;
        }

    }

    /*
    // Update is called once per frame
    void Update()
    {
        // 방향 벡터 만들기 
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // 움직임을 부드럽게 한다 
        // 속도 정규화 : 대각선 이동시 가속도 방지
        // 입력되는 방향 정보는 0~1까지 인데 1이 넘어가면 속도가 빨라지나보다 . 이점을 활용해서 1보다 커지면 정규화 해주는 듯 
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        // 바라보는 방향 변경
        //이동 방향 뿐만 아니라 이 벡터가 바라보는 방향도 같이 변경해준다 (이동하는 방향을 바라보도록) : 이건 시선처리고


        // 벡터가 바라보는 방향을 기준으로 이동할 수 있도록 direction 설정 
        move = transform.TransformDirection(move);
        move = move * walkingSpeed * Time.deltaTime;

        // 이동 
        characterController.Move(move);

    }

    */
}
