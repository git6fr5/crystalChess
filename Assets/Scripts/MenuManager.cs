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
    public Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        playButton.interactable = true;
    }

    // Update is called once per frame
    public void Play()
    {
        string playerNameText = playerName.text;
        if (roomName.text != "")
        {
            PhotonNetwork.JoinRoom(roomName.text);
        }
        else { PhotonNetwork.JoinRandomRoom(); }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        RoomOptions roomOps = new RoomOptions();
        roomOps.IsVisible = true;
        roomOps.IsOpen = true;
        PhotonNetwork.NickName = playerName.text;
        string roomNameText = roomName.text;
        PhotonNetwork.CreateRoom(roomNameText, roomOps);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOps = new RoomOptions();
        roomOps.IsVisible = true;
        roomOps.IsOpen = true;
        PhotonNetwork.NickName = playerName.text;
        string roomNameText = "Room" + Random.Range(0, 1000).ToString();
        PhotonNetwork.CreateRoom(roomNameText, roomOps);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
