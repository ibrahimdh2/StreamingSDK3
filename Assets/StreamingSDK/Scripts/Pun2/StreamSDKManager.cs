
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using FMETP;
using UnityEngine.Events;
using System.Linq;
using WebSocketSharp;


public class StreamSDKManager : MonoBehaviourPunCallbacks
{
   
    public PUNStreamManager PUNStreamManager;
    [Tooltip("The camera you want to scream. It can be some other camera than the MainCamera")]
    public GameObject mainCamera;
    [HideInInspector]
    public GameViewDecoder decoder;
    [HideInInspector]
    public GameViewEncoder encoder;
    [HideInInspector]
    public AudioEncoder audioEncoder;
    [HideInInspector]
    public AudioDecoder audioDecoder;
    [Tooltip("Streamer/Watcher modes to setup streamer and watcher")]
    public enum Mode { Streamer, Watcher };
    public Mode currentMode;
    [Tooltip("Should Stream Audio as well")]
    public bool audioStreaming;
    [Tooltip("Name of the Photon Room you want to stream")]
    public string streamingRoomName;
    [Tooltip("Nickname of the Photon Player")]
    public string playerName;
    [Tooltip("Number of players allowed in the streaming room")]
    public int maxPlayerAmount;

    [Tooltip("Turns on all the debug messages")]
    public bool debug;
    [Tooltip("Assign this for Streamer. It is called when streamer is disconnected")]
    public UnityEvent OnStreamerDisconnected;
    [Tooltip("Assign this for Watcher. It is called when Watcher is disconnected")]
    public UnityEvent OnWatcherDisconnected;
    [Tooltip("Turn on webcam streaming")]
    public bool webcam;
    public GameObject webCamObject;
    public bool micStreaming;





    private void Start()
    {
        if (currentMode == Mode.Streamer || (currentMode == Mode.Watcher && !PhotonNetwork.IsConnected))
        {
            PhotonNetwork.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        if (currentMode == Mode.Streamer && webCamObject != null)
        {
            try
            {
                if (webcam)
                {
                    webCamObject.SetActive(true);
                }
                else
                {
                    webCamObject.SetActive(false);
                }
            }
            catch (System.Exception)
            {

                Debug.Log("Web cam not found");
            }
        }
       
    }



    #region PhotonCallbacks
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        if (debug)
        {
            Debug.Log("Connected to master"); 
        }
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        if(debug)

    {
            Debug.Log("Joined Lobby"); 
        }
        if (currentMode == Mode.Streamer)
        {
            RoomOptions roomOptions = new RoomOptions() { MaxPlayers = maxPlayerAmount, IsVisible = true, IsOpen = true };
            if (streamingRoomName.IsNullOrEmpty())
            {
                streamingRoomName = Application.productName + playerName;
                   
            }
            PhotonNetwork.JoinOrCreateRoom(streamingRoomName, roomOptions, TypedLobby.Default);
          
        }
        else
        {
            PhotonNetwork.JoinRoom(streamingRoomName);
        }
   
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (debug)
        {
            Debug.Log($"{PhotonNetwork.NickName} Joined {PhotonNetwork.CurrentRoom} "); 
        }

        if (currentMode == Mode.Streamer)
        {
            if (debug)
            {
                Debug.Log($"{PhotonNetwork.NickName} is the master client"); 
            }
            TurnOnStreamer();
        }
        else
        {
            if (debug)
            {
                Debug.Log($"{PhotonNetwork.NickName} is watching client"); 
            }
            TurnOnWatcher();
        }

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if (debug)
        {
            Debug.Log("Disconnected"); 
        }

        if (currentMode == Mode.Streamer)
        {
            OnStreamerDisconnected.Invoke();
        }
        else
        {
            OnWatcherDisconnected.Invoke();
        }
    }
    #endregion


    /// <summary>
    /// This function sets up the streaming scene
    /// </summary>
    public void TurnOnStreamer()
    {
        if (debug)
        {
            Debug.Log("Streamer being turned on"); 
        }

        if (mainCamera != null)
        {
            encoder = mainCamera.AddComponent<GameViewEncoder>();
            encoder.RenderCam = mainCamera.GetComponent<Camera>();
            encoder.StreamFPS = 25;
            encoder.Quality = 10;
            encoder.Resolution = new Vector2(1080,720);
            
            audioEncoder = GameObject.FindAnyObjectByType<AudioEncoder>();
            //For some reason, sometimes it doesn't work so assigning it manually in the inspector
            encoder.OnDataByteReadyEvent.AddListener(PUNStreamManager.Action_SendData);
            if (audioStreaming && audioEncoder != null)
            {
                audioEncoder.OnDataByteReadyEvent.AddListener(PUNStreamManager.Action_SendData);
                Debug.Log($"Audio encoder event added");
            }
            else
            {
                Debug.Log($"Audio encoder is null");
            }

      
        }
        else
        {
            Debug.LogError("Camera reference is not assigned in the scene manager");
        }
        

        



    }
    /// <summary>
    /// This function sets up the watching scene
    /// </summary>
    public void TurnOnWatcher()
    {
        if (debug)
        {
            Debug.Log("Watcher being turned on"); 
        }
        //For some reason, sometimes it doesn't work so assigning it manually in the inspector
        PUNStreamManager.OnDataByteReadyEvent.AddListener(decoder.Action_ProcessImageData);
        audioDecoder = GameObject.FindAnyObjectByType<AudioDecoder>();
        if (audioStreaming && audioDecoder != null)
        {
            PUNStreamManager.OnDataByteReadyEvent.AddListener(audioDecoder.Action_ProcessData);
            Debug.Log("Audio Decoder Attached");

        }
        else
        {
            Debug.Log("Audio Decoder is null");
        }

        Debug.Log("Audio Streaming Done");
    }


   

  
}
