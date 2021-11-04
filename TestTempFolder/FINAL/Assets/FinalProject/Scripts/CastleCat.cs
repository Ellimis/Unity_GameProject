using UnityEngine;

// "Game" scene에서 Player에게 할당중
public class CastleCat : MonoBehaviour
{
    public enum STEP
    {
        NONE = -1,
        IDLE = 0,
        RUN,  // 1
    };

    // 조작에 따른 다음 상태의 입력을 받아 행동
    public STEP current_step = STEP.NONE;
    public STEP next_step = STEP.NONE;

    // 캐릭터의 움직임 속도와 회전 속도를 담당하는 변수
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float turnSpeed = 120.0f;

    // 애니메이션 상태를 선택하기 위한 변수
    // 애니메이션 끄기 : 0
    // 애니메이션 키기 : 1
    private int condition = 0;

    // 점프했는지를 확인하는 변수
    private bool isLanded = true;

    // 현재 스크립트가 연결된 오브젝트의 애니메이션과 물리를 담당하는 컴퍼넌트를 다루기 위한 변수
    private Animator m_Animator;
    private Rigidbody m_Rigidbody;

    void Start()
    {
        m_Animator = this.gameObject.GetComponent<Animator>();
        m_Rigidbody = this.gameObject.GetComponent<Rigidbody>();

        // 초기 시작 상태는 idle 이여야 하므로 next_step = STEP.IDLE
        next_step = STEP.IDLE;
    }

    void Update()
    {
        if(PauseMenu.canPlayerMove)
        {
            // 현재 공중에 있는지 확인하는 메소드
            CheckLanded(); 

            // 어느 방향으로 이동하고 있음을 알 수 있는 작업
            // w(↑) :  1 | s(↓) : -1
            // a(←) : -1 | d(→) :  1
            float vertical = Input.GetAxisRaw("Vertical"); 
            float horizontal = Input.GetAxisRaw("Horizontal");

            // vertical이 0에 가까우면 true, 가깝지 않으면 false
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

            if(hasVerticalInput) // 움직임이 감지됬을 때,
            {
                next_step = STEP.RUN;
                condition = 1;
            }
            else // 이동에 대한 입력이 없다면 기본 상태를 idle로 설정
            {
                next_step = STEP.IDLE;
                condition = 0;
            }

            if(next_step != STEP.NONE)
            {
                current_step = next_step;
                next_step = STEP.NONE;

                // 현재 행해야 할 상태에 따라 작업
                switch(current_step) 
                {
                    case STEP.IDLE:
                        m_Animator.SetInteger("IsRunning", condition);
                        moveSpeed = 0.0f;
                        break;

                    case STEP.RUN:
                        m_Animator.SetInteger("IsRunning", condition);

                        if(vertical == 1)
                            moveSpeed = 5.0f;
                        else if(vertical == -1)
                            moveSpeed = 3.5f;
                        break;
                }
            }
            
            // 이동과 회전
            this.transform.Translate(Vector3.forward * vertical * moveSpeed * Time.deltaTime);
            this.transform.Rotate(0.0f, horizontal * turnSpeed * Time.deltaTime, 0.0f);

            // 이상한 곳에 꼇을 시 탈출, 탈출 시 시작 지점으로 이동
            if(Input.GetKeyDown(KeyCode.R))
            {
                // 초기값
                this.transform.position = new Vector3(20, 0, 0);
                this.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            }

            if(Input.GetButtonDown("Jump"))
            {
                if(isLanded) // 착지한 상태에서만 점프할 수 있게 작업
                {
                    m_Rigidbody.velocity = new Vector3(0, moveSpeed, 0);
                }
            }
        }
    }

    private void CheckLanded() 
    {
        Vector3 current_position = this.transform.position;
        Vector3 down_position = current_position + Vector3.down * 0.1f;
        RaycastHit hit;

        // 캐릭터 바로 밑에 물체가 있을 때 착지했음으로 인식
        if(Physics.Linecast(current_position, down_position, out hit)) 
            isLanded = true;
        else 
            isLanded = false;
    }
}