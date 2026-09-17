using UnityEngine;

public class SceneController : MonoBehaviour
{
    /*
public static SceneController Instance;
    void Awake() {
    if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }    
    Instance = this;
    }

    [SerializeField] private LoadingOverlay loadingOverlay;


// its a log of all of the scenes within the game
    private Dictionary<string,string> loadSceneBySlot = new();
    private bool isBusy = false; // we are already in a scene transition
    public SceneTransitionPlan NewTransition()
    {
        return new SceneTransitionPlan();
    //creating a scene transition plan that is able to be called to use the other methods
    }

    private Coroutine ExecutePlan(SceneTransitionPlan plan)
    {
        if (isBusy)
        {
            Debug.Debug.LogWarning("Scene change is happening");
            return null;
            //if the scene is changing it will stop here
        }
        isBusy = true;
        return StartCoroutine(ChangeSceneRoutine(plan));
        //this starts the process of changing the scene
    }
    private IEnumerator ChangeSceneRoutine(SceneTransitionPlan plan)
    {
        if (plan.Overlay)
        {
            yield return loadingOverlay.FadeInBlack();
            yield return new WaitForSeconds(0.5f);
            //fade to the next scene
        }
        foreach (var slotKey in plan.ScenesToUnload)
        {
            //unload scenes that we do not need anymore
            yield return UnloadSceneRoutine(slotKey);
        }
        if(plan.ClearUnusedAssets) yield return CleanupUnusedAssetsRoutine();
        foreach (var kvp in plan.ScenesToLoad)
        {
            if (loadedSceneBySlot.ContainsKey(kvp.Key))
            {
                yield return UnloadSceneRoutine(kvp.Key);
            }
            yield return LoadAdditiveRoutine(kvp.Key, kvp.Value, plan.ActiveSceneName == kvp.Value);
        }
        if (plan.Overlay)
            {
                yield return loadingOverlay.FadeOutBlack();
            }
            isBusy = false;
    }

    private IEnumerator LoadAdditiveRoutine(string slotKey, string sceneName, bool setActive)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if(loadOp == null) yield break;
        loadOp.allowSceneActivation = false;
        while(loadOp.progress < 0.9f)
        {
            yield return null;
        }
        loadOp.allowSceneActivation = true;
        while (!loadOp.isDone)
        {
            yield return null;
        }
        if (setActive)
        {
            Scene newScene = SceneManager.GetSceneByName(sceneName);
            if(newScene.IsValid() && newScene.isLoaded)
            {
                SceneManager.setActive(newScene);
            }
        }
        loadedSceneBySlot[slotKey] = sceneName;
    }

    private IEnumerator UnloadSceneRoutine(string slotKey)
    {
        if(!loadedSceneBySlot.TryGetValue(slotKey, out string sceneName)) yield break;
        if(string.IsNullOrEmpty(sceneName)) yield break;
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
        if(unloadOp != null)
        {
            while (!unloadOp.isDone)
            {
                yield return null;
            }
        }
        loadedSceneBySlot.Remove(slotKey);
    }

    private IEnumerator CleanupUnusedAssetsRoutine()
    {
        AsyncOperation cleanupOp = Resources.UnloadUnusedAssets();
        while (!cleanupOp.isDone)
        {
            yield return null;
        }
    }
    public class SceneTransitionPlan
    {
        public Dictionary<string, string> ScenesToLoad{get;} = new(); 
        public List<string> ScenesToUnload{ get; } = new();
        public string ActiveSceneName {get; private set;} = "";
        public bool ClearUnusedAssets {get; private set; } = false;
        public bool Overlay { get; private set; } = false;

        public SceneTransitionPlan Load(string slotkey, string sceneName, bool setActive = false)
        {
            ScenesToLoad[slotKey] = sceneName; //Grabs the scene that will be loaded
            if(setActive) ActiveSceneName = sceneName; //tells which scene is being used
            return this;
        }
        public SceneTransitionPlan Unload(string slotKey)
        {
            ScenesToUnload.Add(slotKey);//tells the manager what scene to unoad
            return this; 
        }
        public SceneTransitionPlan WithOverlay()
        {
            Overlay = true;
            return this;
        }
        public SceneTransitionPlan WithClearUnusedAssets()
        {
            ClearUnusedAssets = true;
            return this;
        }
        public Coroutine Perform()
        {
            return SceneController.Instance.ExecutePlan(this);
        }
    }   
    */
}
