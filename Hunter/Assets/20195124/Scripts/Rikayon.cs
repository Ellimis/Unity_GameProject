using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rikayon : MonoBehaviour 
{
	public GameObject body;
	public Animator animator;
	public GameObject player;
	public GameObject projectile;
	// 2페이즈 발사체 위치를 저장하는 오브젝트
	public GameObject[] phase2;
	// 3페이즈 발사체 위치를 저장하는 오브젝트
	public GameObject[] phase3;
	// 현재 체력에 따라 페이즈를 바꾸기 위한 변수
	public GameObject HP;
	// 현재 페이즈에 따라 패턴을 바꾸기 위한 변수
	public string curPhase;
	// 페이즈 변환시 무적
	public bool isInvincibility;

	public AudioClip attackSound1;
	public AudioClip attackSound2;
	public AudioClip screamSound;
	private AudioSource AS;

	// 패턴을 랜덤하게 발동
	private int rand;
	// 페이즈 변경 시 발동
	private bool isPhaseChanged;
	// 패턴 발동
	private bool isPatternInvoked;
	// 패턴끼리의 딜레이 시간 기록
	private float patternDelay;
	// 2 페이즈 다음 패턴끼리의 딜레이
	private float[] phase2Delay = { 7.0f, 4.0f };
	// 3 페이즈 다음 패턴끼리의 딜레이
	private float[] phase3Delay = { 5.0f, 3.5f, 4.5f };
	// 위치 정보를 가져오기 위한 인덱스용 변수
	private int projectilePos;
	// 현재 최대 체력에 따른 퍼센트를 주기 위한 변수
	private int curMaxHP;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find("ARCamera");
		curMaxHP = body.GetComponent<Boss>().maxHP[PlayerPrefs.GetInt("CatchCount")];
		patternDelay = 0;
		isInvincibility = false;
		isPhaseChanged = false;
		isPatternInvoked = false;
		AS = this.gameObject.GetComponent<AudioSource>();

		// 1페이즈는 잠만 잠
		curPhase = "Phase1";
		animator.SetTrigger("Sleep");
	}

	void Update()
	{
		// 남은 체력이 30% ~ 60%일때
		if ((int)(curMaxHP * 0.3) < HP.GetComponent<HealthBar>().slider.value && HP.GetComponent<HealthBar>().slider.value <= (int)(curMaxHP * 0.6))
		{
			// 2 페이즈 진입
			if (!isPhaseChanged)
			{
				patternDelay = 0;
				CancelInvoke();
				curPhase = "Phase2";
				animator.SetTrigger("Intimidate_3");
				isInvincibility = true;
				isPhaseChanged = true;
				AS.clip = screamSound;
				AS.Play();
				Invoke("ChangeInvincibility", 7.5f);
			}
		}
		// 남은 체력이 30% 미만일 때
		else if (HP.GetComponent<HealthBar>().slider.value <= (int)(curMaxHP * 0.3))
		{
			// 3 페이즈 진입
			if (isPhaseChanged)
			{
				patternDelay = 0;
				CancelInvoke();
				curPhase = "Phase3";
				animator.SetTrigger("Intimidate_1");
				isInvincibility = true;
				isPhaseChanged = false;
				Invoke("ChangeInvincibility", 3.0f);
			}
		}

		// 현재 페이즈에 따라 패턴 변화
		switch (curPhase)
		{
			case "Phase1":
				break;

			case "Phase2":
				if (isPatternInvoked && isPhaseChanged)
				{
					// 패턴 두개 중 하나 랜덤 발동
					rand = Random.Range(0, 100) + 1;
					Debug.Log("rand : " + rand.ToString());

					isPatternInvoked = false;
					Invoke("P2P" + (rand % 2).ToString(), 3.0f);
				}

				// 패턴 발동 후 일정 시간 지나면 다음 무작위 패턴 발동
				if (!isPatternInvoked && !isInvincibility)
				{
					patternDelay += Time.deltaTime;

					if (patternDelay >= phase2Delay[(rand % 2)])
					{
						isPatternInvoked = true;
						patternDelay = 0;
					}
				}

				break;

			case "Phase3":
				if (!isPatternInvoked && !isPhaseChanged)
				{
					// 패턴 세개 중 하나 랜덤 발동
					rand = Random.Range(0, 99) + 1;
					Debug.Log("rand : " + rand.ToString());

					isPatternInvoked = true;
					Invoke("P3P" + (rand % 3).ToString(), 3.0f);
				}

				// 패턴 발동 후 일정 시간 지나면 다음 무작위 패턴 발동
				if (isPatternInvoked && !isInvincibility)
				{
					patternDelay += Time.deltaTime;

					if (patternDelay >= phase3Delay[(rand % 3)])
					{
						isPatternInvoked = false;
						patternDelay = 0;
					}
				}

				break;

			case "Die":
				// 죽으면 모두 끝내고 사라질때까지 무적으로 만듬
				patternDelay = 0;
				isInvincibility = true;
				CancelInvoke();

				break;
		}

#if UNITY_EDITOR
		// 양손 강공
		if (Input.GetKeyDown(KeyCode.Space))
		{
			animator.SetTrigger("Attack_1");
		}
		// 왼손
		if (Input.GetKeyDown(KeyCode.V))
		{
			animator.SetTrigger("Attack_2");
		}
		// 오른손
		if (Input.GetKeyDown(KeyCode.B))
		{
			animator.SetTrigger("Attack_3");
		}
		// 양손 약공
		if (Input.GetKeyDown(KeyCode.N))
		{
			animator.SetTrigger("Attack_4");
		}
		// 위로 올려치기
		if (Input.GetKeyDown(KeyCode.M))
		{
			animator.SetTrigger("Attack_5");
		}

		// 위협
		if (Input.GetKeyDown(KeyCode.H))
		{
			animator.SetTrigger("Intimidate_1");
		}
		// 포효
		if (Input.GetKeyDown(KeyCode.K))
		{
			animator.SetTrigger("Intimidate_3");
		}

		// 피 세게 달았을때
		if (Input.GetKeyDown(KeyCode.D))
		{
			animator.SetTrigger("Take_Damage_1");
		}
		// 피 약하게 달았을때
		if (Input.GetKeyDown(KeyCode.G))
		{
			animator.SetTrigger("Take_Damage_3");
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			animator.SetTrigger("Die");
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			animator.SetTrigger("Sleep");
		} 
#endif
	}

	GameObject temp;

	// Phase 2 Pattern 0
	private void P2P0()
	{
		projectilePos = 0;
		AS.clip = attackSound1;
		AS.Play();

		for (int i = 0; i < phase2.Length; i++)
		{
			animator.SetTrigger("Attack_2");
			Invoke("SpawnProjectile1", i * 1.0f);
		}
	}

	// Phase 2 Pattern 1
	private void P2P1()
    {
		animator.SetTrigger("Attack_4"); 
		AS.clip = attackSound2;
		AS.Play();

		for (int i = 0; i < phase2.Length; i++)
        {
			Instantiate(projectile, body.transform.localPosition + phase2[i].transform.position, Quaternion.identity).transform.LookAt(player.transform);
		}
	}

	// Phase 3 Pattern 0
	private void P3P0()
	{
		animator.SetTrigger("Intimidate_1");
		projectilePos = 0;
		AS.clip = attackSound1;
		AS.Play();

		for (int i = 0; i < phase2.Length; i++)
		{
			Invoke("SpawnProjectile1", i * 0.5f);
		}
	}

	// Phase 3 Pattern 1
	private void P3P1()
	{
		animator.SetTrigger("Attack_1");
		AS.clip = attackSound2;
		AS.Play();

		for (int i = 0; i < phase3.Length; i++)
		{
			Instantiate(projectile, body.transform.localPosition + phase3[i].transform.position, Quaternion.identity).transform.LookAt(player.transform);
		}
	}

	// Phase 3 Pattern 2
	private void P3P2()
	{
		animator.SetTrigger("Attack_5");
		projectilePos = 0;
		AS.clip = attackSound1;
		AS.Play();

		for (int i = 0; i < phase3.Length; i++)
		{
			Invoke("SpawnProjectile2", i * 0.3f);
		}
	}

	// 2, 3 페이즈의 패턴 1에 쓰이는 함수
	private void SpawnProjectile1()
    {
		Instantiate(projectile, body.transform.localPosition + phase2[projectilePos].transform.position, Quaternion.identity).transform.LookAt(player.transform);
		projectilePos += 1;
	}

	// 3 페이즈의 패턴 3에 쓰이는 함수
	private void SpawnProjectile2()
    {
		Instantiate(projectile, body.transform.localPosition + phase3[projectilePos].transform.position, Quaternion.identity).transform.LookAt(player.transform);
		projectilePos += 1;
	}

	// 페이즈 변화시 무적 여부 전환
	private void ChangeInvincibility()
    {
		isInvincibility = !isInvincibility;
		isPatternInvoked = !isPatternInvoked;
    }
}
