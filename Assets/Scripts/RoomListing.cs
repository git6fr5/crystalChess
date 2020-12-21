using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListing : MonoBehaviour
{
    public Text roomName;

    public void SetInfo(RoomInfo roomInfo)
    {
        roomName.text = roomInfo.Name + " {" + roomInfo.MaxPlayers + " Players }";
    }
}
