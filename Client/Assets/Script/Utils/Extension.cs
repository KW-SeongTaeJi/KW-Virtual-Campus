using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
	public static T GetOrAddComponent<T>(this GameObject gameObject) where T : UnityEngine.Component
	{
		return Util.GetOrAddComponent<T>(gameObject);
	}
}
