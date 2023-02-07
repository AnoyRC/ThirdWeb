using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    bool connected = false;
    public GameObject ConnectWindow;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (connected)
        {
            ConnectWindow.GetComponent<Image>().color -= new Color(0, 0, 0, 0.005f);
            ConnectWindow.GetComponentInChildren<TextMeshProUGUI>().color -= new Color(1, 1, 1, 0.005f);
            StartCoroutine(Remove());
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to Master Server");
        connected = true;
        CheckConnected();

    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(2);
        ConnectWindow.SetActive(false);
    }

    public async void CheckConnected()
    {
        bool isConnected=await ThirdWebManager.Instance.SDK.wallet.IsConnected();
        Debug.Log(isConnected);
    }
}
