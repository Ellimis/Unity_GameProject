using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public Image CursorGaugeImage;
    private Vector3 ScreenCenter;
    private float GaugeTimer;

    void Start()
    {
        ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);    
    }
    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenter);
        RaycastHit hit;
        CursorGaugeImage.fillAmount = GaugeTimer;

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            GaugeTimer += 1.0f / 2.0f * Time.deltaTime;

            if (GaugeTimer >= 1.0f)
            {
                Debug.Log(hit.transform.name);
                hit.transform.GetComponent<Button>().onClick.Invoke();
                GaugeTimer = 0.0f;
            }
        }
        else GaugeTimer = 0.0f;
    }
}
