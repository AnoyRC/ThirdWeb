using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinnerWindow : MonoBehaviour
{
    public GameObject Button;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Image>().color = Color.Lerp(gameObject.GetComponent<Image>().color, new Color(0, 0, 0, 1f), 0.01f);
        Button.GetComponent<Image>().color = Color.Lerp(Button.GetComponent<Image>().color, new Color(1, 1, 1, 1f), 0.01f);
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.Lerp(gameObject.GetComponentInChildren<TextMeshProUGUI>().color, new Color(1, 1, 1, 1f), 0.01f);
        Button.GetComponentInChildren<TextMeshProUGUI>().color = Color.Lerp(Button.GetComponentInChildren<TextMeshProUGUI>().color, new Color(1, 1, 1, 1f), 0.01f);

        var dealer = GameObject.FindGameObjectWithTag("Dealer");
        if (dealer == null) return;
        var dealerScript = dealer.GetComponent<DealerScript>();
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = GameObject.ReferenceEquals(dealerScript.winner, dealerScript.Players[0]) ? "You Win!" : "You Lose!";
    }

    public void loadScene()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
