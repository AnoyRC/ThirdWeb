using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Thirdweb;
using Photon.Pun;

public class MarketPlaceManager : MonoBehaviour
{
    bool connected = false;
    //public GameObject ConnectWindow;
    public TextMeshProUGUI addressTxt;
    string addressShort="0x000.....00000";
    public TextMeshProUGUI tokenBalance;
    string balance="0";
    // Start is called before the first frame update
    void Start()
    {
        CheckConnected();
    }

    // Update is called once per frame
    void Update()
    {
        addressTxt.text=addressShort;
        tokenBalance.text=balance;
    }

    public async void CheckConnected()
    {
        connected=await ThirdWebManager.Instance.SDK.wallet.IsConnected();
        Debug.Log(connected);

        string address=await ThirdWebManager.Instance.SDK.wallet.GetAddress();
        addressShort=address[..5] + "....." + address[(address.Length-5)..];
        Debug.Log(addressShort);
        var x=await ThirdWebManager.Instance.SDK.wallet.GetBalance("0x3c988602f42C894a1f5B08491b03EE6F2C261CAb");
        balance=x.displayValue;
        Debug.Log(x.displayValue);
    }
}
