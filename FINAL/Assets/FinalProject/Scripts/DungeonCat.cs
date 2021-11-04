using UnityEngine;

// "Dungeon" scene에서 Player에게 할당중
public class DungeonCat : MonoBehaviour
{   
    // 캐릭터 이동
    private float walkSpeed = 2.0f;
    private float runSpeed = 5.0f;
    private float applySpeed = 0;
    
    // 달리고 있는지에 따라 애니메이션 상태 변화
    private bool isRun = false;

    // 카메라
    [SerializeField] private Camera Cam = null;
    // 상하 민감도
    [SerializeField] private float SensitivityX = 2.0f;
    // 좌우 민감도
    [SerializeField] private float SensitivityY = 2.0f;
    // 카메라 회전의 최대치
    private float CameraRotationLimit = 45.0f;
    // 카메라의 상하 회전에 쓸 RotationX
    private float CurrentCameraRotationX = 0;

    // 애니메이션
    private Animator m_Animator;
    private static int ON = 1;
    private static int OFF = 0;

    void Start()
    {
        // 기본은 걷기
        applySpeed = walkSpeed;
        m_Animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseMenu.canPlayerMove)
        {
            SetMode();
            Move();
            CharacterRotation();
            CameraRotation();
        }
    }

    // 걷기와 달리기에 대한 모드
    private void SetMode()
    {
        // Left Shift키를 누르고 있는 동안 달리기
        if(Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
        }

        // Left Shift키를 떼는 순간 걷기
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
        }

        // 달리기 토글
        if(Input.GetKeyDown(KeyCode.R))
        {
            isRun = !isRun;
        }
    }

    // 이동
    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical"); 

        Vector3 moveHorizontal = this.transform.right * horizontal;
        Vector3 moveVertical = this.transform.forward * vertical;

        // 대각선으로 갈 때 빨리 이동하므로
        Vector3 velocity = (moveHorizontal + moveVertical).normalized;
        
        // vertical이 0에 가까우면(멈춤) true, 가깝지 않으면(움직임) false의 Not
        // 즉, 움직일 때 true를 가지는 변수
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        
        if(hasVerticalInput)
        {
            if(isRun)
            {
                m_Animator.SetInteger("IsRunning", ON);
                m_Animator.SetInteger("IsWalking", OFF);

                // 전진과 후진 속도를 다르게 설정, 후진일 때 반감
                if(vertical == -1)
                    applySpeed = runSpeed / 2.0f;
                else if(vertical == 1)
                    applySpeed = runSpeed;
            }
            else
            {
                m_Animator.SetInteger("IsRunning", OFF);
                m_Animator.SetInteger("IsWalking", ON);

                // 전진과 후진 속도를 다르게 설정, 후진일 때 반감
                if(vertical == -1)
                    applySpeed = walkSpeed / 2.0f;
                else if(vertical == 1)
                    applySpeed = walkSpeed;
            }
        }
        else
        {
            m_Animator.SetInteger("IsRunning", OFF);
            m_Animator.SetInteger("IsWalking", OFF);
            applySpeed = 0;
        }

        // 이동
        this.transform.Translate(Vector3.forward * vertical * applySpeed * Time.deltaTime);
    }

    // 좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float RotationY = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, RotationY, 0f) * SensitivityY;

        // 회전
        this.transform.Rotate(characterRotationY);
    }

    // 상하 카메라 회전
    private void CameraRotation()
    {
        // 마우스 위아래, 회전 시에는 X축 기준 회전
        float RotationX = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = RotationX * SensitivityX;

        // += : 마우스 반전
        // -= : 기본
        CurrentCameraRotationX -= cameraRotationX;
        // 현재 카메라의 회전을 -45도와 45도 사이로만 고정
        CurrentCameraRotationX = Mathf.Clamp(CurrentCameraRotationX, -CameraRotationLimit, CameraRotationLimit);
        // 로컬 오일러 각도로 회전
        Cam.transform.localEulerAngles = new Vector3(CurrentCameraRotationX, 0f, 0f);
    }
}
