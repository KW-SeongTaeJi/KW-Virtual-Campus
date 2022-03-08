using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;
    public static Managers Singleton 
    {
        get 
        {
            Init();
            return _instance; 
        } 
    }

    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneLoadManager _sceneLoad = new SceneLoadManager();
    WebManager _web = new WebManager();
    public static PoolManager Pool { get { return Singleton._pool; } }
    public static ResourceManager Resource { get { return Singleton._resource; } }
    public static SceneLoadManager SceneLoad { get { return Singleton._sceneLoad; } }
    public static WebManager Web { get { return Singleton._web; } }


    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    static void Init()
    {
        if (_instance == null)
        {
            GameObject gameObject = GameObject.Find("@Managers");
            if (gameObject == null)
            {
                gameObject = new GameObject { name = "@Managers" };
                gameObject.AddComponent<Managers>();
            }
            DontDestroyOnLoad(gameObject);
            _instance = gameObject.GetComponent<Managers>();

            _instance._pool.Init();
        }
    }

    public static void Clear()
    {
        SceneLoad.Clear();
        Pool.Clear();
    }
}
