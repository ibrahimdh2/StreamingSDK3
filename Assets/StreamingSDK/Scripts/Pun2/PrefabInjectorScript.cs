
using UnityEngine;
# if UNITY_EDITOR
using UnityEditor;
public class PrefabInjectorScript : Editor
{
    
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
       
    }
    [MenuItem("StreamerSDK/Set Watcher")]
    public static void SetWatcher()
    {
        GameObject watcherObject = PlacePrefab(GetPrefabPath("StreamWatcherSDK"));
        watcherObject.name = "StreamWatcher";
    }
}
#endif
