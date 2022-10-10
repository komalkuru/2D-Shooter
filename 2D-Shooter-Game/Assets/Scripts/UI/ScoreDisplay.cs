using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class inherits for the UIelement class and handles updating the score display
/// </summary>

public class ScoreDisplay : UIelement
{
    [Tooltip("The text UI to use for display")]
    public Text displayText = null;

    //Updates the score display
    public void DisplayScore()
    {
        if (displayText != null)
        {
            displayText.text = "Score: " + GameManager.score.ToString();
        }
    }

    //Overides the virtual UpdateUI function and uses the DisplayScore to update the score display
    public override void UpdateUI()
    {
        // This calls the base update UI function from the UIelement class
        base.UpdateUI();

        // The remaining code is only called for this sub-class of UIelement and not others
        DisplayScore();
    }
}
