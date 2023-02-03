using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinRandomOrCreateRoom(null , 0 , MatchmakingMode.FillRoom, null, null, null, roomOptions, null);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Here");
        PhotonNetwork.LoadLevel("PlayDemo");
    }
}
