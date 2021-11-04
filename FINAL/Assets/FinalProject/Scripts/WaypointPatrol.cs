using UnityEngine;
using UnityEngine.AI;

// "Dungeon" scene에서 Monster에게 할당중
public class WaypointPatrol : MonoBehaviour
{
    // 길찾기
    public NavMeshAgent navMeshAgent;
    // 이동할 위치
    public Transform[] waypoints;
    // 현재 이동하고 있는 위치
    [SerializeField] private int CurrentWayPointIndex;

    void Start()
    {
        // 초기 이동
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        // 목적지까지 남은 거리가 인스펙터 창에서 설정한 정지 거리보다 짧은지 확인
        if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            // 거의 도착하면 다음 지점으로 이동
            CurrentWayPointIndex = (CurrentWayPointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination (waypoints[CurrentWayPointIndex].position);
        }
    }
}
