using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public Slider hpBar;
	public Gradient gradient;
	public Image fill;
	public Text playTime;
	public Text catchCount;
	public Text leftAmmo;
	public GameObject PauseMenu;
	public bool onPause;
	private float maxHP;
	private float curHP;
	private float playTimer;
	private PlayerController player;
	private EnemySpawn spawn;

	void Start () 
	{
		maxHP = 100.0f;
		curHP = 100.0f;
		hpBar.value = curHP / maxHP;
		fill.color = gradient.Evaluate(1f);
		playTimer = 0;
		onPause = false;
		player = GameObject.Find("Player").gameObject.GetComponent<PlayerController>();
		spawn = GameObject.Find("SpawnPoint").gameObject.GetComponent<EnemySpawn>();
	}
	
	void Update () 
	{
		playTimer += Time.deltaTime;

		UpdateHP();
		UpdateTimer();
		UpdateCount();
		UpdateAmmo();

		// 잡은 수 + 놓친 수 >= 스폰 가능한 최대 적의 수일 때 승리
		// 성의 체력이 다 닳으면 패배
        if(PlayerPrefs.GetInt("CatchCount") + PlayerPrefs.GetInt("MissCount") >= spawn.GetMaxEnemyCount())
        {
			// 잡으면 100점
			// 놓치면 -50점
			//Debug.Log("잡은 수 : " + PlayerPrefs.GetInt("CatchCount").ToString());
			//Debug.Log("놓친 수 : " + PlayerPrefs.GetInt("MissCount").ToString());
			//Debug.Log("시간: " + playTimer);
			PlayerPrefs.SetFloat("PlayTime", playTimer);
			SceneChange("Success");
		}

		// 일시정지 메뉴 불러오기
		if(Input.GetKeyDown(KeyCode.Escape))
        {
			Pause();
        }
	}

	// 성 체력 닳기, 다 닳으면 종료 Scene에서 패배 화면
	public void DecreaseHP(float damage)
    {
		curHP -= damage;

		if(curHP <= 0)
        {
			SceneChange("Fail");
        }
    }

	// 일시정지 누르면 숨겨둔 패널 보이기
	public void Pause()
	{
		PauseMenu.SetActive(true);
		onPause = true;
	}

	// 패널에 있는 각 버튼에 맞는 함수 할당을 위한 함수들
	public void OnContinueButtonClick()
	{
		PauseMenu.SetActive(false);
		onPause = false;
	}

	public void OnMenuButtonClick()
	{
		SceneManager.LoadScene("TitleScene");
	}

	public void OnExitButtonClick()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit(); // 어플리케이션 종료
#endif
	}

	// 성 체력 닳은 피해량이 점차 적용되도록
	private void UpdateHP()
    {
		hpBar.value = Mathf.Lerp(hpBar.value, curHP / maxHP, Time.deltaTime*10);
		fill.color = gradient.Evaluate(hpBar.normalizedValue);
    }

	// 타이머 업데이트
	private void UpdateTimer()
    {
		playTime.text = playTimer.ToString("N1");
	}

	// 잡은 수 업데이트
	private void UpdateCount()
    {
		catchCount.text = PlayerPrefs.GetInt("CatchCount").ToString();
	}

	// 남은 탄약 수 업데이트
	private void UpdateAmmo()
    {
		leftAmmo.text = player.GetCurAmmo().ToString() + " / 30";
    }

	// 종료 Scene에서 승리일지 패배일지 정함
	private void SceneChange(string SceneName)
    {
		PlayerPrefs.SetString("EndSceneName", SceneName);
		SceneManager.LoadScene("EndScene");
    }
}
