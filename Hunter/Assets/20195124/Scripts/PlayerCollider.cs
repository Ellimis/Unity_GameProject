using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    // Fireball에 맞으면 체력 닳기
    // collision 충돌시 Fireball 이펙트 빛 계산 오류로 Trigger로 설정
    private void OnTriggerEnter(Collider other)
    {
        this.GetComponentInParent<PlayerMovement>().TakeDamage(10);
    }
}
