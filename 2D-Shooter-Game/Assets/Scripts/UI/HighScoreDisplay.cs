using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class inherits from the UIelement class and handles the display of the high score
/// </summary>
 
public class HighScoreDisplay : UIelement
{
    [Tooltip("The text UI to use for display")]
    public Text displayText = null;

    //Changes the high score display
    public void DisplayHighScore()
    {
        if (displayText != null)
        {
            displayText.text = "High: " + GameManager.instance.highScore.ToString();
        }
    }

    //Overrides the virtual function UpdateUI() of the UIelement class and uses the DisplayHighScore function to update
    public override void UpdateUI()
    {
        // This calls the base update UI function from the UIelement class
        base.UpdateUI();

        // The remaining code is only called for this sub-class of UIelement and not others
        DisplayHighScore();
    }
}
