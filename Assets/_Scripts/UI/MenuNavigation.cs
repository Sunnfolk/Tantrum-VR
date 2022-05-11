using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Wilberforce;

public class MenuNavigation : MonoBehaviour
{   
    public RadialMenu menu = null; //The script RadialMenu ADD IN EDITOR

    public bool isPaused; //Tells us if the game is paused

    [SerializeField] private int _activeMenu; //Tells which menu is active and is edited when changing menus to keep showing which menu is active 
    [SerializeField] private int _selectedOption; //Tells which option in the menu is currently being hovered over

    [Header("Display Color Active")]
    public List<int> colourSettingList = new List<int>(); //List of values that tells which colour setting is/gets set in PlayerPrefs

    public List<string> colourSettingNames = new List<string>(); //List of names to use when calling PlayerPrefs colour settings

    public string colourName1 = "Normal"; //Name used in the list colourSettingNames
    public string colourName2 = "Protanopia"; //Name used in the list colourSettingNames
    public string colourName3 = "Deuteranopia"; //Name used in the list colourSettingNames
    public string colourName4 = "Tritanopia"; //Name used in the list colourSettingNames

    [Header("Volume Slider Value")]
    public List<float> volumeList = new List<float>(); //List of values from/for PlayerPrefs volume settings

    public List<string> volNames = new List<string>(); //List of names to use when calling PlayerPrefs volume settings

    private string masterVolume = "MasterVolume"; //Name used in the list volNames
    private string musicVolume = "MusicVolume"; //Name used in the list volNames
    private string voicesVolume = "VoicesVolume"; //Name used in the list volNames
    private string ambienceVolume = "AmbienceVolume"; //Name used in the list volNames
    private string sfxVolume = "SFXVolume"; //Name used in the list volNames

    [Header("Arrays")]
    public GameObject[] inMenuNr; //Array of menuContainers ADD IN EDITOR

    private List<GameObject[]> optionsArrays = new List<GameObject[]>(); //List of options arrays arrays

    public GameObject[] pauseMenuOption; //Array of options in pause menu container ADD IN EDITOR
    public GameObject[] optionsMenuOption; //Array of options in options menu container ADD IN EDITOR
    public GameObject[] soundsMenuOption; //Array of options in sounds menu container ADD IN EDITOR
    public GameObject[] displayMenuOption; //Array of options in display menu container ADD IN EDITOR
    public GameObject[] soundSliders; //Array of sliders in soundSliders container ADD IN EDITOR
    public GameObject[] colourMenuOption; //Array of options in colour menu container ADD IN EDITOR

    public string[] scenesToLoad; //Array of scenes which can be loaded ADD MAIN MENU IN INSPECTOR AND ANY OTHERS YOU MAY WANT TO LOAD FROM PAUSE MENU


    [Header("Buttons")]
    private bool _topButtonPressed; //Is set to true the moment the button is pressed and is set to false when the code for that button has been run
    private bool _rightButtonPressed; //Is set to true the moment the button is pressed and is set to false when the code for that button has been run
    public static bool s_bottomButtonPressed; //Is set to true the moment the button is pressed and is set to false when the code for that button has been run
    private bool _leftButtonPressed; //Is set to true the moment the button is pressed and is set to false when the code for that button has been run

