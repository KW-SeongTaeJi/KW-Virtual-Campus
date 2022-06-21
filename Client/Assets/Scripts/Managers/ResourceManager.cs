using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    /* "Asset/Resources" ������ ���ҽ� �ε� */
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject gameObject = Managers.Pool.GetOriginal(name);
            if (gameObject != null)
                return gameObject as T;
        }

        return Resources.Load<T>(path);
    }

    /* "Asset/Resources/Prefabs" ������ ������ ���� */
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Error : Failed to load prefab at {path}");
            return null;
        }

        // Poolable ������Ʈ ����
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // Non-Poolable ������Ʈ ����
        GameObject gameObject = Object.Instantiate(original, parent);
        gameObject.name = original.name;
        return gameObject;
    }
    public GameObject Instantiate(string path, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Error : Failed to load prefab at {path}");
            return null;
        }

        // Poolable ������Ʈ ����
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        // Non-Poolable ������Ʈ ����
        GameObject gameObject = Object.Instantiate(original, pos, rot, parent);
        gameObject.name = original.name;
        return gameObject;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
            return;

        // Poolable ������Ʈ ����
        Poolable poolable = gameObject.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        // Non-Poolable ������Ʈ ����
        Object.Destroy(gameObject);
    }
}
