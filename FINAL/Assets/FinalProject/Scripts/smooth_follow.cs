using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 교수님이 주신 smooth_follow 스크립트
// 거리와 높이만 조정했습니다..
public class smooth_follow : MonoBehaviour
{
    public Transform target;
    float distance = 4.0f;
    float height = 2.0f;
    float heightDamping = 2.0f;
    float rotationDamping = 3.0f;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!target) return;

        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;
        float currentRotationAngle = this.transform.eulerAngles.y;
        float currentHeight = this.transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        this.transform.position = target.position;
        this.transform.position -= currentRotation * Vector3.forward * distance;

        Vector3 temp_position = this.transform.position;
        temp_position.y = currentHeight;
        this.transform.position = temp_position;

        this.transform.LookAt(target);
    }
}
