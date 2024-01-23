using System;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using System.Collections;

class ConvertToVideoScript : MonoBehaviour
{

    IEnumerator EncodeVideo(string frameRate, string inputPath, string outputPath)
    {
        string ffmpegCmd = string.Format("-framerate {0} -i {1} -c:v libx264 -r 30 -y {2}",
                                        frameRate, inputPath, outputPath);

        var ffmpegProcess = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegCmd,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        ffmpegProcess.Start();

        while (!ffmpegProcess.HasExited)
        {
            yield return null;
        }

       
    }
}