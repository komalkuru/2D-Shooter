using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

/// <summary>
/// A class which manages pages of UI elements
/// and the game's UI
/// </summary>

public class UIManager : MonoBehaviour
{

    [Header("Page Management")]
    [Tooltip("The pages (Panels) managed by the UI Manager")]
    public List<UIPage> pages;
    [Tooltip("The index of the active page in the UI")]
    public int currentPage = 0;
    [Tooltip("The page (by index) switched to when the UI Manager starts up")]
    public int defaultPage = 0;

    [Header("Pause Settings")]
    [Tooltip("The index of the pause page in the pages list")]
    public int pausePageIndex = 1;
    [Tooltip("Whether or not to allow pausing")]
    public bool allowPause = true;

    // Whether or not the application is paused
    private bool isPaused = false;

    // A list of all UI element classes
    private List<UIelement> UIelements;

    // The event system handling UI navigation
    [HideInInspector]
    public EventSystem eventSystem;
    // The Input Manager to listen for pausing
    [SerializeField]
    private InputManager inputManager;

    //Standard Unity function called whenever the attached game object is enabled
 
    //When this component wakes up (including switching scenes) it sets itself as the GameManager's UI manager
    private void OnEnable()
    {
        SetupGameManagerUIManager();
    }

    //Sets this component as the UI manager for the GameManagern)
    private void SetupGameManagerUIManager()
    {
        if (GameManager.instance != null && GameManager.instance.uiManager == null)
        {
            GameManager.instance.uiManager = this;
        }     
    }

    //Finds and stores all UIElements in the UIElements list
    private void SetUpUIElements()
    {
        UIelements = FindObjectsOfType<UIelement>().ToList();
    }

    //Gets the event system from the scene if one exists
    //If one does not exist a warning will be displayed
    private void SetUpEventSystem()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogWarning("There is no event system in the scene but you are trying to use the UIManager. /n" +
                "All UI in Unity requires an Event System to run. /n" + 
                "You can add one by right clicking in hierarchy then selecting UI->EventSystem.");
        }
    }

    //Attempts to set up an input manager with this UI manager so it can get pause input
    private void SetUpInputManager()
    {
        if (inputManager == null)
        {
            inputManager = InputManager.instance;
        }
        if (inputManager == null)
        {
            Debug.LogWarning("The UIManager can not find an Input Manager in the scene, without an Input Manager the UI can not pause");
        }
    }

    //If the game is paused, unpauses the game.
    //If the game is not paused, pauses the game.
    public void TogglePause()
    {
        if (allowPause)
        {
            if (isPaused)
            {
                SetActiveAllPages(false);
                Time.timeScale = 1;
                isPaused = false;
            }
            else
            {
                GoToPage(pausePageIndex);
                Time.timeScale = 0;
                isPaused = true;
            }
        }      
    }

    //Goes through all UI elements and calls their UpdateUI function
    public void UpdateUI()
    {
        SetUpUIElements();
        foreach (UIelement uiElement in UIelements)
        {
            uiElement.UpdateUI();
        }
    }

    //Default Unity function that runs once when the script is first started and before Update
    private void Start()
    {
        SetUpInputManager();
        SetUpEventSystem();
        SetUpUIElements();
        UpdateUI();
    }

    //Default function from Unity that runs every frame
    private void Update()
    {
        CheckPauseInput();
    }

    //If the input manager is set up, reads the pause input
    private void CheckPauseInput()
    {
        if (inputManager != null)
        {
            if (inputManager.pausePressed)
            {
                TogglePause();
            }
        }
    }
    //Goes to a page by that page's index
    public void GoToPage(int pageIndex)
    {
        if (pageIndex < pages.Count && pages[pageIndex] != null)
        {
            SetActiveAllPages(false);
            pages[pageIndex].gameObject.SetActive(true);
            pages[pageIndex].SetSelectedUIToDefault();
        }
    }

    //Goes to a page by that page's name
    public void GoToPageByName(string pageName)
    {
        UIPage page = pages.Find(item => item.name == pageName);
        int pageIndex = pages.IndexOf(page);
        GoToPage(pageIndex);
    }

    //Turns all stored pages on or off depending on parameters
    public void SetActiveAllPages(bool activated)
    {
        if (pages != null)
        {
            foreach (UIPage page in pages)
            {
                if (page != null)
                    page.gameObject.SetActive(activated);
            }
        }
    }
}
