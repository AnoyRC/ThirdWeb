using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOnActive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dealer = GameObject.FindGameObjectWithTag("Dealer");
        if (dealer == null) return;
        var dealerScript = dealer.GetComponent<DealerScript>();
        if(dealerScript.current == 0)
        {
            gameObject.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f);
        }
        else
        {
            gameObject.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0f);
        }
        
    }
}
