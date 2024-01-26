using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    public AudioRenderer audioRenderer;


    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            audioRenderer.Rendering = !audioRenderer.Rendering;
            Debug.Log("Recording Audio");
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            audioRenderer.Save("V://audio.txt");
        }
    }

    public void RecordVideo()
    {
        
    }
    public void ProcessVideo()
    {
        
    }
}
