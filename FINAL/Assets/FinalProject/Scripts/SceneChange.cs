using UnityEngine;
using UnityEngine.SceneManagement;

// "Title" scene에서 Canvas에게 할당중
public class SceneChange : MonoBehaviour
{
    // Title에서 버튼 세개에 각각 할당된 Click ~ 함수
    public void ClickStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void ClickHowTo()
    {
        SceneManager.LoadScene("HowTo");
    }

    public void ClickExit()
    {
        // 현재 유니티로 실행된 방식이 에디터일 경우 에디터를 종료시키고, 어플리케이션일 경우 어플리케이션을 종료시키는 문장
        #if UNITY_EDITOR 
            UnityEditor.EditorApplication.isPlaying = false; 
        #else 
            Application.Quit (); 
        #endif
    }
    
    // Scene 변경에 사용될 함수들
    // 타이틀로 이동
    public static void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
    }

    // 점수를 보여주는 곳으로 이동
    public static void EndGame()
    {
        SceneManager.LoadScene("End");
    }

    // 던전으로 이동
    public static void EnterTheDungeon()
    {
        SceneManager.LoadScene("Dungeon");
    }
}
