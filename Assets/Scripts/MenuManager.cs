using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{

    public InputField playerName;
    public InputField roomName;

    public Button createRoomButton;
    public Button joinRoomButton;


    // Start is called before the first frame update
    void Start()
    {
        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;

        print("on connected");
    }

    public override void OnDisconnected(DisconnectCause cause) 
    {
        print("disconnect because " + cause.ToString());
    }

    // Update is called once per frame
    public void OnClick_CreateRoom()
    {
        if (roomName.text != "")
        {
            PhotonNetwork.CreateRoom(roomName.text);
        }
        else { print("need to give room a name"); }
    }

    public void OnClick_JoinRoom()
    {
        //
    }



    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