    private void Start()
    {
        isPaused = false;

        //REMEMBER TO SET ALL THE STUFF THAT NEEDS TO BE SET FOR THE GAME NOT TO BE PAUSED IF RESET HAS BEEN PRESSED

        #region Filling the lists

        //Adds arrays to optionsArrays
        optionsArrays.Add(pauseMenuOption);
        optionsArrays.Add(optionsMenuOption);
        optionsArrays.Add(soundsMenuOption);
        optionsArrays.Add(displayMenuOption);
        optionsArrays.Add(colourMenuOption);
        optionsArrays.Add(soundSliders);

        //Adds string to colourSettingNames
        colourSettingNames.Add(colourName1);
        colourSettingNames.Add(colourName2);
        colourSettingNames.Add(colourName3);
        colourSettingNames.Add(colourName4);

        //Adds string to volNames
        volNames.Add(masterVolume);
        volNames.Add(musicVolume);
        volNames.Add(voicesVolume);
        volNames.Add(ambienceVolume);
        volNames.Add(sfxVolume);


        for (int i = 0; i < volNames.Count; i++) //Adds floats to volumeList from PlayerPrefs with names from volNames
        {
            volumeList.Add(PlayerPrefs.GetFloat(volNames[i]));

        } //Adds floats to volumeList from PlayerPrefs with names from volNames

        for (int i = 0; i < colourSettingNames.Count; i++) //Adds ints to colourSettingList from PlayerPrefs with names from colourSettingNames
        {
            colourSettingList.Add(PlayerPrefs.GetInt(colourSettingNames[i]));

        } //Adds ints to colourSettingList from PlayerPrefs with names from colourSettingNames

        #endregion

        for (int i = 0; i < optionsArrays[5].Length; i++) //Corrects the sound slider values to what has been set in the PlayerPrefs
        {
            optionsArrays[5][i].GetComponentInChildren<Slider>().value = PlayerPrefs.GetFloat(volNames[i]);

        } //Corrects the sound slider values to what has been set in the PlayerPrefs

        for (int i = 0; i < colourSettingList.Count; i++) //Corrects the colour indication of which colour setting has been set in the PlayerPrefs
        {
            if (colourSettingList[i] == 0) //Colour indication for not selected
            {
                optionsArrays[4][i].GetComponent<Image>().color = new Color(255, 255, 255);
            } //Colour indication for not selected
            else //Colour indication for selected
            {
                Camera.main.GetComponent<Colorblind>().Type = i;
                optionsArrays[4][i].GetComponent<Image>().color = new Color(0, 255, 0);
                Camera.main.GetComponent<Colorblind>().Type = i; // Set the colourblind option

            } //Colour indication for selected

        } //Corrects the colour indication of which colour setting has been set in the PlayerPrefs

    }

    private void Update()
    {
        #region PlayerPrefs updates
        for (int i = 0; i < volumeList.Count; i++) //Sets all the PlayerPrefs for Volume according to the ints in the volume list
        {
            PlayerPrefs.SetFloat(volNames[i], volumeList[i]);

        } //Sets all the PlayerPrefs for Volume according to the ints in the volume list

        for (int i = 0; i < colourSettingList.Count; i++) //Sets all the PlayerPrefs for colour according to the ints in the colourSettingList
        {
            PlayerPrefs.SetInt(colourSettingNames[i], colourSettingList[i]);
            
        } //Sets all the PlayerPrefs for colour according to the ints in the colourSettingList
        #endregion
    }
    
