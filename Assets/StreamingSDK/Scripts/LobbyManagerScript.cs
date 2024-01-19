using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class LobbyManagerScript : MonoBehaviourPunCallbacks
{
    public Transform T_content;
    public TMP_InputField nameField;
    /// <summary>
    /// The list of room listings
    /// </summary>
    public List<RoomListingScript> roomListingList;
    public GameObject roomListingPrefab;

    


    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.NickName = nameField.text;
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("connected to master");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }
 
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        
        foreach(RoomInfo room in roomList) 
        {
            if (room.RemovedFromList)
            {
                Debug.Log($"{room.Name} room was remove");
                int r_Index = roomListingList.FindIndex(x => x.roomName == room.Name);
                if (r_Index != -1)
                {
                    Destroy(roomListingList[r_Index].gameObject);
                    roomListingList.RemoveAt(r_Index); 
                }
                
                

            }
            else
            {
                Debug.Log($"{room.Name} room was added");
                GameObject _roomListing = Instantiate(roomListingPrefab, T_content);
                RoomListingScript roomListingScript = _roomListing.GetComponent<RoomListingScript>();
                roomListingScript.SetRoomName(room.Name);
                roomListingList.Add(roomListingScript);


            }
           
        }
    }

    
}
