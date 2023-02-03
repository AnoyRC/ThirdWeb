using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggler : MonoBehaviour
{
    public GameObject[] Buttons;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPressIndex(GameObject k){
        if(k.GetComponent<Toggle>().isOn == true){
            foreach( GameObject i in Buttons ){
            if(!GameObject.ReferenceEquals(i,k))
                i.GetComponent<Toggle>().isOn = false;
            }
        }
    }
}
