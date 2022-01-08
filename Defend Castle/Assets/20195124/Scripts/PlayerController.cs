using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 최대 탄약량과 데미지에 대한 상수
static class UserInfo
{
	public const int MaxAmmo = 30;
	public const float PlayerDamage = 20.0f;
}

public class PlayerController : MonoBehaviour 
{
	public AudioClip AttackSound;
	public AudioClip ReloadSound;
	public GameManager GM;
	private AudioSource AS;
	private float mouseRotateSpeed = 2.0f;
	private Vector3 defaultPosition;
	private Quaternion defaultRotation;
	private int curAmmo;
	private bool isReloading;
	private float reloadTimer;

	void Start()
	{
		GM = GameObject.Find("GameManager").GetComponent<GameManager>();
		AS = this.GetComponent<AudioSource>();
		defaultPosition = this.transform.position;
		defaultRotation = this.transform.rotation;
		curAmmo = UserInfo.MaxAmmo;
		isReloading = false;
		reloadTimer = 0;
	}

	void Update()
	{
		// 일시정지일땐 움직임 막기
		// 멀티에서 적의 이동과 스폰 그리고 플레이 시간은 막혀있지 않은 걸
		// 비슷하게 구현해보기 위해 만들어보았습니다.
		if(!GM.onPause)
        {
			Rotate();
			Attack();
			Reload();
			ResetPosition();
		}
	}

	public int GetCurAmmo()
    {
		return curAmmo;
    }

	// 화면 회전
	private void Rotate()
    {
		// 좌우 회전
		this.transform.Rotate(0f, Input.GetAxis("Mouse X") * mouseRotateSpeed, 0f, Space.World);
		// 상하 회전
		this.transform.Rotate(-Input.GetAxis("Mouse Y") * mouseRotateSpeed, 0f, 0f);
	}

	// 마우스 좌클릭시 공격
	// 재장전 상태가 아닐때 공격 가능
	private void Attack()
    {

		if (Input.GetMouseButtonDown(0) && !isReloading)
		{
			Ray ray;
			RaycastHit hit;
			AS.clip = AttackSound;
			AS.Play();
			curAmmo -= 1;
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray, out hit))
            {
				if(hit.transform.tag == "Enemy")
                {
					hit.transform.GetComponent<EnemyController>().TakeDamage(UserInfo.PlayerDamage);
				}
			}
		}
	}
	
	// R키를 누르거나 남은 탄약이 0 이하일 때 장전
	// 장전 시 1초간의 딜레이
	private void Reload()
    {
		if(Input.GetKeyDown(KeyCode.R) || curAmmo <= 0)
        {
			if(!isReloading)
            {
				isReloading = true;
				AS.clip = ReloadSound;
				AS.Play();
			}
        }

		if(isReloading)
        {
			reloadTimer += Time.deltaTime;

			if(reloadTimer >= 1.0f)
            {
				reloadTimer = 0;
				curAmmo = UserInfo.MaxAmmo;
				isReloading = false;
            }
        }
    }

	// 키보드의 F를 누르면 카메라를 초기 설정으로 변경
	private void ResetPosition()
    {
		// 키보드의 R키를 누르면 바라보는 방향 원위치
		if (Input.GetKeyDown(KeyCode.F))
		{
			this.transform.position = defaultPosition;
			this.transform.rotation = defaultRotation;
		}
	}
}
