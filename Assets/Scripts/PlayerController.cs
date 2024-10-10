using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;

    // 무기
    public GameObject[] weapons;
    public int currentWeapon;

    public float walkingSpeed = 7f;


    // 마우스 방향에 따른 카메라 회전
    // 마우스 감도
    public float mouseSens = 10f;
    // 카메라가 바라보는 방향 
    public Transform cameraTransform;
    // 회전한 각도
    private float verticalAngle;
    private float horizontalAngle;


    // 낙하 
    // 현재 추락하고있는 속도
    private float verticalSpeed;

    // 점프
    public float jumpSpeed = 5f;
    bool isGroundedLocalCheck;
    float groundedTimer;


    // Start is called before the first frame update
    void Start()
    {
        // 커서 처리 : 화면을 못 벗어나게 
        Cursor.lockState = CursorLockMode.Locked;
        // 커서 포인터를 안보이게 처리 
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();


        // 수직 각도는 0 으로 맞춘다(스폰됐는데 옆으로 기우뚱 하는 상태면 안되겠지 ? )  
        verticalAngle = 0; // == Player의 transform.rotation.x
        // 수평 각도(좌우)는 0이 아닐 수도 있다 (내가 캐릭터를 배치하고 각도를 바꿔놓으면 다를 수 있음
        // == Player의 transform.rotation.y값 
        horizontalAngle = transform.localEulerAngles.y; // player 캐릭터의 y축 각도를 가져옴 (좌우 어느 방향을 보고있는지 )

        // 낙하 속도 초기화 
        verticalSpeed = 0;

        isGroundedLocalCheck = false;
        groundedTimer = 0;

        currentWeapon = 0;
    }

    private void Update()
    {
        // 1. 방향 입력받기 : 입력받은 방향
        // "Horizontal" : 방향키, a, d
        // "Vertical" : 방향키,w,s 
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (move.magnitude > 1)
        {
            // 벡터의 크기를 1로 맞춰준다 .
            // 왜 ? 대각선으로 움직이면 더 빨라지는 상황을 막기 위해 
            move.Normalize();
        }

        // 방향에 속도를 곱해준다
        move = move * walkingSpeed * Time.deltaTime;
        // 캐릭터가 바라보는 방향이 계속해서 바뀌므로 캐릭터가 바라보는 방향 기준으로 move 방향벡터를 변환한다.
        // 안할경우 바라보는 방향과 무관하게 움직임 
        move = transform.TransformDirection(move);

        // 2. 이동 
        characterController.Move(move);



        // 3. 좌우 마우스 입력
        float turnPlayer = Input.GetAxis("Mouse X") * mouseSens;
        // 마우스로 움직인 값만큼 수평 회전 각도에 더해준다  
        horizontalAngle += turnPlayer;

        // 각도가 0 ~ 360을 유지하도록 설정 
        // 예: 360도를 회전했다면 -360을 더해서 0도로 만들어준다 
        if (horizontalAngle > 360) horizontalAngle -= 360;
        if (horizontalAngle < 0) horizontalAngle += 360;


        // 3-1. 플레이어의 각도(rotation) 변경 -----------

        // 현재 플레이어의 rotation 값을 가져온다 
        Vector3 currentAngle = transform.localEulerAngles;

        // y축 값만 마우스로 입력받은 값을 넣는다.
        // 나머지는 현재값 유지
        // 참고: 좌우 이동하려면 y축을 회전시켜야함 
        currentAngle.y = horizontalAngle;

        // 플레이어의 rotation값을 변경한다. 
        transform.localEulerAngles = currentAngle;

        // transform.localEulerAngles.y 요런식으로 사용 못하고 통째로 가져와서 바꾼 뒤 다시 넣어주는 방식을 사용해야 한다 
        //  -----------


        // 4. 상하 마우스 입력 
        float turnCam = Input.GetAxis("Mouse Y") * mouseSens;

        // 마우스로 움직인 값만큼 각도를 빼준다 .
        // 왜 ? 마우스 커서와 실제 각도의 +,-값이 반대이기 때문
        // 마우스 커서가 위(+)로 갈수록 실제 x축 각도는 -값을 가진다 
        verticalAngle -= turnCam;

        // verticalAngle의 최소값과 최대값 설정
        // 위나 아래를 바라봤을 때 막 360도 돌아가버리면 안되잖아 ~ 
        verticalAngle = Mathf.Clamp(verticalAngle, -89f, 89f);
        // 카메라의 현재 각도를 가져온다 
        currentAngle = cameraTransform.localEulerAngles;
        // 카메라의 x축에만 마우스로 입력받은 값을 넣는다 
        currentAngle.x = verticalAngle;
        cameraTransform.localEulerAngles = currentAngle;

        // TODO Q. 좌우 화면은 transform으로 조절하는데 상하 화면은 cameraTransform을 이용하는가 ? 

        // 41강. 낙하
        // - 가 붙으면 아래로 떨어진다. (-값이 점점 커질수록 점점 빨라짐)
        // 중력 가속도는 10으로 고정 (떨어지는 속도를 설정)
        verticalSpeed -= 10 * Time.deltaTime;
        if (verticalSpeed < -10)
        {
            // 중력가속도는 -10으로 고정
            // 물체가 추락할 때 속도가 점점 빨라지다가 특정 속도에 도달하면 그 속도를 유지한다 . 그 작업을 해주는 것
            // 만약 중력 가속도가 계속해서 증가한다면 떨어지는 빗방울도 무한대로 빨라질거야 
            verticalSpeed = -10;
        }

        // 수직 방향의 방향벡터 설정 (떨어지는 방향) 
        Vector3 verticalMove = new Vector3(0, verticalSpeed, 0);
        verticalMove = verticalMove * Time.deltaTime;

        // 이동.
        // return값으로 충돌 정보가 담겨있는 CollisionFlags를 받아온다 
        // CollisionFlags: 어딘가에 부딪혔는지 아닌지를 알 수 있다 ! 
        CollisionFlags flags = characterController.Move(verticalMove);

        // flags와  CollisionFlags.Below을 AND 연산 한다 .
        // flags안에 below가 있는가 ? 를 체크 
        if ((flags & CollisionFlags.Below) != 0)
        {
            // 바닥하고 부딪혔다면 speed = 0
            verticalSpeed = 0;
        }


        // 컨트롤러 = 땅에 안붙어있어 !! 
        if (!characterController.isGrounded)
        {
            // 로컬체크 = 땅에 붙어있는데 ? 
            if (isGroundedLocalCheck)
            {
                // 얼마나 붙어있는지 시간 측정 
                groundedTimer += Time.deltaTime;
                // 안 붙어있는 시간이 0.5초 이상 넘어가면 
                if (groundedTimer > 0.5f)
                {
                    // 0.5초 이상 컨트롤러가 땅에 안붙어있다고 하면 
                    isGroundedLocalCheck = false;
                }
            }
        }
        else
        {
            // 한번이라도 땅에 붙어있다고 했으니까 
            isGroundedLocalCheck = true;
            groundedTimer = 0;
        }


        // 점프 : GetKeyDown (X) , GetButtonDown (O)
        if (isGroundedLocalCheck && Input.GetButtonDown("Jump"))
        {
            isGroundedLocalCheck = false;
            verticalSpeed = jumpSpeed; // TODO 어케 이것만으로 점프가 되는건지 ? 
        }

        if (Input.GetButtonDown("ChangeWeapon"))
        {
            // 현재 값에서 +1
            // 배열의 범위를 벗어난 경우 0 으로 세팅
            currentWeapon++;
            if (currentWeapon >= weapons.Length)
            {
                currentWeapon = 0;
            }
            UpdateWeapon();
        }

    }
    private void UpdateWeapon()
    {
        // 우선 모든 무기를 꺼
        foreach (GameObject w in weapons)
        {
            w.SetActive(false);
        }

        weapons[currentWeapon].SetActive(true);

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
