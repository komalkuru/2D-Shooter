using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles quitting out of the game
/// </summary>

public class QuitGameButton : MonoBehaviour
{
    //Closes the game or exits play mode depending on the case
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
