using UnityEngine;
using UnityEngine.UI;

// "HowTo" Scene에서 Other->Main Camera에게 할당중
public class ScoreDisplay : MonoBehaviour
{
    // 각 음식별 점수를 보여줄 Text
    public Text FoodScore;
    // 클릭했을 때 그곳에 물체가 있는지를 보기 위한 RaycastHit
    private RaycastHit hit;
    private float time = 0;
    private bool isClicked = false;

    void Update()
    {
        // 마우스 좌클릭시
        if(Input.GetMouseButtonDown(0))
        {
            // 클릭한 후부터 시간을 기록하기 시작
            time = 0;
            Ray screen_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 mouse_direction = screen_ray.direction;

            // 스크린 좌표를 ray로 바꿔 쐈는데 그곳에 물체가 있다면 그 물체의 이름을 ChangeText 메소드에 보낸다.
            if(Physics.Raycast(this.transform.position, mouse_direction, out hit)) 
            {
                ChangeText(hit.transform.name);
                isClicked = true;
            }
        }

        // 메뉴로 돌아가는 키 할당
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChange.ReturnTitle();
        }

        if(isClicked)
        {
            time += Time.deltaTime;
        }

        if(time >= 3.0f)
        {
            isClicked = false;
            time = 0;
            FoodScore.text = "";
        }
    }

    private void ChangeText(string name)
    {
        // 각 음식에 맞는 점수로 Text 변경
        for(int i = 0; i < FoodInformation.foods.Length - 1; i++)
        {
            if(FoodInformation.foods[i].getName() == name)
            {
                FoodScore.text = FoodInformation.foods[i].getName() + ": " 
                                    + 
                                 FoodInformation.foods[i].getScore().ToString() + " 점";
            }
        }
    }
}
