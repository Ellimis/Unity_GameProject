using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public Slider HP_Bar;
    public Text BossHP;
    
    private float HP = 1.0f;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Head");   
    }
    
    void Update()
    {
        HP_Bar.value = HP;
        BossHP.text = ((int)(HP*10000)).ToString();
    }

    public void decreaseHP(float damage)
    {
        //HP_Bar.value -= damage;
        HP -= damage;

        if(HP <= 0)
        {
            player.GetComponent<PlayerMove>().isBossDied = true;
        }
    }

    // 플레이어가 배에 부딪혔을 때
    private void OnCollisionEnter(Collision collision)
    {
        player.GetComponent<PlayerMove>().decreaseHP(30);
        player.transform.position = new Vector3(0, 300, -3000);
        player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
