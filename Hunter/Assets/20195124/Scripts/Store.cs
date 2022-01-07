using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public GameObject storeUI;
    public bool isOpened;
    public Text chance;
    public Text cost;
    // 강화 여부에 따라 무기 프레임에 몇강인지 표시
    public Text[] enhanceRank;
    public int curEnhanceRank;
    public AudioClip enhanceSuccess;
    public Image[] Frames;

    // 강화 확률 넣어보고 싶어서 넣어본 확률 기능
    // 실패시 변화 X
    private int[] enhanceChance = { 100, 50, 10 };
    private int[] enhanceCost = { 1500, 3000, 5000 };
    // 무기 구매시의 가격
    private int[] weaponPrice =
    {
        // Pistol
        500,
        // Magnum
        2000,
        // M4
        5000,
        // AK47
        9000
    };
    // 남은 돈을 UI에 표시하기 위한 변수
    private int remainMoney;
    // 무기 변경 함수를 위한 변수
    private PlayerMovement PM;
    private AudioSource AS;
    // 무기 클릭시 UI 변경을 위한 변수
    private Color baseColor;
    private float uiReactionTimer;
    private bool uiReactionOn;
    // 무기 강화 성공과 실패 그리고 잔액 부족시 UI출력을 위한 변수
    private Text log;
    private bool isLogOn;
    private float logTimer;

    void Start()
    {
        isOpened = false;
        curEnhanceRank = 0;
        remainMoney = 0;
        PM = GameObject.Find("ARCamera").GetComponent<PlayerMovement>();
        AS = this.gameObject.GetComponent<AudioSource>();
        baseColor = Color.white;
        baseColor.a = 0.5f;
        uiReactionTimer = 0;
        uiReactionOn = false;
        isLogOn = false;
        logTimer = 0;
    }

    void Update()
    {
        // 강화 가능한지 강화 버튼 텍스트 변화
        if(curEnhanceRank < enhanceChance.Length)
        {
            chance.text = "확률: " + enhanceChance[curEnhanceRank].ToString() + "%";
            cost.text = "가격: " + enhanceCost[curEnhanceRank].ToString() + "원";
        }
        else
        {
            chance.text = "최종 강화";
            cost.text = "";
        }

        // 모든 무기에 강화 정도 표시, 모든 무기 강화 정도 공유
        foreach(Text temp in enhanceRank)
        {
            temp.text = "+" + curEnhanceRank.ToString();
        }

        // 무기 구매시 UI 반응
        if(uiReactionOn)
        {
            uiReactionTimer += Time.deltaTime;

            if(uiReactionTimer >= 0.25f)
            {
                Frames[PlayerPrefs.GetInt("GunNumber")].transform.GetComponent<Image>().color = baseColor;
                uiReactionTimer = 0;
                uiReactionOn = false;
            }
        }

        // 무기 강화 성공시의 로그
        if(isLogOn)
        {
            logTimer += Time.deltaTime;

            // 2초만 로그 보이고 안보이게
            if(logTimer >= 2.0f)
            {
                log.text = "";
                logTimer = 0;
                isLogOn = false;
            }
        }

        // PC 돈 확인용 함수
        if (Input.GetKeyDown(KeyCode.U))
        {
            this.remainMoney = 10000;
        }
    }

    // 상점 열기
    public void StoreOpen()
    {
        isOpened = true;
        // 열렸을 때 UI 뒤에 있는 상자가 맞는 것을 방지
        this.GetComponent<BoxCollider>().enabled = false;
        storeUI.SetActive(true);
    }

    // 상점 닫기
    public void StoreExit()
    {
        isOpened = false;
        this.GetComponent<BoxCollider>().enabled = true;
        storeUI.SetActive(false);
    }

    // 무기 구매
    public void PurchaseWeapon(int weaponNum)
    {
        // 현재 들고 있는 무기가 아닐 경우에만 구매 가능, 기존 무기 재구매시 돈이 든다.
        if (PlayerPrefs.GetInt("GunNumber") != weaponNum)
        {
            if((remainMoney - weaponPrice[weaponNum]) >= 0)
            {
                DecreaseMoney(weaponPrice[weaponNum]);
                PM.ChangeWeapon(weaponNum);
            }
        }
    }

    // 현재 남은 잔돈을 반환
    public int GetRemainMoney()
    {
        return remainMoney;
    }

    // 현재 남은 잔돈 감소
    public void DecreaseMoney(int pay)
    {
        this.remainMoney -= pay;
    }

    // 현재 남은 잔돈 증가
    public void IncreaseMoney(int plus)
    {
        this.remainMoney += plus;
    }

    // 강화 기능
    public void EnhanceWeapon()
    {
        log = GameObject.Find("Log").GetComponent<Text>();

        // 강화 가능한 랭크일때
        if (curEnhanceRank < enhanceChance.Length)   
        {
            // 강화에 필요한 돈을 빼고도 잔돈이 남으면
            if ((remainMoney - enhanceCost[curEnhanceRank]) >= 0)
            {
                int rand = Random.Range(0, 100) + 1;

                // 강화 성공 or 실패, 강화 성공 시 소리 발생
                if (rand <= enhanceChance[curEnhanceRank])
                {
                    log.text = "강화 성공";
                    DecreaseMoney(enhanceCost[curEnhanceRank]);
                    curEnhanceRank += 1;
                    AS.clip = enhanceSuccess;
                    AS.Play();
                }
                else
                {
                    log.text = "강화 실패";
                    DecreaseMoney(enhanceCost[curEnhanceRank]);
                }
            }
            else log.text = "잔액 부족";

            isLogOn = true;
        }
    }
    
    // 구매를 누른 무기에 맞는 UI 반응
    public void reactionFrame(int weaponNum)
    {
        Color color;

        // 불투명하게 했다가 다시 0.5만큼 투명하게 설정하기 위한 함수
        color = Frames[weaponNum].transform.GetComponent<Image>().color;
        color.a = 1.0f;
        Frames[weaponNum].transform.GetComponent<Image>().color = color;
        uiReactionOn = true;
    }
}
