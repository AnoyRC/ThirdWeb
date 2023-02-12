using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using TMPro;
using Thirdweb;
using Unity.Mathematics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using Photon.Realtime;

public class DealerScript : MonoBehaviourPunCallbacks
{
    
    [SerializeField] bool matchMaking = false;
    public GameObject[] Players;
    public Sprite[] cardSprites;
    public Dictionary<GameObject, bool> turnCaller = new Dictionary<GameObject, bool>();
    public int current = 0;
    public int selectedNum = 0;
    public char selectedSymbol = '\0';

    public List<Tuple<int, char>> resetCards = new List<Tuple<int, char>>()
    {
        new Tuple<int, char>(1,'C'),
        new Tuple<int, char>(2,'C'),
        new Tuple<int, char>(3,'C'),
        new Tuple<int, char>(4,'C'),
        new Tuple<int, char>(5,'C'),
        new Tuple<int, char>(6,'C'),
        new Tuple<int, char>(7,'C'),
        new Tuple<int, char>(8,'C'),
        new Tuple<int, char>(9,'C'),
        new Tuple<int, char>(10,'C'),
        new Tuple<int, char>(11,'C'),
        new Tuple<int, char>(12,'C'),
        new Tuple<int, char>(13,'C'),
        new Tuple<int, char>(1,'S'),
        new Tuple<int, char>(2,'S'),
        new Tuple<int, char>(3,'S'),
        new Tuple<int, char>(4,'S'),
        new Tuple<int, char>(5,'S'),
        new Tuple<int, char>(6,'S'),
        new Tuple<int, char>(7,'S'),
        new Tuple<int, char>(8,'S'),
        new Tuple<int, char>(9,'S'),
        new Tuple<int, char>(10,'S'),
        new Tuple<int, char>(11,'S'),
        new Tuple<int, char>(12,'S'),
        new Tuple<int, char>(13,'S'),
        new Tuple<int, char>(1,'H'),
        new Tuple<int, char>(2,'H'),
        new Tuple<int, char>(3,'H'),
        new Tuple<int, char>(4,'H'),
        new Tuple<int, char>(5,'H'),
        new Tuple<int, char>(6,'H'),
        new Tuple<int, char>(7,'H'),
        new Tuple<int, char>(8,'H'),
        new Tuple<int, char>(9,'H'),
        new Tuple<int, char>(10,'H'),
        new Tuple<int, char>(11,'H'),
        new Tuple<int, char>(12,'H'),
        new Tuple<int, char>(13,'H'),
        new Tuple<int, char>(1,'D'),
        new Tuple<int, char>(2,'D'),
        new Tuple<int, char>(3,'D'),
        new Tuple<int, char>(4,'D'),
        new Tuple<int, char>(5,'D'),
        new Tuple<int, char>(6,'D'),
        new Tuple<int, char>(7,'D'),
        new Tuple<int, char>(8,'D'),
        new Tuple<int, char>(9,'D'),
        new Tuple<int, char>(10,'D'),
        new Tuple<int, char>(11,'D'),
        new Tuple<int, char>(12,'D'),
        new Tuple<int, char>(13,'D'),
    };
    public List<Tuple<int, char>> cards = new List<Tuple<int, char>>()
    {
        new Tuple<int, char>(1,'C'),
        new Tuple<int, char>(2,'C'),
        new Tuple<int, char>(3,'C'),
        new Tuple<int, char>(4,'C'),
        new Tuple<int, char>(5,'C'),
        new Tuple<int, char>(6,'C'),
        new Tuple<int, char>(7,'C'),
        new Tuple<int, char>(8,'C'),
        new Tuple<int, char>(9,'C'),
        new Tuple<int, char>(10,'C'),
        new Tuple<int, char>(11,'C'),
        new Tuple<int, char>(12,'C'),
        new Tuple<int, char>(13,'C'),
        new Tuple<int, char>(1,'S'),
        new Tuple<int, char>(2,'S'),
        new Tuple<int, char>(3,'S'),
        new Tuple<int, char>(4,'S'),
        new Tuple<int, char>(5,'S'),
        new Tuple<int, char>(6,'S'),
        new Tuple<int, char>(7,'S'),
        new Tuple<int, char>(8,'S'),
        new Tuple<int, char>(9,'S'),
        new Tuple<int, char>(10,'S'),
        new Tuple<int, char>(11,'S'),
        new Tuple<int, char>(12,'S'),
        new Tuple<int, char>(13,'S'),
        new Tuple<int, char>(1,'H'),
        new Tuple<int, char>(2,'H'),
        new Tuple<int, char>(3,'H'),
        new Tuple<int, char>(4,'H'),
        new Tuple<int, char>(5,'H'),
        new Tuple<int, char>(6,'H'),
        new Tuple<int, char>(7,'H'),
        new Tuple<int, char>(8,'H'),
        new Tuple<int, char>(9,'H'),
        new Tuple<int, char>(10,'H'),
        new Tuple<int, char>(11,'H'),
        new Tuple<int, char>(12,'H'),
        new Tuple<int, char>(13,'H'),
        new Tuple<int, char>(1,'D'),
        new Tuple<int, char>(2,'D'),
        new Tuple<int, char>(3,'D'),
        new Tuple<int, char>(4,'D'),
        new Tuple<int, char>(5,'D'),
        new Tuple<int, char>(6,'D'),
        new Tuple<int, char>(7,'D'),
        new Tuple<int, char>(8,'D'),
        new Tuple<int, char>(9,'D'),
        new Tuple<int, char>(10,'D'),
        new Tuple<int, char>(11,'D'),
        new Tuple<int, char>(12,'D'),
        new Tuple<int, char>(13,'D'),
    };
    int currentRound = 0;
    public int maxRound = 10;
    bool progress = false;
    public List<int> currentScore = new List<int>();
    public List<int> roundScore = new List<int>();
    public List<GameObject> selected = new List<GameObject>();
    int minScore = 100;
    public int turnCount = 0;
    public GameObject winner;
    int selectedScore = 0;
    int seed;
    PhotonView PV;
    public TextMeshProUGUI[] scorecards;
    public Image[] hands;
    public Image[] handsOpponent;
    private int k = 0, l = 0;
    public Sprite cardBack;
    public GameObject[] Notifiers;
    public GameObject HiddenCover;
    public TextMeshProUGUI TurnNotifier;
    public GameObject WinnerDialog;
    public GameObject[] Timers;
    float timeRemaining = 12;
    Coroutine lastRoutine;
    bool connected = false;
    string address = "";
    public AudioSource music;
    string[] SKbalance = new string[52]{
        "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0"
    };
    string[] LMbalance = new string[52]
    {
        "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0"
    };
    string[] SKbalanceEnemy = new string[52]{
        "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0"
    };
    string[] LMbalanceEnemy = new string[52]
    {
        "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        "0","0","0","0","0","0","0","0","0","0","0","0"
    };
    Dictionary<string, int> indexer = new Dictionary<string, int>()
    {
        {"1_C",0},
        {"2_C",1},
        {"3_C",2},
        {"4_C",3},
        {"5_C",4},
        {"6_C",5},
        {"7_C",6},
        {"8_C",7},
        {"9_C",8},
        {"10_C",9},
        {"11_C",10},
        {"12_C",11},
        {"13_C",12},
        {"1_S",13},
        {"2_S",14},
        {"3_S",15},
        {"4_S",16},
        {"5_S",17},
        {"6_S",18},
        {"7_S",19},
        {"8_S",20},
        {"9_S",21},
        {"10_S",22},
        {"11_S",23},
        {"12_S",24},
        {"13_S",25},
        {"1_H",26},
        {"2_H",27},
        {"3_H",28},
        {"4_H",29},
        {"5_H",30},
        {"6_H",31},
        {"7_H",32},
        {"8_H",33},
        {"9_H",34},
        {"10_H",35},
        {"11_H",36},
        {"12_H",37},
        {"13_H",38},
        {"1_D",39},
        {"2_D",40},
        {"3_D",41},
        {"4_D",42},
        {"5_D",43},
        {"6_D",44},
        {"7_D",45},
        {"8_D",46},
        {"9_D",47},
        {"10_D",48},
        {"11_D",49},
        {"12_D",50},
        {"13_D",51},
    };
    public AudioSource[] sounds;
    
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            var random = new System.Random();
            seed = random.Next(cards.Count);
            PV.RPC("RPC_generateRandom", RpcTarget.OthersBuffered, seed);
        }
        CheckConnected();
    }

    [PunRPC]
    public void SetOpponentSkin(string type, int index, string value)
    {
        if (type == "SK")
        {
            SKbalanceEnemy[index] = value;
        }
        else
        {
            LMbalanceEnemy[index] = value;
        }
    }

    public async void CheckConnected()
    {
        connected = await ThirdWebManager.Instance.SDK.wallet.IsConnected();
        Debug.Log(connected);

        address = await ThirdWebManager.Instance.SDK.wallet.GetAddress();
        Debug.Log(address);
        var x = await ThirdWebManager.Instance.SDK.wallet.GetBalance("0x3c988602f42C894a1f5B08491b03EE6F2C261CAb");
        Debug.Log(x);
    }

    public async void getBalanceSK()
    {
        var contractSK = ThirdWebManager.Instance.SDK.GetContract("0xb48fAf5A2B3Ef7a28919AB45e93e9Da1F90dA598");
        for(int i = 0; i < SKbalance.Length; i++)
        {
            SKbalance[i] = await contractSK.ERC1155.BalanceOf(address,i.ToString());
            PV.RPC("SetOpponentSkin", RpcTarget.OthersBuffered, "SK", i, SKbalance[i]);
        }
    }

    public async void getBalanceLM()
    {
        var contractLM = ThirdWebManager.Instance.SDK.GetContract("0xA958176D10b9F3d157b8afed0e019bba74D5F9bA");
        for (int i = 0; i < LMbalance.Length; i++)
        {
            LMbalance[i] = await contractLM.ERC1155.BalanceOf(address, i.ToString());
            PV.RPC("SetOpponentSkin", RpcTarget.OthersBuffered, "LM", i, LMbalance[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(matchMaking == true && Players[1] == null)
        {
            PhotonNetwork.Disconnect();
            WinnerDialog.SetActive(true);
            WinnerDialog.GetComponentInChildren<TextMeshProUGUI>().text = "Opponent Disconnected";
        }
        if (winner)
        {
            PhotonNetwork.Disconnect();
            WinnerDialog.SetActive(true);
        }
        if (Players.Length != 2)
        {
            Players = GameObject.FindGameObjectsWithTag("Players");
        }
        if (Players.Length == 2 && matchMaking == false)
        {
            if (PhotonNetwork.IsMasterClient == false)
            {
                current = 1;
            }
            foreach (GameObject player in Players)
            {
                turnCaller[player] = true;
            }
            for (int i = 0; i < Players.Length; i++)
            {
                currentScore.Add(0);
                roundScore.Add(0);
            }
            if (current == 0)
            {
                lastRoutine = StartCoroutine(CountDown());
            }
            music.Play();
            matchMaking = true;
            if (address != "")
            {
                getBalanceSK();
                getBalanceLM();
            }
        }
        if (current == 0)
        {
            TurnNotifier.text = "Your Turn...";
        }
        else
        {
            TurnNotifier.text = "Waiting for opponent...";         
        }

        if(matchMaking == true && Players.Length == 2)
        {
            timeRemaining -= Time.deltaTime;
        }

 
        Timers[current].GetComponent<Slider>().value = (timeRemaining / 12);
        Timers[current].SetActive(true);
        Timers[current == 0 ? 1 : 0].SetActive(false);
        
        
        var MainRounds = GameObject.FindGameObjectWithTag("MainRoundScore").GetComponent<TextMeshProUGUI>();
        MainRounds.text = roundScore[0].ToString();
        var OpponentRounds = GameObject.FindGameObjectWithTag("RoundScore").GetComponent<TextMeshProUGUI>();
        OpponentRounds.text = roundScore[1].ToString();


        if (progress == true)
        {
            selected.Clear();
            HiddenCover.SetActive(false);
            var MainScores = GameObject.FindGameObjectWithTag("MainPlayerScore").GetComponent<TextMeshProUGUI>();
            MainScores.text = currentScore[0].ToString();
            var OpponentScores = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
            OpponentScores.text = currentScore[1].ToString();
            for (int i = 0; i < currentScore.Count; i++)
            {
                if (Mathf.Abs(currentScore[i] - 21) < minScore && currentScore[i] - 21 <= 0)
                {
                    minScore = Mathf.Abs(currentScore[i] - 21);
                    selectedScore = currentScore[i];
                }
            }
            if (minScore == 100)
            {
                for (int i = 0; i < currentScore.Count; i++)
                {
                    if (Mathf.Abs(currentScore[i] - 21) <= minScore)
                    {
                        minScore = Mathf.Abs(currentScore[i] - 21);
                        selectedScore = currentScore[i];
                    }
                }
            }
            for (int i = 0; i < currentScore.Count; i++)
            {
                if (currentScore[i] == selectedScore)
                {
                    selected.Add(Players[i]);
                    roundScore[i]++;
                }
                currentScore[i] = 0;
            }
            foreach (GameObject roundWinner in selected)
            {
                if (GameObject.ReferenceEquals(roundWinner, Players[0]))
                {
                    Notifiers[0].GetComponent<Image>().color = new Color(0.3f, 0.6f, 0.3f, 1f);
                    Notifiers[0].GetComponentInChildren<TextMeshProUGUI>().text = "+1";
                    Notifiers[0].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1f);
                }
                if (GameObject.ReferenceEquals(roundWinner, Players[1]))
                {
                    Notifiers[1].GetComponent<Image>().color = new Color(0.3f, 0.6f, 0.3f, 1f);
                    Notifiers[1].GetComponentInChildren<TextMeshProUGUI>().text = "+1";
                    Notifiers[1].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1f);
                }
            }
            currentRound++;
            int maxScore = 0;
            if (currentRound == maxRound)
            {
                IEnumerable<int> duplicates = roundScore.GroupBy(x => x)
                                        .Where(g => g.Count() > 1)
                                        .Select(x => x.Key);

                if (duplicates.Count() > 0)
                {
                    maxRound++;
                }
                else
                {
                    for (int i = 0; i < roundScore.Count; i++)
                    {
                        if (maxScore < roundScore[i])
                        {
                            maxScore = roundScore[i];
                            winner = Players[i];
                        }
                    }
                }
            }
            foreach (GameObject player in Players)
            {
                turnCaller[player] = true;
            }
            turnCount = 0;
            progress = false;
            minScore = 100;
            k = 0;
            l = 0;
        }
    }

    public void checkRound()
    {
        if (turnCount == Players.Length * 6)
        {
            foreach (GameObject player in Players)
            {
                turnCaller[player] = false;
            }
            turnCount = 0;
        }
        foreach (GameObject player in Players)
        {
            if (turnCaller[player] == false)
            {
                progress = true;
            }
            else
            {
                progress = false; break;
            }
        }
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(12);
       photonView.RPC("RPC_Stand", RpcTarget.AllBuffered);
        
    }
    
    public void Hit()
    {
        if (lastRoutine != null)
            StopCoroutine(lastRoutine);
        
        timeRemaining = 12;

        sounds[0].Play();
        HiddenCover.SetActive(true);
        if (k == 0 && l == 0)
        {
            hands[0].sprite = cardBack;
            handsOpponent[0].sprite = cardBack;
            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].GetComponent<Image>().enabled = false;
                hands[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                handsOpponent[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                handsOpponent[i].GetComponent<Image>().enabled = false;
            }
        }
        var OpponentScores = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        OpponentScores.text = "?";
        turnCaller[Players[current]] = true;
        turnCount++;
        var random = new System.Random(seed += 1000);
        var index = random.Next(cards.Count);
        selectedNum = cards[index].Item1;
        selectedSymbol = cards[index].Item2;

        cards.Remove(cards[index]);
        if (cards.Count == 0)
        {
            cards = new List<Tuple<int, char>>(resetCards);
            sounds[2].Play();
            Notifiers[current == 0 ? 1 : 0].GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 1f);
            Notifiers[current == 0 ? 1 : 0].GetComponentInChildren<TextMeshProUGUI>().text = "Reshuffling";
            Notifiers[current == 0 ? 1 : 0].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1f);
        }
        if (selectedNum == 1)
        {
            if (currentScore[current] + 11 > 21)
            {
                currentScore[current] += selectedNum;
            }
            else
            {
                currentScore[current] += 11;
            }
        }
        else
        {
            currentScore[current] += selectedNum > 10 ? 10 : selectedNum;
        }
        Notifiers[current].GetComponent<Image>().color = new Color(0.3f, 0.6f, 0.3f, 1f);
        Notifiers[current].GetComponentInChildren<TextMeshProUGUI>().text = "Hit";
        Notifiers[current].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1f);
        for (int i = 0; i < cardSprites.Length; i++)
        {
            string str;
            if (selectedNum.ToString().Length == 1)
            {
                str = selectedNum.ToString();
            }
            else
            {
                str = selectedNum.ToString();
            }
            string balanceSK = SKbalance[indexer[selectedNum.ToString() + "_" + selectedSymbol]];
            string balanceLM = LMbalance[indexer[selectedNum.ToString() + "_" + selectedSymbol]];
            if (int.Parse(balanceSK) > 0 && int.Parse(balanceLM) > 0)
            {
                var rand = new System.Random();
                var skin = rand.Next(0, 1);
                if(skin == 0)
                {
                    if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 0 && cardSprites[i].name.Contains("SK"))
                    {
                        hands[k].sprite = cardSprites[i];
                        hands[k].GetComponent<Image>().enabled = true;
                        if (k > 1)
                            hands[k - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        k++;
                        break;
                    }
                }
                if (skin == 1)
                {
                    if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 0 && cardSprites[i].name.Contains("LM"))
                    {
                        hands[k].sprite = cardSprites[i];
                        hands[k].GetComponent<Image>().enabled = true;
                        if (k > 1)
                            hands[k - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        k++;
                        break;
                    }
                }
            }
            else if(int.Parse(balanceSK) > 0)
            {
                if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 0 && cardSprites[i].name.Contains("SK"))
                {
                    hands[k].sprite = cardSprites[i];
                    hands[k].GetComponent<Image>().enabled = true;
                    if (k > 1)
                        hands[k - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    k++;
                    break;
                }
            }
            else if(int.Parse(balanceLM) > 0)
            {
                if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 0 && cardSprites[i].name.Contains("LM"))
                {
                    hands[k].sprite = cardSprites[i];
                    hands[k].GetComponent<Image>().enabled = true;
                    if (k > 1)
                        hands[k - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    k++;
                    break;
                }
            }
            else
            {
                if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 0)
                {
                    hands[k].sprite = cardSprites[i];
                    hands[k].GetComponent<Image>().enabled = true;
                    if (k > 1)
                        hands[k - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    k++;
                    break;
                }
            }

            string balanceSKEnemy = SKbalanceEnemy[indexer[selectedNum.ToString() + "_" + selectedSymbol]];
            string balanceLMEnemy = LMbalanceEnemy[indexer[selectedNum.ToString() + "_" + selectedSymbol]];
            if (int.Parse(balanceSKEnemy) > 0 && int.Parse(balanceLMEnemy) > 0)
            {
                var rand = new System.Random();
                var skin = rand.Next(0, 1);
                if (skin == 0)
                {
                    if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 1 && cardSprites[i].name.Contains("SK"))
                    {
                        handsOpponent[k].sprite = cardSprites[i];
                        handsOpponent[k].GetComponent<Image>().enabled = true;
                        if (k > 1)
                            handsOpponent[k - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        k++;
                        break;
                    }
                }
                if (skin == 1)
                {
                    if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 1 && cardSprites[i].name.Contains("LM"))
                    {
                        handsOpponent[k].sprite = cardSprites[i];
                        handsOpponent[k].GetComponent<Image>().enabled = true;
                        if (k > 1)
                            handsOpponent[k - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        k++;
                        break;
                    }
                }
            }
            else if (int.Parse(balanceSKEnemy) > 0)
            {
                if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 1 && cardSprites[i].name.Contains("SK"))
                {
                    handsOpponent[k].sprite = cardSprites[i];
                    handsOpponent[k].GetComponent<Image>().enabled = true;
                    if (k > 1)
                        handsOpponent[k - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    k++;
                    break;
                }
            }
            else if (int.Parse(balanceLMEnemy) > 0)
            {
                if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 1 && cardSprites[i].name.Contains("LM"))
                {
                    handsOpponent[k].sprite = cardSprites[i];
                    handsOpponent[k].GetComponent<Image>().enabled = true;
                    if (k > 1)
                        handsOpponent[k - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    k++;
                    break;
                }
            }
            else
            {
                if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 1)
                {
                    handsOpponent[k].sprite = cardSprites[i];
                    handsOpponent[k].GetComponent<Image>().enabled = true;
                    if (k > 1)
                        handsOpponent[k - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    k++;
                    break;
                }
            }
        }
        var MainScores = GameObject.FindGameObjectWithTag("MainPlayerScore").GetComponent<TextMeshProUGUI>();
        MainScores.text = currentScore[0].ToString();
        current++;
        if (current == Players.Length)
        {
            current = 0;
        }
        checkRound();
        if(current == 0)
            lastRoutine = StartCoroutine(CountDown());
    }

    [PunRPC]
    public void RPC_generateRandom(int getSeed)
    {
        seed = getSeed;
    }

    public void Stand()
    {
        if (lastRoutine != null)
            StopCoroutine(lastRoutine);
 
        timeRemaining = 12;

        sounds[1].Play();
        HiddenCover.SetActive(true);
        var MainScores = GameObject.FindGameObjectWithTag("MainPlayerScore").GetComponent<TextMeshProUGUI>();
        MainScores.text = currentScore[0].ToString();
        var OpponentScores = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        OpponentScores.text = "?";
        if (k == 0 && l == 0)
        {
            hands[0].sprite = cardBack;
            handsOpponent[0].sprite = cardBack;
            for (int i = 1; i < hands.Length; i++)
            {
                hands[i].GetComponent<Image>().enabled = false;
                handsOpponent[i].GetComponent<Image>().enabled = false;
            }
        }
        Notifiers[current].GetComponent<Image>().color = new Color(0.6f, 0.3f, 0.3f, 1f);
        Notifiers[current].GetComponentInChildren<TextMeshProUGUI>().text = "STAND";
        Notifiers[current].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1f);
        turnCaller[Players[current]] = false;
        current++;
        turnCount++;
        if (current == Players.Length)
        {
            current = 0;
        }
        checkRound();
        if(current == 0)
            lastRoutine = StartCoroutine(CountDown());
    }
    
    [PunRPC]
    public void RPC_Stand()
    {
        Stand();
        Debug.Log("Stand");
    }
    
    [PunRPC]
    public void RPC_Hit()
    {
        Hit();
        Debug.Log("Hit");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        WinnerDialog.SetActive(true);
        WinnerDialog.GetComponentInChildren<TextMeshProUGUI>().text = "Disconnected From Server";
    }
}
