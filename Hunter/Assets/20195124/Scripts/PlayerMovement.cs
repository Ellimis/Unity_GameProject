using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Player HP
    public int maxHP = 100;
    public int curHP;
    public HealthBar hpBar;
    // Player UI
    public Text Name;
    public Text Ammo;
    public Text Money;
    // 무기들
    public GameObject[] Weapons;
    // 피격시 UI
    public Image bloodScreen;


    private Store storeMenu;
    private GameObject temp;
    private string[] WeaponName =
    {
        "Pistol",
        "Magnum",
        "M4",
        "AK47"
    };
    private Vector3[] WeaponPos =
    {
        // Pistol
        new Vector3(0.5f, -0.6f, 1.15f),
        // Magnum
        new Vector3(0.4f, -0.65f, 0.6f),
        // M4
        new Vector3(0.55f, -0.9f, 1.15f),
        // AK
        new Vector3(0.55f, -0.9f, 1.15f)
    };

    void Start()
    {
        curHP = maxHP;
        hpBar.SetMaxHealth(maxHP);
        storeMenu = GameObject.Find("Store").GetComponent<Store>();

        // 무기 생성 후 자식으로 등록
        temp = GameObject.Instantiate(Weapons[0], this.transform.position, this.transform.rotation);
        temp.transform.SetParent(this.transform);
        temp.transform.localPosition = WeaponPos[0];
        // 초기 무기는 권총
        PlayerPrefs.SetInt("GunNumber", 0);
    }

    void Update()
    {
        // UI 정보 최신화
        Name.text = WeaponName[PlayerPrefs.GetInt("GunNumber")];
        Ammo.text = temp.GetComponent<Guns>().curAmmo.ToString() + "/" + temp.GetComponent<Guns>().curWeapon.MAX_ammo.ToString();
        Money.text = "잔돈 : " + storeMenu.GetRemainMoney().ToString() + "원";
    }

    // 무기 변경
    public void ChangeWeapon(int WeaponNum)
    {
        // 현재 무기 지우고 변경할 것으로 무기 정보 다시 설정
        Destroy(temp);

        temp = GameObject.Instantiate(Weapons[WeaponNum], this.transform.position, this.transform.rotation);
        temp.transform.SetParent(this.transform);
        temp.transform.localPosition = WeaponPos[WeaponNum];
        PlayerPrefs.SetInt("GunNumber", WeaponNum);
    }

    // 데미지 입었을 때 함수 발동, 죽으면 강 제 종 료 ㅎㅎ (시간이 모자라요 ㅠㅠ)
    public void TakeDamage(int damage)
    {
        curHP -= damage;
        hpBar.SetHealth(curHP);

        if(curHP <= 0)
        {
            curHP = 0;
            hpBar.SetHealth(curHP);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        // 순간적으로 피격 UI 보여주고 사라지게
        StartCoroutine(ShowBloodScreen());
    }

    IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 0, 0, UnityEngine.Random.Range(0.5f, 0.6f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear;
    }
}
