using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public List<Resolution> Resolutions { get; set; } = new List<Resolution>();

    FullScreenMode _currentMode;

    
    public void Init()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
                Resolutions.Add(Screen.resolutions[i]);
        }
        _currentMode = Screen.fullScreenMode;
    }

    public void Update()
    {
        if (_currentMode.Equals(Screen.fullScreen) == false)
            _currentMode = Screen.fullScreenMode;
    }

    public void LoadScene(Define.Scene sceneType)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(sceneType));
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
