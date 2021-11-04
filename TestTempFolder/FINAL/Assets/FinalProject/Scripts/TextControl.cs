using UnityEngine;
using UnityEngine.UI;

// 스크립트를 연결하면서 AudioSource를 자동으로 연결해주는 문장
// [RequireComponent(typeof(AudioSource))]
// "Game" scene에서 MainCanvas에게 할당중
public class TextControl : MonoBehaviour
{
    public Text TimeText;
    public Text ScoreText;
    public Text TipsText;

    // 게임이 끝나서 Scene이 이동하면서 점수를 넘긴 적이 있다면 true
    // 그 동안 update문이 발생해 다른 Scene에서도 작동하는 것을 방지하는 변수
    private bool isSend = false;

    // 남은 시간
    [SerializeField] private float time = 90.0f;
    // 점수
    [SerializeField] private int score = 0;
    // 팁이 생성될 시간
    private float tipTime = 12.0f;
    // 현재 보여줄 팁의 위치
    private int currentTip = 0;

    private string[] tips = new string[] 
    {
        "Space 바를 통한 점프와 Esc키를 통한 메뉴를 불러올 수 있습니다.",
        "성에서 R키를 누르면 처음 시작한 위치로 돌아갑니다.",
        "고양이는 너무 귀엽습니다.",
        "고양이를 그린 사람은 같이 게임을 즐기는 지인입니다.",
        "맵 어딘가에는 또 다른 맵으로 갈 수 있는 포탈이 있습니다.",
        "던전에서 R키를 통해서 달리기를 토글로 이용할 수 있습니다.",
        "여러 음식들을 먹는 것 보다 숨겨진 맵에서 얻는 점수가 더 클 수 있습니다.",
        "20195124"
    };

    // 던전에 있는 큰 물고기들의 수
    private int BigFishCount = 4;

    void Update()
    {
        if(!isSend)
        {
            time -= Time.deltaTime;
            tipTime -= Time.deltaTime;

            if(time <= 0f)
            {
                time = 0;
                End();
            }

            ScoreText.text = "Score : " + score;
            TimeText.text = string.Format("Time : {0:0.0}", time);

            if(tipTime <= 0)
            {
                if(currentTip >= tips.Length)
                {
                    currentTip = 0;
                }

                TipsText.text = tips[currentTip % tips.Length];
                tipTime = 12.0f;
                currentTip++;
            }
        }

        // 큰 물고기들을 모두 먹으면 점수 화면으로 이동
        if(BigFishCount == 0)
        {
            End();
        }
    }
    
    // 고양이가 음식을 먹었을 경우 호출된다.
    public void ChangeScore(string tag, string name) 
    {
        if(tag == "Food")
        {
            this.GetComponent<AudioSource>().Play();
            
            // contains로 하다보니 Fish가 겹쳐서 100점만 주는 것을 방지하기 위해 따로 뺏습니다.
            if(name == "BigFish")
            {
                this.score += FoodInformation.foods[6].getScore();
                BigFishCount--;
                return;
            }

            for(int i = 0; i < FoodInformation.foods.Length - 1; i++) 
            {
                if(name.Contains(FoodInformation.foods[i].getName())) 
                {
                    this.score += FoodInformation.foods[i].getScore();
                    break;
                }
            }
        }
    }

    // 시간 조정을 위한 메소드
    public void plusTime(float value)
    {
        time += value;
    }

    // 점수 화면에서 현재 얻은 총 점수를 얻기 위한 메소드
    public int GetScore()
    {
        return score;
    }

    // 시간이 다 지나면 종료
    public void End()
    {
        // Scene을 이동하면서 오브젝트가 사라지는 데 이때 MainCanvas(this)는 점수의 저장을 위해 놔두는 함수
        DontDestroyOnLoad(this);
        isSend = true;
        SceneChange.EndGame();
    }
}