    private void FixedUpdate()
    {  

        #region What happens when you press the top button
        if (_topButtonPressed)
        {
            if (isPaused) //What happens if the game is paused
            {
                if (_activeMenu == 0) //PauseMenu
                {
                    if (_selectedOption == 0) //Play
                    {
                        pause(); //Unpauses the game
                    } //Play
                    else if (_selectedOption == 1) //Options
                    {

                        next(1); //Runs the next() function and tells it how much to add to the _active menu int

                        resetMenuPosition(); //Resets the position of the options in the menu

                    } //Options
                    else if (_selectedOption == 2) //Reset
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reloads the Scene you are in
                    } //Reset
                    else if (_selectedOption == 3) //Exit
                    {
                        SceneManager.LoadScene(scenesToLoad[0]); //Loads the main menu
                    } //Exit

                } //PauseMenu
                else if (_activeMenu == 1) //OptionsMenu
                {
                    int maxlength = optionsArrays[_activeMenu].Length - 1; //Temporary int for the max length of the menu

                    if (_selectedOption < maxlength) //All options except for Back
                    {

                        next(_selectedOption + 1); //Runs the next() function and tells it how much to add to the _active menu int

                        resetMenuPosition(); //Resets the position of the options in the menu

                    } //All options except for Back
                    else //Back
                    {

                        back(1); //Runs the back() function and tells it how much to remove to the _active menu int

                        resetMenuPosition(); //Resets the position of the options in the menu

                    } //Back

                } //OptionsMenu
                else if (_activeMenu == 2) //SoundsMenu
                {
                    int maxlength = optionsArrays[_activeMenu].Length - 1;  //Temporary int for the max length of the menu

                    if (_selectedOption < maxlength) //All options except for Back
                    {

                        next(3);  //Runs the next() function and tells it how much to add to the _active menu int

                        optionsArrays[_activeMenu][_selectedOption].SetActive(true);

                    } //All options except for Back
                    else //Back
                    {

                        back(1); //Runs the back() function and tells it how much to remove to the _active menu int

                        resetMenuPosition(); //Resets the position of the options in the menu

                    } //Back
                } //SoundsMenu
                else if (_activeMenu == 3) //DisplayMenu
                {
                    int maxlength = optionsArrays[_activeMenu].Length - 1; //Temporary int for the max length of the menu

                    if (_selectedOption < maxlength) //All options except for Back
                    {

                        //next(_selectedOption + 1); //Runs the next() function and tells it how much to add to the _active menu int

                        //resetMenuPosition(); //Resets the position of the options in the menu

                    } //All options except for Back
                    else //Back
                    {

                        back(2); //Runs the back() function and tells it how much to remove to the _active menu int

                        resetMenuPosition(); //Resets the position of the options in the menu

                    } //Back
                } //DisplayMenu
                else if (_activeMenu == 4) //ColourMenu
                {
                    int maxlength = optionsArrays[_activeMenu].Length - 1; //Temporary int for the max length of the menu

                    if (_selectedOption < maxlength) //All options except for Back
                    {
                        for (int i = 0; i < colourSettingList.Count; i++) //Resets all buttons to standard colour
                        {
                            colourSettingList[i] = 0;
                            optionsArrays[_activeMenu][i].GetComponent<Image>().color = new Color(255, 255, 255);
                        } //Resets all buttons to standard colour

                        Camera.main.GetComponent<Colorblind>().Type = _selectedOption; // Set the colourblind option
                        

                        colourSettingList[_selectedOption] = 1; //Updates which colour setting is chosen

                        optionsArrays[_activeMenu][_selectedOption].GetComponent<Image>().color = new Color(0, 255, 0); //Sets the colour of the selected colour setting to be noticable
                        Debug.Log("IM SELECTING");

                    } //All options except for Back
                    else //Back
                    {

                        back(1); //Runs the back() function and tells it how much to remove to the _active menu int

                        resetMenuPosition(); //Resets the position of the options in the menu

                    } //Back
                } //ColourMenu

            } //What happens if the game is paused
            else if (MenuInputManager.s_deviceIsUp && !isPaused) //What happens when you are not in paused menu
            {

                FMODPlaylist.PausePlay();

            } //What happens when you are not in paused menu

            _topButtonPressed = false;
        }
        #endregion

        #region What happens when you press the right button
        if (_rightButtonPressed)
        {
            if (isPaused && _activeMenu != 5) //In any pause menu exept the sound sliders
            {
                int MenuOptionsMax = optionsArrays[_activeMenu].Length - 1; //Temporary int for the max length of the menu

                if (optionsArrays[_activeMenu][MenuOptionsMax].GetComponent<RectTransform>().anchoredPosition.x > 0) //Runs if you are not in the last option in the menu
                {
                    for (int i = 0; i < optionsArrays[_activeMenu].Length; i++) //Moves all the "buttons" on step to the left
                    {
                        optionsArrays[_activeMenu][i].GetComponent<RectTransform>().anchoredPosition = new Vector2((float)Math.Round((optionsArrays[_activeMenu][i].GetComponent<RectTransform>().anchoredPosition.x - 0.27f), 2), optionsArrays[_activeMenu][i].GetComponent<RectTransform>().anchoredPosition.y);
                    } //Moves all the "buttons" on step to the left

                    _selectedOption += 1; //Changes which option is currently selected
                } //Runs if you are not in the last option in the menu

            } //In any pause menu exept the sound sliders
            else if (isPaused && _activeMenu == 5) //In Sound Slider
            {
                if (optionsArrays[5][_selectedOption].GetComponentInChildren<Slider>().value < 1) //Will increase volume unless volume is at max
                {
                    optionsArrays[5][_selectedOption].GetComponentInChildren<Slider>().value += 0.05f;

                } //Will increase volume unless volume is at max

                volumeList[_selectedOption] = optionsArrays[5][_selectedOption].GetComponentInChildren<Slider>().value; //Updates the int to the correct value

            } //In Sound Slider
            else if (MenuInputManager.s_deviceIsUp && !isPaused) //What happens when you are not in paused menu
            {
                FMODPlaylist.LastNext(true);

            } //What happens when you are not in paused menu

            _rightButtonPressed = false;
        }
        #endregion

