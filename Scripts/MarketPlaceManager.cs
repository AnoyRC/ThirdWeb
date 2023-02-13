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
using System.Net;
using System.Diagnostics.Contracts;
using System;

public class MarketPlaceManager : MonoBehaviour
{
    bool connected = false;
    //public GameObject ConnectWindow;
    public TextMeshProUGUI addressTxt;
    string addressShort = "0x000.....00000";
    public TextMeshProUGUI tokenBalance;
    string balance = "0";
    public GameObject CrateOpenDialog;
    public GameObject ClaimDialog;
    string[,] Balance = new string[,]
    {
        {"0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0"
        },
        {
        "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0"
        }
    };
    string address;
    public Sprite[] CardSkins;
    string[] selected = new string[2];
    Dictionary<string,string> indexer = new Dictionary<string,string>()
    {
        {"0", "1_C"},
        {"1", "2_C"},
        {"2", "3_C"},
        {"3", "4_C"},
        {"4", "5_C"},
        {"5", "6_C"},
        {"6", "7_C"},
        {"7", "8_C"},
        {"8", "9_C"},
        {"9", "10_C"},
        {"10", "11_C"},
        {"11", "12_C"},
        {"12", "13_C"},
        {"13", "1_S"},
        {"14", "2_S"},
        {"15", "3_S"},
        {"16", "4_S"},
        {"17", "5_S"},
        {"18", "6_S"},
        {"19", "7_S"},
        {"20", "8_S"},
        {"21", "9_S"},
        {"22", "10_S"},
        {"23", "11_S"},
        {"24", "12_S"},
        {"25", "13_S"},
        {"26", "1_H"},
        {"27", "2_H"},
        {"28", "3_H"},
        {"29", "4_H"},
        {"30", "5_H"},
        {"31", "6_H"},
        {"32", "7_H"},
        {"33", "8_H"},
        {"34", "9_H"},
        {"35", "10_H"},
        {"36", "11_H"},
        {"37", "12_H"},
        {"38", "13_H"},
        {"39", "1_D"},
        {"40", "2_D"},
        {"41", "3_D"},
        {"42", "4_D"},
        {"43", "5_D"},
        {"44", "6_D"},
        {"45", "7_D"},
        {"46", "8_D"},
        {"47", "9_D"},
        {"48", "10_D"},
        {"49", "11_D"},
        {"50", "12_D"},
        {"51", "13_D"},
    };
    public Image MainClaim;
    bool fetched = false;
    public GameObject ConnectWindow;
    // Start is called before the first frame update
    void Start()
    {
        CheckConnected();
    }

    async Task getBalance()
    {
        for (int i = 0; i < Balance.Length; i++)
        {
            if (i == 0)
            {
                var contractSK = ThirdWebManager.Instance.SDK.GetContract("0xb48fAf5A2B3Ef7a28919AB45e93e9Da1F90dA598");
                for (int j = 0; j < 52; j++)
                {
                    Balance[i,j] = await contractSK.ERC1155.BalanceOf(address, j.ToString());
                }
            }

            if (i == 1)
            {
                var contractLM = ThirdWebManager.Instance.SDK.GetContract("0xA958176D10b9F3d157b8afed0e019bba74D5F9bA");
                for (int j = 0; j < 52; j++)
                {
                    Balance[i, j] = await contractLM.ERC1155.BalanceOf(address, j.ToString());
                }
            }

        }
        fetched = true;
    }

