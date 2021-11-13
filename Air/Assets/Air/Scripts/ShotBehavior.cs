using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour
{
    public GameObject explosion;
    public AudioClip explosionSound;

    // 한번이라도 부딪혔으면 끝
    private bool isCrashed = false;
    private float power = 750.0f;

    void Update ()
    {
		this.transform.position += transform.forward * Time.deltaTime * power;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "boss")
        {
            StartCoroutine(Attack());
            GameObject.Find("Boss").GetComponent<BossManager>().decreaseHP(0.001f);
        }
    }

    IEnumerator Attack()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(explosionSound);
        explosion.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
