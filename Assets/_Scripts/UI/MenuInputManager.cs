using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;


public class MenuInputManager : MonoBehaviour
{
    //This script creates custom actions to map to your controller
    // https://www.youtube.com/watch?v=CPyaWkjo6Ss is the tutorial i used to set up this script

    [Header("Actions")]
    public SteamVR_Action_Boolean touch = null;
    public SteamVR_Action_Boolean press = null;
    public SteamVR_Action_Vector2 touchPosition = null;

    public SteamVR_Action_Boolean menuButton = null;

    public static bool s_deviceIsUp = false;

    [Header("Scene Objects")]
    public RadialMenu menu = null;
    public MenuNavigation pauseMenu = null;
    public GameObject menuContainer = null;
    public GameObject[] controller;

    private void Start()
    {
        s_deviceIsUp = false;

        touch.AddOnStateDownListener(Touch, SteamVR_Input_Sources.RightHand);
        touch.AddOnStateDownListener(Touch, SteamVR_Input_Sources.LeftHand);
        //touch.onChange += Touch;
        press.onStateDown += PressRelease;
        touchPosition.onAxis += Position;

        menuButton.onStateDown += MenuPressRelease;
    }

    private void OnDestroy()
    {
        touch.RemoveOnStateDownListener(Touch, SteamVR_Input_Sources.RightHand);
        touch.RemoveOnStateDownListener(Touch, SteamVR_Input_Sources.LeftHand);

        press.onStateDown -= PressRelease;
        touchPosition.onAxis -= Position;

        menuButton.onStateDown -= MenuPressRelease;
    }

    private void Position(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        menu.SetTouchPosition(axis);
    }

    private void Touch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!s_deviceIsUp)
        {
            s_deviceIsUp = true;
            if (pauseMenu.isPaused == false)
            {
                menu.Show(true);
            }

            if (fromSource == SteamVR_Input_Sources.LeftHand && SceneManager.GetActiveScene().name != "Street")
            {
                if (controller[0] == null)
                    return;

                menuContainer.transform.parent = controller[0].transform;
                menuContainer.transform.localPosition = new Vector3(0.03f, 0.01f, -0.05f);
                menuContainer.transform.localRotation = Quaternion.Euler(85, 0, 0);

            }
            else if (fromSource == SteamVR_Input_Sources.RightHand || SceneManager.GetActiveScene().name == "Street")
            {
                if (controller[1] == null)
                    return;

                menuContainer.transform.parent = controller[1].transform;
                menuContainer.transform.localPosition = new Vector3(-0.03f, 0.01f, -0.05f);
                menuContainer.transform.localRotation = Quaternion.Euler(85, 0, 0);

            }
        }
        else if (s_deviceIsUp && pauseMenu.isPaused)
        {
            pauseMenu.pause();
            menu.Show(false);
            s_deviceIsUp = false;
        }
        else if (s_deviceIsUp && !pauseMenu.isPaused)
        {
            pauseMenu.pause();
        }
    }

    private void PressRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        menu.ActivateHighlightedSection();
    }

    private void MenuPressRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        //if (!s_deviceIsUp)
        //{
        //    pauseMenu.pause();
        //}
    }
}