        #region What happens when you press the bottom button
        if (s_bottomButtonPressed)
        {
            if (isPaused)
            {
                if (_activeMenu == 0) //PauseMenu
                {

                    pause(); //Unpauses the game

                } //PauseMenu
                else if (_activeMenu == 1 || _activeMenu == 2 || _activeMenu == 4) //OptionsMenu, SoundsMenu, and ColourMenu
                {

                    back(1); //Runs the back() function and tells it how much to remove to the _active menu int

                    resetMenuPosition(); //Resets the position of the options in the menu

                } //OptionsMenu, SoundsMenu, and ColourMenu
                else if (_activeMenu == 3) //DisplayMenu
                {

                    back(2); //Runs the back() function and tells it how much to remove to the _active menu int

                    resetMenuPosition(); //Resets the position of the options in the menu

                } //DisplayMenu
                else if (_activeMenu == 5) //SoundSlider
                {

                    back(3); //Runs the back() function and tells it how much to remove to the _active menu int

                    for (int i = 0; i < optionsArrays[5].Length; i++) //Sets all the sound sliders to inactive
                    {
                        optionsArrays[5][i].SetActive(false);
                    } //Sets all the sound sliders to inactive

                } //SoundSlider
            }
            else
            {
                menu.Show(false);
                MenuInputManager.s_deviceIsUp = false;
            }


            s_bottomButtonPressed = false;
        }
        #endregion

