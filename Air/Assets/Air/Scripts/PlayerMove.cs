using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public GameObject head;
    public GameObject mainCam;
    public GameObject body;

    private float cur_angle;
    private float prev_angle;
    private float delta_angle;

    public GameObject bulletPrefab;
    public Transform muzzle;
    public Slider HP_Bar;
    public Text ShootMode;
    public bool isBossDied = false;

    [SerializeField]
    private int HP = 100;
    private float moveSpeed = 3.5f;
    // 단발: 0, 3점사: 1
    private int mode = 1;

    private Vector3 StartHeadPos;
    private Vector3 StartBodyPos;
    private Quaternion StartRot;
    private float reset_timer = 0f;

    void Start()
    {
        ShootMode.text = getMode();
        StartHeadPos = this.transform.position;
        StartBodyPos = body.transform.position;
        StartRot = this.transform.rotation;
    }

    void Update()
    {
        MoveForward();
        CheckPosition();
        // 간단하게 구현해놓고 나니 VR 카드보드지는 터치 하는 것 밖에 없어서 게임이라도 빨리 끝나라고 3점사로 해두었습니다 ㅠㅠ
        //ChangeShootMode();
        Shoot();

        HP_Bar.value = HP;

        CheckBothHP();
        //ReturnToStart();
    }

    public void MoveForward()
    {
        head.transform.Translate(mainCam.transform.forward * moveSpeed);

        cur_angle = mainCam.transform.eulerAngles.y;
        delta_angle = cur_angle - prev_angle;
        prev_angle = cur_angle;

        if(delta_angle < 0)
        {
            body.transform.rotation = Quaternion.Lerp(body.transform.rotation, Quaternion.Euler(body.transform.eulerAngles.x, body.transform.eulerAngles.y, 45), Time.deltaTime);
        }
        else if(delta_angle > 0)
        {
            body.transform.rotation = Quaternion.Lerp(body.transform.rotation, Quaternion.Euler(body.transform.eulerAngles.x, body.transform.eulerAngles.y, -45), Time.deltaTime);
        }
        else
        {
            body.transform.rotation = Quaternion.Lerp(body.transform.rotation, Quaternion.Euler(body.transform.eulerAngles.x, body.transform.eulerAngles.y, 0), Time.deltaTime);
        }
    }

    public void CheckPosition()
    {
        // 활동 가능 범위
        // x: -4000 ~ 4000
        // y:   150 ~ 2000
        // z: -4000 ~ 4000

        float X = this.transform.position.x;
        float Y = this.transform.position.y;
        float Z = this.transform.position.z;

        float limitPosXZ = 4000.0f;

        // X pos
        if(X <= -4000)
        {
            this.transform.position = new Vector3(limitPosXZ, Y, Z);
        }
        else if (4000 <= X)
        {
            this.transform.position = new Vector3(-limitPosXZ, Y, Z);
        }

        // Y pos
        if (Y <= 150)
        {
            this.transform.position = new Vector3(X, 150.0f, Z);
        }
        else if (2000 <= Y)
        {
            this.transform.position = new Vector3(X, 2000.0f, Z);
        }

        // Z pos
        if (Z <= -4000)
        {
            this.transform.position = new Vector3(X, Y, limitPosXZ);
        }
        else if (4000 <= Z)
        {
            this.transform.position = new Vector3(X, Y, -limitPosXZ);
        }
    }

    public void decreaseHP(int damage)
    {
        this.HP -= damage;
    }

    // 몸체에 Kinetic을 키면 배랑 부딪혔을 때 충돌이 안되고 그렇자니 끄면 폭탄에 의해서 가끔 이상하게 튕겨져 나가네요..
    // head에 kinetic을 키고 몸체는 꺼두고 해서 되는 줄 알았더니 다시 비슷하게 일어나더라고요..
    // 그래서 리셋 버튼을 이용해서 튕겨져 나가면 다시 돌아와서 할 수 있게 하려고 했는데
    // 컴퓨터 게임이면 R 키를 이용해서 탈출시킬텐데 구현해놓고 보니 또 터치만 가능한걸 까먹었네요..
    // 터치로만 하려니까 제약되는 사항이 많은게 아쉽네요..
    public void InitPosition()
    {
        this.transform.position = StartHeadPos;
        this.transform.rotation = StartRot;

        body.transform.position = StartBodyPos;
        body.transform.rotation = StartRot;
    }

    private void ReturnToStart()
    {
        // 긴급 탈출용 코드, 가끔 이상하게 움직이거나 이상하게 회전할 시 이용
        if (Input.GetMouseButton(0))
        {
            reset_timer += Time.deltaTime;

            if (reset_timer >= 2.0f)
            {
                InitPosition();
                reset_timer = 0;
            }
        }
    }

    private string getMode()
    {
        if (mode == 0) return "단발";
        else return "점사";
    }

    private void ChangeShootMode()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (mode == 0)
            {
                mode = 1;
                ShootMode.text = getMode();
            }
            else
            {
                mode = 0;
                ShootMode.text = getMode();
            }
        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mode == 0)
            {
                GameObject bullet = GameObject.Instantiate(bulletPrefab, muzzle.position, muzzle.rotation) as GameObject;
                GameObject.Destroy(bullet, 3f);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    GameObject bullet = GameObject.Instantiate(bulletPrefab, muzzle.position, muzzle.rotation) as GameObject;
                    GameObject.Destroy(bullet, 3f);
                }
            }
        }
    }
    
    private void CheckBothHP()
    {
        if (HP <= 0)
        {
            // 죽으면 메뉴화면으로 이동
            SceneManager.LoadScene("Start");
        }

        if (isBossDied)
        {
            SceneManager.LoadScene("End");
        }
    }
}
