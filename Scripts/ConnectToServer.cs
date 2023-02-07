using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine.UI;
using Thirdweb;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    bool connected = false;
    public GameObject ConnectWindow;
    public TextMeshProUGUI addressTxt;
    string addressShort="0x000.....00000";
    public TextMeshProUGUI tokenBalance;
    string balance="0";
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void  Update()
    {
        addressTxt.text=addressShort;
        tokenBalance.text=balance;
        if (connected && addressTxt.text!="0x000.....00000")
        {
            ConnectWindow.GetComponent<Image>().color = Color.Lerp(ConnectWindow.GetComponent<Image>().color, new Color(0, 0, 0, 0f), 0.02f);
            ConnectWindow.GetComponentInChildren<TextMeshProUGUI>().color = Color.Lerp(ConnectWindow.GetComponentInChildren<TextMeshProUGUI>().color, new Color(1, 1, 1, 0f), 0.02f);
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

        string address=await ThirdWebManager.Instance.SDK.wallet.GetAddress();
        addressShort=address[..5] + "....." + address[(address.Length-5)..];
        Debug.Log(addressShort);
        var x=await ThirdWebManager.Instance.SDK.wallet.GetBalance("0x3c988602f42C894a1f5B08491b03EE6F2C261CAb");
        balance=x.displayValue;
        Debug.Log(x.displayValue);


    }

    public async void BuyToken()
    {
        Contract contract=ThirdWebManager.Instance.SDK.GetContract("0x3c988602f42C894a1f5B08491b03EE6F2C261CAb");
        await contract.ERC20.Claim("10");

        var x=await ThirdWebManager.Instance.SDK.wallet.GetBalance("0x3c988602f42C894a1f5B08491b03EE6F2C261CAb");
        balance=x.displayValue;
        Debug.Log(x.displayValue);
    }
}
