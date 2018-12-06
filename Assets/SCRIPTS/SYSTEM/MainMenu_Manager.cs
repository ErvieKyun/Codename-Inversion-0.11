using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_Manager: MonoBehaviour
{
    enum MenuTracker { Main, Start, Options, Controls, Leaderboard, Credits };     // Tracker for where which screen the Player is.
    MenuTracker _menuLocation;                                                     // Class-wide paramater statement for the enum to work.

    [Header("Insert Canvases Here")]
    [Tooltip("Click and drag the canvas objects from heirarchy here.")]
    public Canvas MainScreen;           // Main Menu Handler
    //public Canvas StartGameScreen;      // Game Menu Handler
    public Canvas OptionsScreen;        // Options Menu Handler
    //public Canvas ControlsScreen;       // Controls Screen Handler
    //public Canvas Leaderboard;          // Leaderboard Handler
    //public Canvas CreditsScreen;        // Credits Screen Handler
    public Canvas ConfirmScreen;        // Confirmation Screen Handler

    /*
    [Header("Options Sliders")]         // Slider controllers.
    [Tooltip("Click and drag the slider objects from hierarchy here.")]
    public Slider FieldOfViewSlider;    
    public Slider SensitivitySlider;    
    public Slider MusicSlider;          
    public Slider SoundSlider;          
    public Slider DifficultySlider;     

    [Header("Slider Text Fields")]      // Feedback Handler for Sliders
    [Tooltip("Click and drag the slider text objects from hierarchy here.")]
    public Text FieldOfViewText;      
    public Text SensitivityText;      
    public Text MusicText;            
    public Text SoundText;            
    public Text DifficultyText;       
    */

    public void Start()
    {
        MenuDefault();               // Default functionality.

        // TYPE IN CUSTOM STUFF HERE
    }

    public void MenuDefault()           // Default settings. Any menu testing can be changed in void Start().
    {
        _menuLocation = MenuTracker.Main;

        MainScreen.enabled = true;
        //StartGameScreen.enabled = false;
        OptionsScreen.enabled = false;
        //ControlsScreen.enabled = false;
        //Leaderboard.enabled = false;
        //CreditsScreen.enabled = false;
        ConfirmScreen.enabled = false;
    }

    public void MenuOptions()           // Menu Switch for Main Menu to Options
    {
        MainScreen.enabled = false;
        OptionsScreen.enabled = true;
        
        _menuLocation = MenuTracker.Options;
    }

    /* public void MenuControls()          // Menu Switch for Options to Controls
    {
        OptionsScreen.enabled = false;
        ControlsScreen.enabled = true;

        _menuLocation = MenuTracker.Controls;
    }*/

    /*public void MenuLeaderboard()       // Menu Switch for Main Menu to Leaderboard
    {
        MainScreen.enabled = false;
        OptionsScreen.enabled = true;

        _menuLocation = MenuTracker.Leaderboard;

    }*/

    /* public void MenuCredits()           // Menu Switch for Main Menu to Credits
    {
        MainScreen.enabled = false;
        CreditsScreen.enabled = true;

        _menuLocation = MenuTracker.Credits;

    }*/

    public void MenuConfirmScreen()     // Menu Switch for Main Menu to Confirm Screen
    {
        ConfirmScreen.enabled = true;
        MainScreen.enabled = false;
    }

    public void BackController()        // Context Sensitive Back Function for Menus.
    {
        switch (_menuLocation)             // MenuTracker.Main, .Start, .Options, .Controls, .Leaderboard, .Credits
        {
            case MenuTracker.Main:
                #if UNITY_EDITOR
                                UnityEditor.EditorApplication.isPlaying = false;
                #else
                        Application.Quit();
                #endif
                break;

            case MenuTracker.Start:
                MenuDefault();
                break;

            case MenuTracker.Options:
                MenuDefault();
                break;

            case MenuTracker.Controls:
                MenuOptions();
                break;

            case MenuTracker.Leaderboard:
                MenuDefault();
                break;

            case MenuTracker.Credits:
                MenuDefault();
                break;

            default:
                MenuDefault();
                break;
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}