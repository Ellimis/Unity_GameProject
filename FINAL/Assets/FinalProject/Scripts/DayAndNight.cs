using UnityEngine;

// 낮과 밤을 구별할 수 있게 해주는 스크립트
// window->Rendering->lightning->fog 사용
// "Game" scene에서 Directional Light에게 할당중
public class DayAndNight : MonoBehaviour
{
    // 현실대비 시간이 게임 속에서 얼마만큼 지날지
    [SerializeField] private float secondPerRealTimeSecond = 10;
    // 안개 증감량 비율
    [SerializeField] private float fogDensityCalc = 0.1f;
    // 밤일 때 최대 안개
    [SerializeField] private float nightFogDensity = 0.075f;
    // 낮일 때의 최소 안개를 받아온다.(0.02)
    private float dayFogDensity;
    // 낮인지 밤인지 구별
    private bool isNight = false;
    // 현재 안개의 밀집도에 따라 낮과 밤일 때의 안개 설정
    private float currentFogDensity;

    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    void Update()
    {
        // 매시간마다 햇빛의 역할을 하는 Directional Light를 회전시켜 낮과 밤을 구현
        this.transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        // 각도에 따라 낮과 밤을 지정
        if(transform.eulerAngles.x >= 170) 
        {
            isNight = true;
        }
        else if(transform.eulerAngles.x <= 10) 
        {
            isNight = false;
        }

        if(isNight) // 밤일 때
        {
            if(currentFogDensity <= nightFogDensity) 
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
            
        }
        else // 낮일 때
        {
            if(currentFogDensity >= dayFogDensity) 
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }

}