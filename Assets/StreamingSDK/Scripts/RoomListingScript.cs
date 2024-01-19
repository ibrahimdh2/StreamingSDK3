using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListingScript : MonoBehaviour
{
    
    public TextMeshProUGUI roomNameUI;
    public string roomName;
    public Button btn;

    public void SetRoomName(string _roomName)
    {
        roomNameUI.text = roomName =  _roomName;
        btn.onClick.AddListener(OpenRoom);
    }
    public void OpenRoom()
    {
        Debug.Log("Trying to open the room");
        PhotonNetwork.JoinRoom(roomName);
        PhotonNetwork.LoadLevel("WatcherScene");
    }
}
