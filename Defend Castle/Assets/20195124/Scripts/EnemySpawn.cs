using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour 
{
	public GameObject EnemyPrefab;
	private float spawnDelay;
	private float spawnTimer;
	private int curEnemyCount;
	private int maxEnemyCount;
	private bool isAllSpawned;

	// 적이 일정 시간마다 랜덤한 위치에서 소환
	void Start () 
	{
		spawnDelay = 1.5f;
		spawnTimer = 0;
		curEnemyCount = 0;
		maxEnemyCount = 20;
		isAllSpawned = false;
	}
	
	void Update () 
	{
		SpawnEnemy();
	}

	public int GetMaxEnemyCount()
    {
		return maxEnemyCount;
    }

	private void SpawnEnemy()
    {
		if (!isAllSpawned)
		{
			spawnTimer += Time.deltaTime;

			if (spawnTimer >= spawnDelay)
			{
				// 연결된 오브젝트 위치
				// (-10, 20, -175)
				// 소환할 적 위치
				// (-40~20, 20, -200~-150)
				// 랜덤 적 위치 범위
				// -30~30, -25~25
				float X = Random.Range(-30, 31);
				float Z = Random.Range(-25, 26);

				spawnTimer = 0;
				curEnemyCount++;
				Instantiate(EnemyPrefab, new Vector3(this.transform.position.x + X, 20.0f, this.transform.position.z + Z), Quaternion.identity);

				// 모두 소환되면 소환을 막기 위한 조건문
				if (curEnemyCount >= maxEnemyCount) isAllSpawned = true;
			}
		}
	}
}
