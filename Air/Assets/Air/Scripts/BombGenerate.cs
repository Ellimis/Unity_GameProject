using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGenerate : MonoBehaviour
{
    public GameObject BombPrefab;
    public Transform SpawnPoint;
    public GameObject player;
    public float[] ZRange;

    // 사정 거리 내에 들어오면 사격
    private bool isInRange = false;

    void Start()
    {
        StartCoroutine(SpawnBomb());
    }

    IEnumerator SpawnBomb()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            
            if(ZRange[0] <= player.transform.position.z && player.transform.position.z <= ZRange[1])
            {
                GameObject bomb = GameObject.Instantiate(BombPrefab, SpawnPoint.position, SpawnPoint.rotation) as GameObject;
                GameObject.Destroy(bomb, 3f);
            }
        }
    }
}
