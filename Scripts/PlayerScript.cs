using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public GameObject Dealer;
    
    DealerScript script;
    public Button hit;
    public Button stand;
    public GameObject your;
    public GameObject turn;
    PhotonView PV;
    GameObject[] Players;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        hit.onClick.AddListener(Hit);
        stand.onClick.AddListener(Stand);
    }

    // Update is called once per frame
    void Update()
    {
        Players = GameObject.FindGameObjectsWithTag("Players");
        Dealer = GameObject.FindGameObjectWithTag("Dealer");
        if (Dealer)
        {
            script = Dealer.GetComponent<DealerScript>();
            if (GameObject.ReferenceEquals(script.Players[1], gameObject))
            {
                your.SetActive(true);
                turn.SetActive(true);
                if (script.current == 1)
                {
                    your.GetComponentInChildren<TextMeshProUGUI>().text = "Wait";
                    turn.GetComponentInChildren<TextMeshProUGUI>().text = "Wait";
                }
                else
                {
                    your.GetComponentInChildren<TextMeshProUGUI>().text = "your";
                    turn.GetComponentInChildren<TextMeshProUGUI>().text = "turn";
                }
            }
        }
    }

    public void Stand()
    {
        if (GameObject.ReferenceEquals(script.Players[script.current], gameObject))
        {
            if (!GameObject.ReferenceEquals(Players[1], gameObject))
            {
                PV.RPC("RPC_Stand", RpcTarget.AllBuffered);
            }
        }

    }

    public void Hit()
    {
        if (GameObject.ReferenceEquals(script.Players[script.current], gameObject))
        {
            if (!GameObject.ReferenceEquals(Players[1], gameObject))
            {
                PV.RPC("RPC_Hit", RpcTarget.AllBuffered);
            }
        }

    }
    
    [PunRPC]
    public void RPC_Hit()
    {
      
         
        script.Hit();
        Debug.Log("Hit");
         
    }

    [PunRPC]
    public void RPC_Stand()
    {
        
        
        script.Stand();
        Debug.Log("Stand");
        
        
    }
}
