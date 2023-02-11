using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Thirdweb;
using Photon.Pun;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System.Linq.Expressions;

public class MarketPlaceManager : MonoBehaviour
{
    bool connected = false;
    //public GameObject ConnectWindow;
    public TextMeshProUGUI addressTxt;
    string addressShort="0x000.....00000";
    public TextMeshProUGUI tokenBalance;
    string balance="0";
    public GameObject CrateOpenDialog;
    public GameObject ClaimDialog;
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

    Pack GetPackContract(string listing)
    {
        return listing == "7" ? ThirdWebManager.Instance.SDK.GetContract("0x2dDCb0F088312eEa656783FCbdC7e20F8Fd9cbE5").pack :
             ThirdWebManager.Instance.SDK.GetContract("0x338656979086953eb44eC4Fb39649A4e06742521").pack;
    }

    async Task<ERC1155Reward> OpenPack(string listing)
    {
        Pack packContract=GetPackContract(listing);
        var result = await packContract.Open("0","1");
        ClaimDialog.SetActive(true);
        StartCoroutine(remove());
        return result.erc1155Rewards[0];
    }
    
    Marketplace GetMarketPlaceContract()
    {
        return ThirdWebManager.Instance.SDK.GetContract("0x0e5b7953097620b4d5B304eDBA8B94D21dA088Ad").marketplace;
    }

    async Task BuyPackFromMarketPlace(string listing)
    {
        CrateOpenDialog.GetComponent<TextMeshProUGUI>().text = "Purchasing Pack from Marketplace";
        Marketplace marketplace = GetMarketPlaceContract();
        await marketplace.BuyListing(listing, 1);
        CrateOpenDialog.GetComponent<TextMeshProUGUI>().text = "Purchase Complete";
    }

    async Task BuyAndOpenPack(string listing)
    {
        try
        {
            CrateOpenDialog.SetActive(true);
            await BuyPackFromMarketPlace(listing);
            await OpenPack(listing);
        }
        catch (System.Exception error)
        {
            CrateOpenDialog.GetComponent<TextMeshProUGUI>().text = "Error Opening Pack";
            Debug.Log(error);
        }
    }

    IEnumerator remove()
    {
        yield return new WaitForSeconds(10);
        CrateOpenDialog.SetActive(false);
        ClaimDialog.SetActive(false);
        
    }

    public void Claim()
    {
        CrateOpenDialog.SetActive(false);
        ClaimDialog.SetActive(false);
    }
    
    public async void BuyLootbox(string listing)
    {
        await BuyAndOpenPack(listing);
    }

    public void ToMainMenu()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
