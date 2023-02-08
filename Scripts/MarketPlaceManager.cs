using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Thirdweb;
using Photon.Pun;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MarketPlaceManager : MonoBehaviour
{
    bool connected = false;
    //public GameObject ConnectWindow;
    public TextMeshProUGUI addressTxt;
    string addressShort="0x000.....00000";
    public TextMeshProUGUI tokenBalance;
    string balance="0";
    public Button Buy;
    public Button Buy100;
    public Button Buy200;
    public Button Buy500;
    public Button Buy1000;
    // Start is called before the first frame update
    void Start()
    {
        CheckConnected();
//
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

    public async void BuyToken(string amount)
    {
        Contract contract=ThirdWebManager.Instance.SDK.GetContract("0x3c988602f42C894a1f5B08491b03EE6F2C261CAb");
        await contract.ERC20.Claim(amount);

        var x=await ThirdWebManager.Instance.SDK.wallet.GetBalance("0x3c988602f42C894a1f5B08491b03EE6F2C261CAb");
        balance=x.displayValue;
        Debug.Log(x.displayValue);
    }

    async Task<Pack> GetPackContract()
    {
        return  ThirdWebManager.Instance.SDK.GetContract("0x338656979086953eb44eC4Fb39649A4e06742521").pack;
    }

    async Task<ERC1155Reward> OpenPack()
    {
        Pack packContract=await GetPackContract();

        var result=await packContract.Open("0","1");

        return result.erc1155Rewards[0];
    }
}
