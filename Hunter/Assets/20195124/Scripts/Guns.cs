using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 변경시 어떤 무기로 바꿀지를 위한 열거형
public enum WEAPONCODE
{ PISTOL, MAGNUM, M4, AK }

// 총의 데미지나 dps, 딜레이 등을 기록하기 위한 구조체
public struct Weapon
{
    public int MAX_ammo;
    public int damage;
    public int NoSps; // the Number of Shots per Sec, 초당 발사수
    public float delay;
    public int dps;

    public Weapon(int MAX_ammo, int damage, int NoSps, float delay)
    {
        this.MAX_ammo = MAX_ammo;
        this.damage = damage;
        this.NoSps = NoSps;
        this.delay = delay;
        this.dps = this.NoSps * this.damage;
    }
}

public class Guns : MonoBehaviour
{
    public Camera cam;
    // 현재 무기 기록
    public Weapon curWeapon;
    // 현재 나온 총에 따라 설정
    public int curAmmo;
    public GameObject muzzleFlash;
    public AudioClip fire;
    public AudioClip reload;
    // 구매와 강화를 위한 메뉴
    public Store storeMenu;

    private AudioSource AS;
    private Boss boss;
    private float fireDelay;
    private float timer = 0f;
    private bool canFire = true;
    private bool isFired = false;
    private float reloadTimer = 0;

    void Start()
    {
        cam = GameObject.Find("ARCamera").GetComponent<Camera>();
        storeMenu = GameObject.Find("Store").GetComponent<Store>();
        // 총의 정보를 Get과 Set을 통해 설정
        SetWeapon(PlayerPrefs.GetInt("GunNumber"));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 발사 가능 여부 확인
            if(canFire)
            {
                isFired = true;
                canFire = false;

                // 상점에선 총알 안닳게
                if(storeMenu.isOpened) curAmmo = curWeapon.MAX_ammo;
                else curAmmo -= 1;
                
                // 쏘는 순간 보이고 사라지기 위한 변수
                muzzleFlash.transform.localScale = new Vector3(3f, 3f, 3f);
                AS.clip = fire;
                AS.Play();
                Shoot();
            }
        }

        // 발사 후 무기 딜레이 만큼 대기
        if (isFired)
        {
            timer += Time.deltaTime;

            if (timer >= fireDelay)
            {
                isFired = false;
                canFire = true;
                timer = 0;
            }
        }

        // 쏘고 나면 크기를 0으로 해서 없어진 것처럼 보이게
        if (Input.GetMouseButtonUp(0))
        {
            muzzleFlash.transform.localScale = new Vector3(0f, 0f, 0f);
        }

        // 현재 남은 총알이 적어지면 장전
        if(curAmmo <= 0)
        {
            AS.clip = reload;
            AS.Play();
            canFire = false;
            reloadTimer += Time.deltaTime;
        }

        if(reloadTimer >= 1.5f)
        {
            curAmmo = curWeapon.MAX_ammo;
            reloadTimer = 0;
            canFire = true;
        }
    }

    void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 보스 때리기
            if (hit.transform.name == "Rikayon")
            {
                boss = hit.transform.GetComponent<Boss>();

                if (boss != null)
                {
                    boss.TakeDamage(curWeapon.damage + (int)(curWeapon.damage*0.1) * (storeMenu.curEnhanceRank * storeMenu.curEnhanceRank));
                }
            }

            /////////////////
            /// 상점 메뉴 ///
            /////////////////
            // 상점 열기
            if(hit.transform.tag == "Store") storeMenu.StoreOpen();
            // 상점 닫기
            if (hit.transform.tag == "Close") storeMenu.StoreExit();
            // 상점에서 권총 이미지 맞출시
            if (hit.transform.tag == "Pistol")
            {
                storeMenu.PurchaseWeapon(0);
                storeMenu.reactionFrame(0);
            }
            // 상점에서 매그넘 이미지 맞출시
            if (hit.transform.tag == "Magnum")
            {
                storeMenu.PurchaseWeapon(1);
                storeMenu.reactionFrame(1);
            }
            // 상점에서 M4 이미지 맞출시
            if (hit.transform.tag == "M4")
            {
                storeMenu.PurchaseWeapon(2);
                storeMenu.reactionFrame(2);
            }
            // 상점에서 AK 이미지 맞출시
            if (hit.transform.tag == "AK47")
            {
                storeMenu.PurchaseWeapon(3);
                storeMenu.reactionFrame(3);
            }
            // 강화 버튼 클릭시
            if (hit.transform.tag == "Enhance") storeMenu.EnhanceWeapon();
        }
    }

    // 무기 변경 함수
    public void SetWeapon(int WeaponCode)
    {
        // 입력받은 코드 0~3에 따라 무기 설정
        switch (WeaponCode)
        {
            case (int)WEAPONCODE.PISTOL:
                curWeapon = new Weapon(15, 30, 8, 0.125f);
                break;

            case (int)WEAPONCODE.MAGNUM:
                curWeapon = new Weapon(6, 90, 4, 0.25f);
                break;

            case (int)WEAPONCODE.M4:
                curWeapon = new Weapon(30, 50, 11, 0.09f);
                break;

            case (int)WEAPONCODE.AK:
                curWeapon = new Weapon(25, 85, 10, 0.1f);
                break;
        }

        // 현재 무기 정보에 맞게 설정
        curAmmo = curWeapon.MAX_ammo;
        AS = this.transform.GetComponent<AudioSource>();
        fireDelay = curWeapon.delay;
    }
}
