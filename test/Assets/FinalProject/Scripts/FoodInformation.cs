using UnityEngine;

// 따로 할당되지 않고 음식들의 정보를 갖는 스크립트
public class FoodInformation : MonoBehaviour
{
    // 음식의 이름과 각각의 점수를 가지는 구조체
    public struct Food
    {
        private string name;
        private int score;

        public Food(string name, int score) 
        {
            this.name = name;
            this.score = score;
        }
        public string getName() { return name; }
        public int getScore() { return score; }
    }
    
    // 구글링으로 알아낸 유니티에서의 c# 구조체 배열 초기화 방법, 생성자 필수
    public static Food[] foods = new Food[] 
    {
        new Food("Bread", 10),
        new Food("Cheese", 20),
        new Food("Pie", 40),
        new Food("Ribs", 60),
        new Food("Ham", 80),
        new Food("Fish", 100),
        new Food("BigFish", 250)
    };

    // 음식의 X 좌표와 Z 좌표 그리고 사용됬는지에 대한 판단을 가지는 구조체
    public struct FoodPosition 
    {
        private float X;
        private float Z;
        // 이렇게 해두면 나중에 아무 위치에 원하는 대로 생성할 때 유용할 것 같아서 isUsed를 넣었습니다.
        private bool isUsed;

        public FoodPosition(float X, float Z) 
        {
            this.X = X;
            this.Z = Z;
            this.isUsed = false;
        }

        public float getX() { return X; }
        public float getZ() { return Z; }
        public bool getIsUsed() { return isUsed; }
        public void setIsUsed(bool isUsed) { this.isUsed = isUsed; }
    }

    // 총 25개의 위치
    // 특정 목표인 적, 아이템 등등이 랜덤으로 아무곳에서나 나올 수 있게 하기 위한 X, Z 좌표들
    public static FoodPosition[] pos = new FoodPosition[] 
    {
        // 중심 상가
        new FoodPosition(0.75f, -5.0f),
        new FoodPosition(0, 0),
        new FoodPosition(-5.0f, 0.5f),
        new FoodPosition(1.0f, 3.25f),
        new FoodPosition(4.0f, -0.25f),
        new FoodPosition(5.55f, -5.5f), // 수풀

        // 성문 쪽
        new FoodPosition(22.0f, 0f),
        new FoodPosition(-20.0f, -0.5f),
        
        // 성벽 쪽
        new FoodPosition(10.0f, 21.5f),
        new FoodPosition(-18.0f, 18.0f),
        new FoodPosition(3.0f, -22.0f),

        // barrack 밑
        new FoodPosition(16.5f, 15.5f),

        //dairy 안
        new FoodPosition(-4.0f, 17.0f),
        new FoodPosition(0.25f, 15.5f), // 수풀

        // mine 쪽
        new FoodPosition(-11.5f, 14.0f), // 수풀

        // warehouse 쪽
        new FoodPosition(-13.5f, 3.5f),

        // windmill 쪽
        new FoodPosition(-6.5f, -12.0f),

        // bakery 쪽
        new FoodPosition(-14.25f, -13.25f),
        new FoodPosition(-9.75f, -17.5f),

        // smith 쪽
        new FoodPosition(13.25f, -15.0f),

        // 수풀 속
        new FoodPosition(10.75f, 10.5f),
        new FoodPosition(5.0f, 8.5f),
        new FoodPosition(18.5f, -4.5f),
        new FoodPosition(12.5f, 4.5f),
        new FoodPosition(9.6f, -16.5f)
    };

    // 각 프리팹의 생성될 Y좌표와 최대 갯수에서 몇개만큼 나오게 할 것인지를  정하는 배열
    //   bread  Cheese  Pie    Ribs   Ham   Fish
    public static float[] startY = 
        { 0.03f, 0.05f, 0.03f, 0.02f, 0.03f, 0f };

    public static float[] ratio = 
        { 0.25f, 0.25f, 0.2f, 0.15f, 0.1f, 0.05f };

    // 모두 사용안한 것처럼 초기화
    public static void resetPos()
    {
        for(int i = 0; i < pos.Length; i++)
        {
            if(pos[i].getIsUsed())
            {
                pos[i].setIsUsed(false);
            }
        }
    }
    
}
