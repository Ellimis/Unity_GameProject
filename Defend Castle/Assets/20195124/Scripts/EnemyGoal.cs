using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoal : MonoBehaviour
{
	public GameObject enemySpawn;
	public GameObject gameManager;
	private float enemyDamage;

	// 잡은 수와 놓친 수 기록
	void Start () 
	{
		PlayerPrefs.SetInt("CatchCount", 0);
		PlayerPrefs.SetInt("MissCount", 0);
		enemyDamage = 10.0f;
	}

	// 목표 지점에 도달하면 놓친 수 증가, 성에 데미지 주고 도달한 적을 Destroy
	private void OnTriggerEnter(Collider hitObject)
    {
		if(hitObject.transform.tag == "Enemy")
        {
			gameManager.GetComponent<AudioSource>().Play();
			gameManager.GetComponent<GameManager>().DecreaseHP(enemyDamage);
			PlayerPrefs.SetInt("MissCount", PlayerPrefs.GetInt("MissCount")+1);
			Destroy(hitObject.gameObject);
		}
	}
}
