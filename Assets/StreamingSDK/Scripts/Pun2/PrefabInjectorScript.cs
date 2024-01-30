
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using FMETP;


#if UNITY_EDITOR
using UnityEditor;
public class PrefabInjectorScript : Editor
{
    public static List<AudioSource> allAudioSourcesList = new List<AudioSource>();
    public static VideoRecorder videoRecorder = new VideoRecorder(); 
    //**To do** Audio renderer is not being attached to audio source game objects when streamer object is created with the
    //menu item
    //also merging audios and then attaching audio to video

    public static GameObject PlacePrefab(string prefabPath)
    {
        // Get the path to the prefab from the user
        

        if (!string.IsNullOrEmpty(prefabPath))
        {
            // Load the prefab asset
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab != null)
            {
                // Instantiate the prefab in the scene at the origin
                GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);

                // Add the instance to the current scene
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
                return instance;
            }
            else
            {
                Debug.LogError("Failed to load prefab from path: " + prefabPath);
              
            }
           
        }
        return null;
    }

 
    public static string GetPrefabPath(string prefabName)
    {
        string[] guids = AssetDatabase.FindAssets("t:prefab");

        foreach(string guid in guids) 
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if(prefab != null)
            {
                if (prefab.name == prefabName)
                {
                    return assetPath;
                }
            }
        }
        return null;
    }

    [MenuItem("StreamerSDK/Set Streamer")]
    public static void SetStreamerScene()
    {
        
        GameObject streamerObject = PlacePrefab(GetPrefabPath("StreamManagerSDK"));
        streamerObject.name = "StreamManager";
        streamerObject.GetComponent<StreamSDKManager>().mainCamera = Camera.main.gameObject;
        videoRecorder = streamerObject.GetComponent<VideoRecorder>();
        GetAllAudioSourceObjects();






    }
    [MenuItem("StreamerSDK/Set Watcher")]
    public static void SetWatcher()
    {
        GameObject watcherObject = PlacePrefab(GetPrefabPath("StreamWatcherSDK"));
        watcherObject.name = "StreamWatcher";
        

        
    }
    public static void GetAllAudioSourceObjects()
    {
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (var gObject in rootObjects)
        {
            GetAllChildrenWithAudioSource(gObject.transform);
        }

     

        

    }
    public static void GetAllChildrenWithAudioSource(Transform parent)
    {
       
        if (parent.TryGetComponent<AudioSource>(out AudioSource _audioSource))
        {
            allAudioSourcesList.Add(_audioSource);
            if (!_audioSource.gameObject.TryGetComponent<AudioRenderer>(out AudioRenderer renderer))
            {
                renderer = _audioSource.gameObject.AddComponent<AudioRenderer>();
                videoRecorder.audioRenderers.Add(renderer);
                
            }
            //Debug.Log("parent has audio source adding it");

        }
        if (parent.transform.childCount > 0)
        {
           // Debug.Log("parent has children");
           

            for (int i = 0; i < parent.childCount; i++)
            {
                var childObject = parent.GetChild(i);
                //Debug.Log($"child is {parent.GetChild(i)}");
                if (childObject.gameObject.TryGetComponent<AudioSource>(out AudioSource __audioSource)
                    )
                {

             
                    allAudioSourcesList.Add(__audioSource);
                    if (!__audioSource.gameObject.TryGetComponent<AudioRenderer>(out AudioRenderer renderer))
                    {
                        renderer = __audioSource.gameObject.AddComponent<AudioRenderer>();
                       
                        videoRecorder.audioRenderers.Add(renderer);
                    }
                }
                if (childObject.childCount > 0)
                {
                    GetAllChildrenWithAudioSource(childObject);
                }
            }

        }



        
    }

}
#endif
