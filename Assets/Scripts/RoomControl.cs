using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomControl : MonoBehaviourPunCallbacks //IInRoomCallbacks
{

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Player 1", new Vector3(-5, 0, 0), new Quaternion(0f, 0f, 0f, 0f));
        }
        else
        {
            PhotonNetwork.Instantiate("Player 2", new Vector3(5, 0, 0), new Quaternion(0f, 0f, 0f, 0f));
            transform.eulerAngles = Vector3.up * 180;
        }
    }


    /*public interface OnPlayerEnteredRoom<Player>
    {
        void 
    }*/

    /*// Called when a remote player left the room or became inactive.Check otherPlayer.IsInactive.More...
    void OnPlayerLeftRoom(Player otherPlayer)
    {

    }

    //     Called when a room's custom properties changed. The propertiesThatChanged contains all that was set via Room.SetCustomProperties. More...
    void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {

    }

    //     Called when custom player-properties are changed.Player and the changed properties are passed as object[]. More...
    void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {

    }

    void OnMasterClientSwitched(Player newMasterClient)
    {

    }*/
}
