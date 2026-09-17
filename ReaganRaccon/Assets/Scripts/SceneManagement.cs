using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SceneGroups
{

    public class SceneData
    {
        //
        //Contains all details about each scene
        public SceneType SceneType;

        public List<SceneData> Scenes;
        public string FindSceneNameByType(SceneType sceneType){
            return SceneData.FirstOrDefault(scene => scene.SceneType == sceneType)?.Reference.Name;
        }


    }
    [Serializable]
    public enum SceneType{ActiveScene, MainMenu, MinigameScene, OverWorld, LevelSelect}
    //ActiveScene: Scene we are using
    //MainMenu: Load the start of the game
    //MinigameScene: All of the minigame will be labeled like this
    //OverWorld: Where regan can move around 
    //LevelSelect: takes you to the level selection
}
