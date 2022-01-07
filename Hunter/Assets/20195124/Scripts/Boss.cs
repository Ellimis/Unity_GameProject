using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // 본체 삭제를 위한 Body
    public GameObject Body;
    // HP 포인트에 따른 UI 오브젝트
    public HealthBar hpBar;
    // 보스 최대 체력 저장
    public int[] maxHP = { 1000, 5000, 10000 };
    // 현재 보스의 남은 체력
    [SerializeField]
    private int curHP;
    // 보스를 잡은 횟수에 따른 난이도 조절을 위한 변수
    private int catchCount;
    // 기본 지급 보상
    private int[] rewards = { 1000, 2000, 4000 };
    // 클리어 시간에 따른 등급
    private int rank = 0;
    // 등급에 따른 보너스
    private int[] bossBonus = { 0, 1500, 3500, 6000 };
    // 보스를 잡는 시간을 기록하는 변수
    [SerializeField]
    private float timer = 0;
    private float speedRun;

    // Start is called before the first frame update
    void Start()
    {
        catchCount = PlayerPrefs.GetInt("CatchCount");
        curHP = maxHP[catchCount];
        hpBar.SetMaxHealth(maxHP[catchCount]);
        speedRun = 90.0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        // 페이즈 변화중이 아닐때, 무적이 아닐때
        if(!Body.GetComponent<Rikayon>().isInvincibility)
        {
            curHP -= damage;
            hpBar.SetHealth(curHP);

            if (curHP <= 0)
            {
                Store storeMenu = GameObject.Find("Store").GetComponent<Store>();
                float timeRemain = speedRun - timer;

                // 오래 걸렸을 때 추가 보상 X
                if (timeRemain <= 0)
                    rank = 0;
                // 30초 이내로 남기고 잡았을 때 추가 보상 등급 1
                else if (0 < timeRemain && timeRemain <= 30.0f)
                    rank = 1;
                // 30~60초 이내로 남기고 잡았을 때 추가 보상 등급 2
                else if (30.0f < timeRemain && timeRemain < 60.0f)
                    rank = 2;
                // 60~90초 이내로 남기고 잡았을 때 추가 보상 등급 3
                else if (60.0f < timeRemain && timeRemain < 90.0f)
                    rank = 3;

                // 보스에 따른 기본 지급 + 추가 보상 등급에 따른 지급 
                storeMenu.IncreaseMoney(rewards[catchCount] + bossBonus[rank]);
                StartCoroutine(Die());
            }
        }
    }

    // 죽으면 발동
    IEnumerator Die()
    {
        // 체력, 시간 다는거 멈추고 죽었을 때 애니메이션 설정
        curHP = 0;
        timer = 0;
        hpBar.SetHealth(curHP);
        Body.GetComponent<Rikayon>().curPhase = "Die";
        Animator animator = Body.GetComponent<Animator>();
        animator.SetTrigger("Die");

        // 잡은 횟수가 일정 이상으로 넘어가면 체력 등을 고정하기 위한 변수 조정
        if (catchCount >= 2) PlayerPrefs.SetInt("CatchCount", 2);
        else PlayerPrefs.SetInt("CatchCount", catchCount+1);

        // 죽고 5초 기다린 후 재생성을 요청
        yield return new WaitForSeconds(5.0f);
        GameObject.Find("BossTarget").GetComponent<BossSpawn>().RequestGenerate();
        Destroy(Body);
    }
}