        #region What happens when you press the left button
        if (_leftButtonPressed)
        {
            if (isPaused && _activeMenu != 5) //In any pause menu exept the sound sliders
            {

                if (optionsArrays[_activeMenu][0].GetComponent<RectTransform>().anchoredPosition.x < 0) //Runs if you are not in the first option in the menu
                {
                    for (int i = 0; i < optionsArrays[_activeMenu].Length; i++) //Moves all the "buttons" on step to the left
                    {
                        optionsArrays[_activeMenu][i].GetComponent<RectTransform>().anchoredPosition = new Vector2((float)Math.Round((optionsArrays[_activeMenu][i].GetComponent<RectTransform>().anchoredPosition.x + 0.27f), 2), optionsArrays[_activeMenu][i].GetComponent<RectTransform>().anchoredPosition.y);
                    } //Moves all the "buttons" on step to the left

                    _selectedOption -= 1; //Changes which option is currently selected

                } //Runs if you are not in the first option in the menu

            } //In any pause menu exept the sound sliders
            else if (isPaused && _activeMenu == 5) //In Sound Slider
            {
                if (optionsArrays[5][_selectedOption].GetComponentInChildren<Slider>().value > 0)  //Will decrease volume unless volume is at min
                {
                    optionsArrays[5][_selectedOption].GetComponentInChildren<Slider>().value -= 0.05f;

                }  //Will decrease volume unless volume is at min

                volumeList[_selectedOption] = optionsArrays[5][_selectedOption].GetComponentInChildren<Slider>().value; //Updates the int to the correct value

            } //In Sound Slider
            else if (MenuInputManager.s_deviceIsUp && !isPaused) //What happens when you are not in paused menu
            {
                FMODPlaylist.LastNext(false);

            } //What happens when you are not in paused menu

            _leftButtonPressed = false;
        }
        #endregion
    }

    public void pause() //What happens when you press the menu button
    {        
        //REMEMBER TO TURN OFF WORLD INTERACTION AND TIMEFLOW

        if (isPaused && MenuInputManager.s_deviceIsUp)
        {
            for (int i = 0; i < inMenuNr.Length; i++)
            {
                inMenuNr[i].SetActive(false);
            }
            for (int i = 0; i < soundSliders.Length; i++)
            {
                soundSliders[i].SetActive(false);
            }
            _activeMenu = 0;
            isPaused = false;
            resetMenuPosition();

        }
        else if (!isPaused && MenuInputManager.s_deviceIsUp)
        {
            inMenuNr[_activeMenu].SetActive(true);
            isPaused = true;
            menu.Show(true);
        }
    }

    public void next(int toAddToActiveMenuInt)
    {
        _activeMenu += toAddToActiveMenuInt; //Sets the _activeMenu int to the newly selected menu

        for (int i = 0; i < inMenuNr.Length; i++) //Sets all menus to inactive
        {
            inMenuNr[i].SetActive(false);

        } //Sets all menus to inactive

        inMenuNr[_activeMenu].SetActive(true); //Sets the newly selected menu to active
    }

    public void back(int toRemoveFromActiveMenuInt) //what happens if you press the back option
    {
        _activeMenu -= toRemoveFromActiveMenuInt; //Sets the _activeMenu to the previous menu

        for (int i = 0; i < inMenuNr.Length; i++) //Sets all menus to inactive
        {
            inMenuNr[i].SetActive(false);

        } //Sets all menus to inactive

        inMenuNr[_activeMenu].SetActive(true); //Sets the previous selected menu to active
    }

    #region Button input voids
    public void navTop() //The action for the top button of the vive touchpad which is called from the menu object in hierarchy
    {
        if (!_topButtonPressed)
        {
            _topButtonPressed = true;
        }
    }

    public void navRight() //The action for the right button of the vive touchpad which is called from the menu object in hierarchy
    {
        if (!_rightButtonPressed)
        {
            _rightButtonPressed = true;
        }
    }

    public void navBottom() //The action for the bottom button of the vive touchpad which is called from the menu object in hierarchy
    {
        if (!s_bottomButtonPressed)
        {
            s_bottomButtonPressed = true;
        }
    }

    public void navLeft() //The action for the left button of the vive touchpad which is called from the menu object in hierarchy
    {
        if (!_leftButtonPressed)
        {
            _leftButtonPressed = true;
        }
    }
    #endregion

    public void resetMenuPosition() //Reset position of MenuOptions
    {     
        for (int i = 0; i < optionsArrays.Count - 1; i++) //Goes through the list of arrays
        {
            for (int a = 0; a < optionsArrays[i].Length; a++) //Goes through the arrays in the list of arrays and sets their position
            {
                optionsArrays[i][0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, optionsArrays[i][0].GetComponent<RectTransform>().anchoredPosition.y);

                if (a > 0) //Sets the position of all options except the first in accordance to the first options position
                {
                    optionsArrays[i][a].GetComponent<RectTransform>().anchoredPosition = new Vector2(optionsArrays[i][a - 1].GetComponent<RectTransform>().anchoredPosition.x + 0.27f, optionsArrays[i][0].GetComponent<RectTransform>().anchoredPosition.y);
                } //Sets the position of all options except the first in accordance to the first options position

            } //Goes through the arrays in the list of arrays and sets their position

        } //Goes through the list of arrays

        for (int i = 0; i < optionsArrays[5].Length; i++) //Corrects the position of the Sound Sliders
        {
            optionsArrays[5][i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, optionsArrays[5][0].GetComponent<RectTransform>().anchoredPosition.y);
        } //Corrects the position of the Sound Sliders

        _selectedOption = 0; //Resets the _selectedOption int
    }
}