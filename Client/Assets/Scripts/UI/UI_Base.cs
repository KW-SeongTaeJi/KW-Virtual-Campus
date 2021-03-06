using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();


    private void Awake()
    {
        Init();
    }

    public abstract void Init();

    // T: unity object to bind
    // type: Enum which contains names of bind object 
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Error : Fail to bind ({names[i]})");
        }

        _objects.Add(typeof(T), objects);
    }

    // idx : Enum value which wants to get
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TMP_InputField GetInputField(int idx) { return Get<TMP_InputField>(idx); }
    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected TMP_Dropdown GetDropdown(int idx) { return Get<TMP_Dropdown>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected Slider GetSlider(int idx) { return Get<Slider>(idx); }
    protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }

    public static void BindEvent(GameObject gameObject, Action<PointerEventData> action, Define.UIEvent type)
    {
        UI_EventHandler eventHandler = Util.GetOrAddComponent<UI_EventHandler>(gameObject);

        switch (type)
        {
            case Define.UIEvent.Click:
                eventHandler.OnClickHandler -= action;
                eventHandler.OnClickHandler += action;
                break;
            case Define.UIEvent.Enter:
                eventHandler.OnEnterHandler -= action;
                eventHandler.OnEnterHandler += action;
                break;
            case Define.UIEvent.Exit:
                eventHandler.OnExitHandler -= action;
                eventHandler.OnExitHandler += action;
                break;
            default:
                Debug.Log("Error : Undefined UI eventType");
                break;
        }
    }
}
