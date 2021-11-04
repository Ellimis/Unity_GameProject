using UnityEngine;

// "Game" scene에서 mine->Gate에게 할당중
public class MovetoDungeon : MonoBehaviour
{   
    // 인스펙터 창에서 직접 넣어주기
    [SerializeField] private GameObject canvas = null;

    private void OnTriggerEnter(Collider other) 
    {
        // 던전을 발견하면 그 보상(?)으로 시간 추가
        canvas.GetComponent<TextControl>().plusTime(30.0f);
        // 시간과 점수 그리고 팁을 가지고 바꿀 수 있는 MainCanvas 오브젝트를 Scene 변경시 삭제하지 않도록 지정
        DontDestroyOnLoad(canvas);
        SceneChange.EnterTheDungeon();
    }
}
