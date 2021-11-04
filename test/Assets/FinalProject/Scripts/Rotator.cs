using UnityEngine;

// "HowTo" scene에서 Objects에게 할당중
public class Rotator : MonoBehaviour 
{
    // 음식들의 정보를 가지게 될 변수
    [SerializeField] private Transform[] foods;
    // 음식이 회전하는 속도
    [SerializeField] private float rotateSpeed = 50f;

	void Start () 
    {
        // 현재 연결된 객체의 자식들(음식들)을 전부 배열에 할당
        foods = new Transform[this.transform.childCount];

        for (int i = 0; i < foods.Length; i++)
        {
            foods[i] = this.transform.GetChild(i);
        }
	}
	
	void Update () 
    {
        // 자식의 수만큼 모든 자식들을 회전 시킨다
        for (int i = 0; i < foods.Length; i++)
        {
            foods[i].Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.World);
        }
	}
}
