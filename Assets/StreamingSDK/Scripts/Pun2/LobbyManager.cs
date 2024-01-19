using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField nameField;
    public TMP_InputField roomField;
    public GameObject connectingCanvas;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.NickName = nameField.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to master server");
        PhotonNetwork.JoinLobby();
        connectingCanvas.SetActive(false);
    }
    public void CreateOrJoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinOrCreateRoom(roomField.text, new RoomOptions() { MaxPlayers = 10 }, TypedLobby.Default);
        }
        else
        {

            Debug.Log("Is not connected wait for the connected then try again");
        }

    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Room joined");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("StreamingRoom");
        }
        else
        {
            PhotonNetwork.LoadLevel("WatchingRoom");
        }
    }

   
}
