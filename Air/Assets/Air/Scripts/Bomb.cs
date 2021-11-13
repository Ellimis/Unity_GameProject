using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosion;
    public AudioClip explosionSound;

    // 한번이라도 부딪혔으면 끝
    private bool isCrashed = false;
    private float power = 500.0f;

    void Start()
    {
        this.transform.LookAt(GameObject.Find("Head").transform.position);
    }

    void Update()
    {
       this.transform.position += transform.forward * Time.deltaTime * power;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isCrashed)
        {
            isCrashed = true;
            GameObject.Find("Head").GetComponent<PlayerMove>().decreaseHP(10);
            StartCoroutine(Crash());
        }
    }

    IEnumerator Crash()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(explosionSound);
        explosion.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
