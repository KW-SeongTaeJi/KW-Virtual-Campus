using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasController : UI_Base
{ 
    enum Texts
    {
        NameText,
        ChatText
    }
    enum Images
    {
        ChatImage
    }

    GameObject _mainCamera;

    Coroutine _coActiveChatBox;

    float _activeTime = 5.0f;


    public override void Init()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    void Update()
    {
        CanvasToCamera();
    }

    void CanvasToCamera()
    {
        Vector3 dir = _mainCamera.transform.position - transform.position;
        dir.x = 0;
        dir.z = 0;
        transform.LookAt(_mainCamera.transform.position - dir);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, transform.rotation.eulerAngles.z);
    }

    public void OnEnterChat(string msg)
    {
        if (_coActiveChatBox != null)
            StopCoroutine(_coActiveChatBox);

        _coActiveChatBox = StartCoroutine(CoActiveChatBox(msg));
    }
    IEnumerator CoActiveChatBox(string msg)
    {
        Image chatImage = Get<Image>((int)Images.ChatImage);
        TextMeshProUGUI chatText = Get<TextMeshProUGUI>((int)Texts.ChatText);

        chatImage.gameObject.SetActive(true);
        chatText.text = msg;

        yield return new WaitForSeconds(_activeTime);
        chatImage.gameObject.SetActive(false);
    }
}
