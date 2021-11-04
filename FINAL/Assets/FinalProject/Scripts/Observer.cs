using UnityEngine;

// "Dungeon" scene에서 Monster->PointOfVeiw에게 할당중
public class Observer : MonoBehaviour
{
    // 플레이어의 위치를 쉽게 알게해주는 Transform 타입
    public Transform player;
    // 플레이어가 범위 내에 있는지 확인
    private bool IsPlayerInRange;
    // 플레이어가 시야에 들어와서 얼마의 시간이 지났는지 체크
    [SerializeField] private float timer = 0;

    // 플레이어가 시야에 들어왔을 때
    private void OnTriggerEnter(Collider other) 
    {
        if(other.transform == player)
        {
            Debug.Log("시야에 보임");
            IsPlayerInRange = true;
        }
    }

    // 플레이어가 시야에서 나갔을 때
    private void OnTriggerExit(Collider other) 
    {
        if(other.transform == player)
        {
            Debug.Log("시야에 보이지 않음");
            IsPlayerInRange = false;
            timer = 0;
        }
    }

    void Update()
    {
        // 시야 내에 있을 때 
        if(IsPlayerInRange)
        {
            timer += Time.deltaTime;

            // 일정 시간이 지나면 플레이어는 초기 위치로 돌아가며 남은 시간이 줄어든다.
            if(timer >= 3.0f)
            {
                player.transform.position = new Vector3(-37.5f, 0, 0);
                player.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                GameObject.Find("MainCanvas").GetComponent<TextControl>().plusTime(-20.0f);
            }
        }
    }
}
