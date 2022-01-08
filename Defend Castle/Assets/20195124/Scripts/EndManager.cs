using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour 
{
	// 종료 Scene에서 성공일시 보일 오브젝트들
	public GameObject SuccessObject;
	public Text scoreText;
	public Text timerText;

	// 종료 Scene에서 패배일시 보일 오브젝트들
	public GameObject[] FailObjects;

	void Start () 
	{
		// 성공 화면
		if (PlayerPrefs.GetString("EndSceneName") == "Success")
		{
			SuccessObject.SetActive(true);
			scoreText.text = "Score: " + (PlayerPrefs.GetInt("CatchCount")*100 - PlayerPrefs.GetInt("MissCount")*50).ToString("0");
			timerText.text = "Time: " + PlayerPrefs.GetFloat("PlayTime").ToString("N1");
		}

		// 실패 화면
		if (PlayerPrefs.GetString("EndSceneName") == "Fail")
        {
			foreach (GameObject temp in FailObjects)
			{
				temp.SetActive(true);
			}
		}
	}
	
	// 각 버튼에 맞는 함수
	public void OnRestartButtonClick()
    {
		SceneManager.LoadScene("GameScene");
    }

	public void OnMenuButtonClick()
    {
		SceneManager.LoadScene("TitleScene");
    }
}
