using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wilberforce;

public class MainMenuVolumeSlider : MonoBehaviour
{
    [Header("Volume Slider Value")]
    public List<string> volNames = new List<string>(); //List of names to use when calling PlayerPrefs volume settings

    private string masterVolume = "MasterVolume"; //Name used in the list volNames
    private string musicVolume = "MusicVolume"; //Name used in the list volNames
    private string voicesVolume = "VoicesVolume"; //Name used in the list volNames
    private string ambienceVolume = "AmbienceVolume"; //Name used in the list volNames
    private string sfxVolume = "SFXVolume"; //Name used in the list volNames

    public GameObject[] soundSliders; //Array of sliders in soundSliders container ADD IN EDITOR

    [Header("Display Color Active")]
    public List<int> colourSettingList = new List<int>(); //List of values that tells which colour setting is/gets set in PlayerPrefs

    public List<string> colourSettingNames = new List<string>(); //List of names to use when calling PlayerPrefs colour settings

    public string colourName1 = "Normal"; //Name used in the list colourSettingNames
    public string colourName2 = "Protanopia"; //Name used in the list colourSettingNames
    public string colourName3 = "Deuteranopia"; //Name used in the list colourSettingNames
    public string colourName4 = "Tritanopia"; //Name used in the list colourSettingNames

    public GameObject[] displayMenuOption; //Array of options in display menu container ADD IN EDITOR

    public GameObject[] languageButtons;


    // Start is called before the first frame update
    void Start()
    {
        //Adds string to volNames
        volNames.Add(masterVolume);
        volNames.Add(musicVolume);
        volNames.Add(voicesVolume);
        volNames.Add(ambienceVolume);
        volNames.Add(sfxVolume);

        //Adds string to colourSettingNames
        colourSettingNames.Add(colourName1);
        colourSettingNames.Add(colourName2);
        colourSettingNames.Add(colourName3);
        colourSettingNames.Add(colourName4);

        for (int i = 0; i < soundSliders.Length; i++) //Corrects the sound slider values to what has been set in the PlayerPrefs
        {
            soundSliders[i].GetComponentInChildren<Slider>().value = PlayerPrefs.GetFloat(volNames[i]);

        } //Corrects the sound slider values to what has been set in the PlayerPrefs

        for (int i = 0; i < colourSettingNames.Count; i++) //Adds ints to colourSettingList from PlayerPrefs with names from colourSettingNames
        {
            colourSettingList.Add(PlayerPrefs.GetInt(colourSettingNames[i]));

        } //Adds ints to colourSettingList from PlayerPrefs with names from colourSettingNames

        for (int i = 0; i < colourSettingList.Count; i++) //Corrects the colour indication of which colour setting has been set in the PlayerPrefs
        {
            if (colourSettingList[i] == 0) //Colour indication for not selected
            {
                ColorBlock colors = displayMenuOption[i].GetComponent<Button>().colors;
                colors.normalColor = new Color(255, 255, 255);
                displayMenuOption[i].GetComponent<Button>().colors = colors;
            } //Colour indication for not selected
            else //Colour indication for selected
            {
                Camera.main.GetComponent<Colorblind>().Type = i;
                ColorBlock color2 = displayMenuOption[i].GetComponent<Button>().colors;
                color2.normalColor = new Color(0, 255, 0);
                displayMenuOption[i].GetComponent<Button>().colors = color2;

            } //Colour indication for selected

        } //Corrects the colour indication of which colour setting has been set in the PlayerPrefs

        if (PlayerPrefs.GetInt("English") == 1 && PlayerPrefs.GetInt("Norwegian") == 0)
        {
            ColorBlock color2 = languageButtons[0].GetComponent<Button>().colors;
            color2.normalColor = new Color(0, 255, 0);
            languageButtons[0].GetComponent<Button>().colors = color2;

            //ColorBlock colors = languageButtons[1].GetComponent<Button>().colors;
            //colors.normalColor = new Color(255, 255, 255);
            //languageButtons[1].GetComponent<Button>().colors = colors;
        }
        else if (PlayerPrefs.GetInt("Norwegian") == 1 && PlayerPrefs.GetInt("English") == 0)
        {
            ColorBlock color2 = languageButtons[1].GetComponent<Button>().colors;
            color2.normalColor = new Color(0, 255, 0);
            languageButtons[1].GetComponent<Button>().colors = color2;

            ColorBlock colors = languageButtons[0].GetComponent<Button>().colors;
            colors.normalColor = new Color(255, 255, 255);
            languageButtons[0].GetComponent<Button>().colors = colors;
        }
    }

    private void Update()
    {
        for (int i = 0; i < soundSliders.Length; i++) //Corrects the sound slider values to what has been set in the PlayerPrefs
        {
            PlayerPrefs.SetFloat(volNames[i], soundSliders[i].GetComponentInChildren<Slider>().value);

        } //Corrects the sound slider values to what has been set in the PlayerPrefs
    }

    public void setColour(int _selectedOption)
    {
        for (int i = 0; i < colourSettingList.Count; i++) //Resets all buttons to standard colour
        {
            colourSettingList[i] = 0;
            ColorBlock colors = displayMenuOption[i].GetComponent<Button>().colors;
            colors.normalColor = new Color(255, 255, 255);
            displayMenuOption[i].GetComponent<Button>().colors = colors;

        } //Resets all buttons to standard colour

        Camera.main.GetComponent<Colorblind>().Type = _selectedOption; // Set the colourblind option

        colourSettingList[_selectedOption] = 1; //Updates which colour setting is chosen

        ColorBlock color2 = displayMenuOption[_selectedOption].GetComponent<Button>().colors;
        color2.normalColor = new Color(0, 255, 0);
        displayMenuOption[_selectedOption].GetComponent<Button>().colors = color2;

        for (int i = 0; i < colourSettingList.Count; i++) //Sets all the PlayerPrefs for colour according to the ints in the colourSettingList
        {
            PlayerPrefs.SetInt(colourSettingNames[i], colourSettingList[i]);

        } //Sets all the PlayerPrefs for colour according to the ints in the colourSettingList
    }

    public void setLanguage(int _selectedOption)
    {
        if (_selectedOption == 0)
        {
            PlayerPrefs.SetInt("Norwegian", 0);
            PlayerPrefs.SetInt("English", 1);

            ColorBlock color2 = languageButtons[0].GetComponent<Button>().colors;
            color2.normalColor = new Color(0, 255, 0);
            languageButtons[0].GetComponent<Button>().colors = color2;

            //ColorBlock colors = languageButtons[1].GetComponent<Button>().colors;
            //colors.normalColor = new Color(255, 255, 255);
            //languageButtons[1].GetComponent<Button>().colors = colors;
        }
        else if (_selectedOption == 1)
        {
            PlayerPrefs.SetInt("Norwegian", 1);
            PlayerPrefs.SetInt("English", 0);

            ColorBlock color2 = languageButtons[1].GetComponent<Button>().colors;
            color2.normalColor = new Color(0, 255, 0);
            languageButtons[1].GetComponent<Button>().colors = color2;

            ColorBlock colors = languageButtons[0].GetComponent<Button>().colors;
            colors.normalColor = new Color(255, 255, 255);
            languageButtons[0].GetComponent<Button>().colors = colors;
        }
    }
}
