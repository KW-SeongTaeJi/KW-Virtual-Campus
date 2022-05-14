using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    // one current Scene UI 
    public UI_Scene SceneUI { get; private set; }

    // can have many ordered Popup UI 
    public Stack<UI_Popup> PopupUIStack { get; set; } = new Stack<UI_Popup>();
    int _popupOrder = 10;


    public void SetCanvas(GameObject gameObject, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(gameObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        if (sort)
            canvas.sortingOrder = _popupOrder++;
        else
            canvas.sortingOrder = 0;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject gameObject = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(gameObject);
        SceneUI = sceneUI;
        gameObject.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject gameObject = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popupUI = Util.GetOrAddComponent<T>(gameObject);
        PopupUIStack.Push(popupUI);
        gameObject.transform.SetParent(Root.transform);

        return popupUI;
    }

    public void ClosePopupUI()
    {
        if (PopupUIStack.Count == 0)
            return;

        UI_Popup popup = PopupUIStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _popupOrder--;
    }

    public void CloseAllPopupUI()
    {
        while (PopupUIStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        SceneUI = null;
        CloseAllPopupUI();
    }
}
