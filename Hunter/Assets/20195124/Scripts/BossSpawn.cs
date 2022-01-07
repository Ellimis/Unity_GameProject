using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject boss;
    // 보스 재생성을 위한 변수
    private bool respawn;
    // 보스가 재생성될 시간
    private float timer;
    // 보스를 소환하기 위한 변수
    private GameObject temp;

    void Start()
    {
        respawn = false;
        timer = 0;
        PlayerPrefs.SetInt("CatchCount", 0);
    }

    void Update()
    {
        if(respawn)
        {
            timer += Time.deltaTime;

            if(timer >= 5.0f)
            {
                temp = Instantiate(boss, this.transform.position, boss.transform.rotation);
                temp.transform.SetParent(this.transform);
                respawn = false;
                timer = 0;
            }
        }
    }

    // 요청 받은 후 5초 뒤 부활, 이때 Boss.cs에 있는 정보대로 보스의 체력이 바껴서 부활
    public void RequestGenerate()
    {
        respawn = true;
    }
}
