using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingWindow : MonoBehaviour
{
    GameObject Dealer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Dealer = GameObject.FindGameObjectWithTag("Dealer");
        if (Dealer == null) return;
        var dealerScript = Dealer.GetComponent<DealerScript>();
        if (dealerScript.Players.Length == 2)
        {
            gameObject.GetComponent<Image>().color -= new Color(0, 0, 0, 0.0025f);
            gameObject.GetComponentInChildren<TextMeshProUGUI>().color -= new Color(1, 1, 1, 0.0025f);
            StartCoroutine(Remove());
        }
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }


}