    async Task CheckBalance()
    {
        CrateOpenDialog.GetComponentInChildren<TextMeshProUGUI>().text = "Fetching Reward...";
        for (int i = 0; i < Balance.Length; i++)
        {
            if (i == 0)
            {
                var contractSK = ThirdWebManager.Instance.SDK.GetContract("0xb48fAf5A2B3Ef7a28919AB45e93e9Da1F90dA598");
                for (int j = 0; j < 52; j++)
                {
                    string CurBalance = await contractSK.ERC1155.BalanceOf(address, j.ToString());
                    if (Balance[i, j] != CurBalance)
                    {
                        Balance[i, j] = CurBalance;
                        selected[1] = j.ToString();
                        selected[2] = "SK";
                        break;
                    }
                }
            }

            if (i == 1)
            {
                var contractLM = ThirdWebManager.Instance.SDK.GetContract("0xA958176D10b9F3d157b8afed0e019bba74D5F9bA");
                for (int j = 0; j < 52; j++)
                {
                    string CurBalance = await contractLM.ERC1155.BalanceOf(address, j.ToString());
                    if (Balance[i, j] != CurBalance)
                    {
                        Balance[i, j] = CurBalance;
                        selected[1] = j.ToString();
                        selected[2] = "LM";
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < CardSkins.Length; i++)
        {
            if(CardSkins[i].name.Contains(indexer[selected[1]]) && CardSkins[i].name.Contains(selected[2]))
            {
                MainClaim.sprite = CardSkins[i];
                break;
            }
        }
        ClaimDialog.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        addressTxt.text = addressShort;
        tokenBalance.text = balance.Length > 5 ? balance[..5] : balance;
        if (fetched)
        {
            ConnectWindow.GetComponent<Image>().color = Color.Lerp(ConnectWindow.GetComponent<Image>().color, new Color(0, 0, 0, 0f), 0.02f);
            ConnectWindow.GetComponentInChildren<TextMeshProUGUI>().color = Color.Lerp(ConnectWindow.GetComponentInChildren<TextMeshProUGUI>().color, new Color(1, 1, 1, 0f), 0.02f);
            StartCoroutine(RemoveConnect());
        }
        UpdateCp();
    }

    async void UpdateCp()
    {
        var x = await ThirdWebManager.Instance.SDK.wallet.GetBalance("0x3c988602f42C894a1f5B08491b03EE6F2C261CAb");
        balance = x.displayValue;
    }
    IEnumerator RemoveConnect()
    {
        yield return new WaitForSeconds(2);
        ConnectWindow.SetActive(false);
    }

    public async void CheckConnected()
    {
        connected = await ThirdWebManager.Instance.SDK.wallet.IsConnected();
        Debug.Log(connected);

        address = await ThirdWebManager.Instance.SDK.wallet.GetAddress();
        addressShort = address[..5] + "....." + address[(address.Length - 5)..];
        Debug.Log(addressShort);
        
        if(connected)
            await getBalance();
    }

    public async void BuyToken(string amount)
    {
        Thirdweb.Contract contract = ThirdWebManager.Instance.SDK.GetContract("0x3c988602f42C894a1f5B08491b03EE6F2C261CAb");
        await contract.ERC20.Claim(amount);

        var x = await ThirdWebManager.Instance.SDK.wallet.GetBalance("0x3c988602f42C894a1f5B08491b03EE6F2C261CAb");
        balance = x.displayValue;
        Debug.Log(x.displayValue);
        CheckConnected();
    }

    Thirdweb.Contract GetPackContract(string listing)
    {
        return listing == "7" ? ThirdWebManager.Instance.SDK.GetContract("0x2dDCb0F088312eEa656783FCbdC7e20F8Fd9cbE5") :
             ThirdWebManager.Instance.SDK.GetContract("0x338656979086953eb44eC4Fb39649A4e06742521");
    }

    async Task<TransactionResult> OpenPack(string listing)
    {
        CrateOpenDialog.GetComponentInChildren<TextMeshProUGUI>().text = "Opening Pack";
        Thirdweb.Contract packContract = GetPackContract(listing);
        TransactionRequest query = new TransactionRequest();
        query.gasLimit = "3000000";
        var result = await packContract.Write("openPack", 0, 1, query);
        Debug.Log(result);
        return result;
    }

    Marketplace GetMarketPlaceContract()
    {
        return ThirdWebManager.Instance.SDK.GetContract("0x0e5b7953097620b4d5B304eDBA8B94D21dA088Ad").marketplace;
    }

    async Task BuyPackFromMarketPlace(string listing)
    {
        CrateOpenDialog.GetComponentInChildren<TextMeshProUGUI>().text = "Purchasing Pack from Marketplace";
        Marketplace marketplace = GetMarketPlaceContract();
        await marketplace.BuyListing(listing, 1);
        CrateOpenDialog.GetComponentInChildren<TextMeshProUGUI>().text = "Purchase Complete";
    }

    async Task BuyAndOpenPack(string listing)
    {
        try
        {
            CrateOpenDialog.SetActive(true);
            await BuyPackFromMarketPlace(listing);
            await OpenPack(listing);
            await CheckBalance();
        }
        catch (System.Exception error)
        {
            CrateOpenDialog.GetComponentInChildren<TextMeshProUGUI>().text = "Error Opening Pack";
            StartCoroutine(remove());
            Debug.Log(error);
        }
    }

    IEnumerator remove()
    {
        yield return new WaitForSeconds(15);
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
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
