using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This class is meant to be used on buttons as a quick easy way to load levels (scenes)
/// </summary>

public class LevelLoadButton : MonoBehaviour
{
    //Loads a level according to the name provided
    public void LoadLevelByName(string levelToLoadName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelToLoadName);
    }
}
