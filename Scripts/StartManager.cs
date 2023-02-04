using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Thirdweb;
using TMPro;

public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject connected;
    public TextMeshProUGUI addressTxt;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void ConnectWallet()
    {
        string address = await ThirdWebManager.Instance.SDK.wallet.Connect(new WalletConnection()
        {
           provider= WalletProvider.MetaMask,
           chainId=80001
        });
        string addressShort=address[..5] + "....." + address[(address.Length-5)..];
        Debug.Log(addressShort);
        addressTxt.text=addressShort;
    }
}
