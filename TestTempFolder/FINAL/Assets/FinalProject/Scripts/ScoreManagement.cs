using UnityEngine;
using UnityEngine.UI;

// "End" scene에서 Canvas에게 할당중
public class ScoreManagement : MonoBehaviour
{
    // 한 게임당 점수를 가지는 TotalScore
    public Text TotalScore;
    // 모든 게임의 점수를 기록하는 Rank
    public Text[] Rank;
    // 한 게임의 점수를 받아올 변수
    private int score = 0;

    void Start()
    {
        // 매판마다의 점수 데이터를 가지는 오브젝트를 찾아 점수를 저장한뒤 제거해준다.
        GameObject canvas = GameObject.Find("MainCanvas");
        score = canvas.GetComponent<TextControl>().GetScore();
        // 제거하지 않으면 매 판마다 MainCanvas가 늘어난다..
        Destroy(canvas);

        // 저장한 점수를 보여주고 랭킹에도 반영한다.
        TotalScore.text = score.ToString();
        SaveData.ChangeRanking(score);

        // 랭킹을 보여준다.
        for(int i = 0; i < Rank.Length; i++)
        {
            Rank[i].text = ((i+1) + ": " + SaveData.Score[i]).ToString();
        }
    }

    void Update()
    {
        // 타이틀로 가기 위한 키 할당
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChange.ReturnTitle();
        }
    }
}
