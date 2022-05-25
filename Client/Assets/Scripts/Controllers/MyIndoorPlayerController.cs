using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyIndoorPlayerController : IndoorPlayerController
{
    [SerializeField, Tooltip("Camera move speed")]
    float cameraSpeed = 5.0f;

    float _lastMoveX = 0.0f;

    float _cameraWidth;
    GameObject _mainCamera;

    MyIndoorPlayerInput _input;


    void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");   
    }

    protected override void Init()
    {
        base.Init();

        _cameraWidth = _mainCamera.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height;

        BaseScene currentScene = Managers.SceneLoad.CurrentScene;
        switch (currentScene.SceneType)
        {
            case Define.Scene.IndoorBima:
                _input = ((IndoorScene_Bima)currentScene).GetComponent<MyIndoorPlayerInput>();
                break;
            case Define.Scene.IndoorHanwool:
                _input = ((IndoorScene_Hanwool)currentScene).GetComponent<MyIndoorPlayerInput>();
                break;
            case Define.Scene.IndoorHwado:
                _input = ((IndoorScene_Hwado)currentScene).GetComponent<MyIndoorPlayerInput>();
                break;
            case Define.Scene.IndoorLibrary:
                _input = ((IndoorScene_Library)currentScene).GetComponent<MyIndoorPlayerInput>();
                break;
            case Define.Scene.IndoorOgui:
                _input = ((IndoorScene_Ogui)currentScene).GetComponent<MyIndoorPlayerInput>();
                break;
            case Define.Scene.IndoorSaebit:
                _input = ((IndoorScene_Saebit)currentScene).GetComponent<MyIndoorPlayerInput>();
                break;
        }
    }

    protected override void OnUpdate()
    {
        Move();
        CheckChanges();
    }

    protected override void OnLateUpdate()
    {
        CameraMove();
    }

    protected override void Move()
    {
        if (_input.Move.x == 0)
        {
            _isWalk = false;
        }
        else
        {
            transform.localScale = new Vector3(_input.Move.x * 0.8f, 0.8f, 0.8f);
            _canvas.transform.localScale = new Vector3(_input.Move.x * 0.5f, 0.5f, 0.5f);
            _isWalk = true;
        }

        float newPosX = Mathf.Clamp(transform.position.x + _input.Move.x * moveSpeed * Time.deltaTime, _leftEnd.transform.position.x, _rightEnd.transform.position.x);

        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);

        _animator.SetBool("isWalk", _isWalk);
    }

    void CheckChanges()
    {
        // If movement chage, send packet to server
        if (_lastMoveX != _input.Move.x)
        {
            C_MoveIndoor moveIndoorPacket = new C_MoveIndoor()
            {
                PosX = transform.position.x,
                MoveX = _input.Move.x
            };
            Managers.Network.Send(moveIndoorPacket);

            // update last movement
            _lastMoveX = _input.Move.x;
        }
    }

    void CameraMove()
    {
        float newPosX = gameObject.transform.position.x;
        if (newPosX - _cameraWidth < _leftEnd.transform.position.x)
            newPosX = _mainCamera.transform.position.x;
        if (newPosX + _cameraWidth > _rightEnd.transform.position.x)
            newPosX = _mainCamera.transform.position.x;

        _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, new Vector3(newPosX, _mainCamera.transform.position.y, -400), Time.deltaTime * cameraSpeed);
    }
}
