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
    public Button playButton;

    public RoomListing roomListing;
    public Transform roomList;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;

        PhotonNetwork.JoinLobby();
        print("on connected");
    }

    public override void OnDisconnected(DisconnectCause cause) 
    {
        print("disconnect because " + cause.ToString());
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel(1);
        print("joined room");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomInfoList)
    {
        print("adding a roomm");
        foreach (RoomInfo roomInfo in roomInfoList)
        {
            RoomListing _roomListing = Instantiate(roomListing, roomList);
            _roomListing.gameObject.SetActive(true);
            _roomListing.SetInfo(roomInfo);
        }
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
        if (roomName.text != "")
        {
            PhotonNetwork.JoinRoom(roomName.text);
        }
    }

    public void OnClick_Play()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

}
