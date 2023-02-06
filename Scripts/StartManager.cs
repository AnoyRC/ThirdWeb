using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Thirdweb;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button connectButton;
    public TextMeshProUGUI connectButtonText;
    public TextMeshProUGUI addressTxt;
    string addressShort = "0x000.....00000";

    void Start()
    {
        connectButtonText.text = "Connect";
        connectButton.onClick.AddListener(ConnectWallet);
    }

    // Update is called once per frame
    void Update()
    {
        addressTxt.text = addressShort;
        if (connectButtonText.text != "Play")
        {
            if (addressShort != "0x000.....00000")
            {
                connectButtonText.text = "Play";
                connectButton.onClick.RemoveAllListeners();
                connectButton.onClick.AddListener(LoadScene);
            }
        }
    }

    public async void ConnectWallet()
    {
        string address = await ThirdWebManager.Instance.SDK.wallet.Connect(new WalletConnection()
        {
           provider= WalletProvider.MetaMask,
           chainId=80001
        });
        addressShort=address[..5] + "....." + address[(address.Length-5)..];
        Debug.Log(addressShort);
    }
    
    public void LoadScene()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
