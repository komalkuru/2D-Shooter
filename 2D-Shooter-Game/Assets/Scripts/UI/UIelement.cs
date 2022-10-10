using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is a base class meant to be inherited from so multiple differetn UpdateUI functions can be called
/// to handle various cases
/// </summary>

public class UIelement : MonoBehaviour
{
    /// This is a "virtual" function so it can be overridden by classes that inherit from the UIelement class
    public virtual void UpdateUI()
    {

    }
}
