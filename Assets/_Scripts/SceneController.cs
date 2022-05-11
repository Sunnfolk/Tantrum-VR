using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class SceneController : ScriptableObject
{
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void DeactivateCurrentMenu(GameObject currentMenu)
    {
        currentMenu.SetActive(false);
    }

    public void ChangeMenu(GameObject newMenu)
    {
        newMenu.SetActive(true);
    }

    public void AddScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#else
        Application.Quit();
#endif
    }
}
