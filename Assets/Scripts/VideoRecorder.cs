using FMETP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoRecorder : MonoBehaviour
{
    public List<AudioRenderer> audioRenderers;
    public float recordTime;
    public AudioListener audioListener;
    public GameViewEncoder gameViewEncoder;
    public bool recordingAudio;
    public bool recordingVideo; 


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (recordingAudio && recordingVideo)
            {
                StopRecordingAudioVideo();
                SaveAudios();
            }
            else if (!recordingAudio && !recordingVideo) 
            {
                StartRecordingAudioVideo();
            }
           

        }
        

    }
    public void StartRecordingAudioVideo()
    {
        if (gameViewEncoder == null)
        {
            gameViewEncoder = GameObject.FindAnyObjectByType<GameViewEncoder>();
            if (gameViewEncoder == null)
            {
                Debug.LogError("Couldn't find GameViewEncoder");
            }
        }
        else
        {
            StartRecordingVideo();
            StartRecordingAudio();
        }

        
    }
    public void StopRecordingAudioVideo()
    {
        StopRecordingVideo();
        StopRecordingAudio();
    }
    private void StartRecordingVideo()
    {
        gameViewEncoder.storeVideo = true;
        recordingVideo = true;
        Debug.Log("Recording Video");
    }
    private void StopRecordingVideo()
    {
        gameViewEncoder.storeVideo = false;
        recordingVideo = false;
        Debug.Log("Stopped Recording Video");
    }
    private void StartRecordingAudio()
    {
        foreach (AudioRenderer renderer in audioRenderers)
        {
            renderer.Rendering = true;
        }
        recordingAudio = true;
        Debug.Log("Recording Audio");
    }
    private void StopRecordingAudio()
    {
        foreach (AudioRenderer renderer in audioRenderers)
        {
            renderer.Rendering = false;
        }
        recordingAudio = false;
        Debug.Log("Stopped Recording Audio");
    }
    private void SaveAudios()
    {
        int audioIndex = 0;
        foreach (AudioRenderer audioRenderer in audioRenderers)
        {
            audioRenderer.Save("V://Audios/"+audioIndex.ToString());
            audioIndex++;
         
        }
    }

}
