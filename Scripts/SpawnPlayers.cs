using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject dealer;
    public GameObject Canvas;
    public GameObject[] SpawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        var newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);  
    }

    // Update is called once per frame
    void Update()
    {

        GameObject[] Players = GameObject.FindGameObjectsWithTag("Players");
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].transform.SetParent(Canvas.transform);
            Players[i].transform.position = SpawnPoints[i].transform.position;
        }

        if (Players.Length == 2)
        {
            dealer.SetActive(true);
        }
    }

}
