using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 적의 체력과 속도를 저장하는 상수
static class EnemyInfo
{
	public const float MaxHP = 100.0f;
	public const float Speed = 3.0f;
}

public class EnemyController : MonoBehaviour 
{
	public Slider hpBar;
	public Gradient gradient;
	public Image fill;
	private float curHP;
	private bool isDamaged;
	private Animator animator;

	// 데미지 입은 순간부터 체력바가 보이게 설정
	void Start () 
	{
		SetInitHP();
		isDamaged = false;
		animator = this.transform.GetComponent<Animator>();
	}

	void Update () 
	{
		Walk();
	}

	// 적이 입는 피해 함수
	public void TakeDamage(float damage)
    {
		if(!isDamaged)
		{
			hpBar.gameObject.SetActive(true);
			isDamaged = true;
        }

		curHP -= damage;
		UpdateHP();

		// 체력이 다 떨어지면 소리 내고 잡은 수 올리고 Destroy
		if (curHP <= 0)
        {
			PlayerPrefs.SetInt("CatchCount", PlayerPrefs.GetInt("CatchCount") + 1);
			this.GetComponent<AudioSource>().Play();
			Destroy(this.gameObject);
        }
    }

	// 초기 체력 설정
	private void SetInitHP()
	{
		curHP = EnemyInfo.MaxHP;
		hpBar.value = curHP / EnemyInfo.MaxHP;
		fill.color = gradient.Evaluate(1f);
	}

	// 적의 체력 UI 업데이트
	private void UpdateHP()
	{
		hpBar.value = curHP / EnemyInfo.MaxHP;
		fill.color = gradient.Evaluate(hpBar.normalizedValue);
	}

	// 정면으로 꾸준히 접근하는 함수
	private void Walk()
	{
		animator.SetBool("IsWalking", true);
		this.transform.Translate(Vector3.forward * EnemyInfo.Speed * Time.deltaTime);
	}
}
