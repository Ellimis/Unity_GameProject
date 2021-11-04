using UnityEngine;

// "Game" scene에서 Plane에게 할당중
public class FoodGenerator : MonoBehaviour
{
    public GameObject[] foods;
    private TextControl textControl;
    private int maxCount = 20;
    // 인스펙터 창에서 보이고 수정 가능하지만 다른 스크립트에서는 접근 불가능
    [SerializeField] private int remain = 0;

    void Start()
    {
        // 다시 시작했을 때를 위한 사용했던 위치들과 남은 갯수를 초기화
        remain = maxCount;
        FoodInformation.resetPos();
        textControl = GameObject.Find("MainCanvas").GetComponent<TextControl>();

        int randomStart = Random.Range(0, 25);

        for(int i = 0; i < foods.Length; i++)
        {
            // 설정된 최대 개수가 6개일 때 음식도 6개인데 이때 확률상 안나오는 음식들이 생기므로
            // 적어도 한개의 음식을 배치할 수 있도록 지정
            int atLeast = (int)(maxCount * FoodInformation.ratio[i]);

            if(atLeast == 0)
            {
                atLeast = 1;
            }

            for(int j = 0; j < atLeast; j++)
            {
                // 사용되지 않은 위치 정보에 음식 생성
                if(FoodInformation.pos[randomStart].getIsUsed() == false) 
                {
                    // 음식의 정보를 가지고 있는 스크립트로부터 정보를 받아와 생성
                    Instantiate(foods[i], 
                                new Vector3(FoodInformation.pos[randomStart].getX(), 
                                            FoodInformation.startY[i], 
                                            FoodInformation.pos[randomStart].getZ()), 
                                this.transform.rotation);
                    FoodInformation.pos[randomStart].setIsUsed(true);
                    randomStart++;
                }

                // 설정해둔 총 위치의 갯수인 25개보다 높아지면 0번째 인덱스부터 이어지게 하는 조건문
                if(randomStart >= 25)
                {
                    randomStart = 0;
                }
            }
        }
    }

    void Update()
    {
        // 목표의 남은 갯수가 0이 되면 게임을 종료시킨다
        if(remain == 0)
        {
            textControl.End();
        }
    }

    // 음식을 먹으면 남은 갯수를 줄이기 위해 호출되는 메소드
    public void DecreaseRemain()
    {
        remain -= 1;
    }
}
