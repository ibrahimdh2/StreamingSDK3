using FMETP;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class VideoRecorder : MonoBehaviour
{
    public List<AudioRenderer> audioRenderers = new List<AudioRenderer>();
    public float recordTime;
    public AudioListener audioListener;
    public GameViewEncoder gameViewEncoder;
    public bool recordingAudio;
    public bool recordingVideo;
    public string applicationPath;
    public string imagesPath;
    public string audioPath;

    private void Awake()
    {
        applicationPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));


        imagesPath = applicationPath + "/Images/";
        audioPath = applicationPath + "/Audio/";
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            
            if (Directory.Exists(imagesPath))
            {
                //Delete everything int he directory
                Directory.Delete(imagesPath, true);
                Directory.CreateDirectory(applicationPath + "/Images/");
            }
            else
            {
                Directory.CreateDirectory(applicationPath + "/Images/");
            }

            if (Directory.Exists(applicationPath + "/Audio/"))
            {
                //Delete everything int he directory
                Directory.Delete(audioPath, true);
                Directory.CreateDirectory(applicationPath + "/Audio/");
            }
            else
            {
                Directory.CreateDirectory(applicationPath + "/Audio/");
            }
            gameViewEncoder.SaveAllPictures(imagesPath);
            StopRecordingAudioVideo();
            SaveAudios();




        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            StartRecordingAudioVideo();
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
            Debug.Log("Saving Audio");
            audioRenderer.Save(audioPath +audioIndex.ToString() +".wav");
            audioIndex++;
         
        }
    }

}
