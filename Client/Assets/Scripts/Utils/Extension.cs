using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
	public static T GetOrAddComponent<T>(this GameObject gameObject) where T : UnityEngine.Component
	{
		return Util.GetOrAddComponent<T>(gameObject);
	}

	public static T FindChild<T>(this GameObject gameObject, string name = null, bool recursive = false) where T : UnityEngine.Object
	{
		return Util.FindChild<T>(gameObject, name, recursive);
	}

	public static GameObject FindChild(this GameObject gameObject, string name = null, bool recursive = false)
    {
		return Util.FindChild(gameObject, name, recursive);
    } 

	public static void BindEvent(this GameObject gameObject, Action<PointerEventData> action, Define.UIEvent type)
    {
		UI_Base.BindEvent(gameObject, action, type);
    }
}
