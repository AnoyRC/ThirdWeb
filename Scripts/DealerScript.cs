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
using Unity.Mathematics;

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
    float timeRemaining = 8;
    Coroutine lastRoutine;
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
    }

    // Update is called once per frame
    void Update()
    {
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
            matchMaking = true;
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

 
        Timers[current].GetComponent<Slider>().value = (timeRemaining / 8);
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
                    Notifiers[0].GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 1f);
                    Notifiers[0].GetComponentInChildren<TextMeshProUGUI>().text = "+1";
                    Notifiers[0].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1f);
                }
                if (GameObject.ReferenceEquals(roundWinner, Players[1]))
                {
                    Notifiers[1].GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 1f);
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
        yield return new WaitForSeconds(8);
       photonView.RPC("RPC_Stand", RpcTarget.AllBuffered);
        
    }
    
    public void Hit()
    {
        if (lastRoutine != null)
            StopCoroutine(lastRoutine);
        
        timeRemaining = 8;
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
        Notifiers[current].GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 1f);
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
            if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 0)
            {
                hands[k].sprite = cardSprites[i];
                hands[k].GetComponent<Image>().enabled = true;
                if (k > 1)
                    hands[k - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                k++;
                break;
            }
            if (cardSprites[i].name.Contains(str) && cardSprites[i].name.Contains(selectedSymbol) && current == 1)
            {
                handsOpponent[l].sprite = cardSprites[i];
                handsOpponent[l].GetComponent<Image>().enabled = true;
                if (l > 1)
                    handsOpponent[l - 1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                l++;
                break;
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
 
        timeRemaining = 8;
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
        Notifiers[current].GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 1f);
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
}
