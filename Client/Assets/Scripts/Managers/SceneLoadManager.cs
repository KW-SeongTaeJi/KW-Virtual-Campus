using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public List<Resolution> Resolutions { get; set; } = new List<Resolution>();

    
    public void Init()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
                Resolutions.Add(Screen.resolutions[i]);
        }
    }

    public void LoadScene(Define.Scene sceneType)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(sceneType));
    }

    public void LoadSceneAsync(Define.Scene sceneType, int channel = 1)
    {
        Managers.Clear();
        UI_LoadScene.NextScene = GetSceneName(sceneType);
        UI_LoadScene.GameChannel = channel;
        SceneManager.LoadScene("Load");
    }

    string GetSceneName(Define.Scene sceneType)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), sceneType);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
