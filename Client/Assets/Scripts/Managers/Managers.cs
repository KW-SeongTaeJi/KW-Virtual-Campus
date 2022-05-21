using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;
    public static Managers Instance 
    {
        get 
        {
            Init();
            return _instance; 
        } 
    }

    NetworkManager _network = new NetworkManager();
    ObjectManager _obj = new ObjectManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneLoadManager _sceneLoad = new SceneLoadManager();
    UIManager _ui = new UIManager();
    WebManager _web = new WebManager();
    public static NetworkManager Network { get { return Instance._network; } }
    public static ObjectManager Object { get { return Instance._obj; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneLoadManager SceneLoad { get { return Instance._sceneLoad; } }
    public static UIManager UI { get { return Instance._ui; } } 
    public static WebManager Web { get { return Instance._web; } }


    void Start()
    {
        Init();
    }

    void Update()
    {
        _network.Update();
        _sceneLoad.Update();
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
            _instance._sceneLoad.Init();
        }
    }

    public static void Clear()
    {
        SceneLoad.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
