using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class PlayerScript : MonoBehaviour
{
    public GameObject Dealer;
    
    DealerScript script;
    public Button hit;
    public Button stand;
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
