using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneGroupManager
{
    public event Action<string> OnSceneLoaded = delegate {};
    //When the scene load happens
    public event Action<string> OnSceneUnloaded = delegate {};
    //When the scene needs to fucking die
    public event Action OnSceneGroupLoaded = delegate {};
    //When there has to be multiple scenes in the game *WINK WINK* like a minigame pop up
    
    SceneGroup ActiveSceneGroup; 
    //This shows which ones are loaded  
    public readonly struct AsyncOperationGroup 
    {// i need to 
        public readonly List<AsyncOperation> Operations;
    }
}
