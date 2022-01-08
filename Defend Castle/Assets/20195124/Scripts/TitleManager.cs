using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour 
{
	public GameObject[] Enemies;

	void Start () 
	{
		// 적이 플레이어를 바라보도록 설정
		foreach(GameObject Enemy in Enemies)
        {
			Enemy.transform.LookAt(this.transform);
        }
	}

	// UI Button이 편한데 배운거 써먹어볼겸 GUI한번 썻습니다만..
	// 확실히 에셋이 없는 상태에서는 GUI보단 UI Button이 꾸미기 편하네요 ㅠㅠ
	// UI -> Button 이용
	public void OnStartButtonClicked()
    {
		SceneManager.LoadScene("GameScene");
    }

	public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
	}

	// GUI 함수 이용
	void OnGUI()
    {
		if (GUI.Button(new Rect(150, Screen.height - 250, 200, 50), "Start"))
        {
			SceneManager.LoadScene("GameScene");
		}

		if(GUI.Button(new Rect(150, Screen.height - 175, 200, 50), "Exit"))
        {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit(); // 어플리케이션 종료
#endif
		}
	}
}
