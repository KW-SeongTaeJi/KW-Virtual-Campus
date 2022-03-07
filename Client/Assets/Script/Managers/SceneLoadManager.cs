using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }


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
