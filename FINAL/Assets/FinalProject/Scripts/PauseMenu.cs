using UnityEngine;

// esc키를 눌러 메뉴를 닫거나 버튼을 누르면 메뉴를 닫거나 게임을 나갈 수 있게 만들기위한 스크립트
// "Game" scene에서 MainCanvas->PauseMenu 에게 할당중
public class PauseMenu : MonoBehaviour
{
    // 일시정지가 되었을 경우 캐릭터의 움직임을 막기 위한 bool 변수
    public static bool canPlayerMove = true;
    // Pause Menu로 보일 UI
    [SerializeField] private GameObject BaseUI = null;
    
    void Update()
    {
        // 게임 중 Esc 키를 누르면 일시정지 화면 출력
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(canPlayerMove)
                OpenMenu();
            else
                // 메뉴의 버튼 종류가 적어서 Esc키를 다시 눌렀을 때,
                // Continue 버튼을 눌렀을 때가 동일하므로 함수는 하나만 만들었습니다.
                CloseMenu();
        }
    }

    private void OpenMenu()
    {
        // 플레이어의 움직임 제한
        canPlayerMove = false;
        // UI 표시
        BaseUI.SetActive(true);
        // 시간이 흐르지 않도록 설정
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        // 플레이어의 움직임 제한 해제
        canPlayerMove = true;
        // UI 가리기
        BaseUI.SetActive(false);
        // 시간이 다시 정상적으로 흐를 수 있게 설정
        Time.timeScale = 1f;
    }

    public void ClickExit()
    {
        // 움직임과 시간의 흐름을 돌려놓고 타이틀로 이동
        canPlayerMove = true;
        Time.timeScale = 1f;
        SceneChange.ReturnTitle();
    }
}
