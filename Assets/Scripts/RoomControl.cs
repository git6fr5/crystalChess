using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomControl : MonoBehaviourPunCallbacks //IInRoomCallbacks
{

    public override void OnJoinedRoom()
    {
        print("connected");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("disconnect because " + cause.ToString());
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
