using UnityEngine;

// Prefabs 폴더에서 각 음식 프리팹들에게 할당중
public class FoodControl : MonoBehaviour
{    
    private TextControl textControl;
    private FoodGenerator generator = null;

    void Start()
    {
        textControl = GameObject.Find("MainCanvas").GetComponent<TextControl>();
        
        if(this.transform.name != "BigFish")
        {
            generator = GameObject.Find("Plane").GetComponent<FoodGenerator>();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        textControl.ChangeScore(this.transform.tag, this.transform.name);
        if(generator != null)
        {
            generator.DecreaseRemain();
        }
        Destroy(this.gameObject, 0.1f);
    }
}
