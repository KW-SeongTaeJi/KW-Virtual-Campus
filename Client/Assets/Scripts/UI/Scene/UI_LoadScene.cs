using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_LoadScene : UI_Base
{
    static public string NextScene { get; set; }
    static public int GameChannel { get; set; }
    static public AsyncOperation AsyncOp { get; set; }

    // name of components to bind
    enum Images
    {
        LoadingbarImage
    }


    public override void Init()
    {
        Bind<Image>(typeof(Images));
    }

    void Start()
    {
        StartCoroutine(CoLoadNextScene());
    }

    IEnumerator CoLoadNextScene()
    {
        // NextScene �񵿱� �ε� ����
        AsyncOp = SceneManager.LoadSceneAsync(NextScene);
        AsyncOp.allowSceneActivation = false;

        float timer = 0f;
        bool isEnter = true;
        Image loadingBar = GetImage((int)Images.LoadingbarImage);

        while (AsyncOp.isDone == false)
        {
            yield return null;

            // 90% ���� -> �ε� �۾� ���൵�� ���� true loading bar update
            if (AsyncOp.progress < 0.9f)
            {
                loadingBar.fillAmount = AsyncOp.progress;
            }
            // 90% ���� -> 1�� ���� fake loading bar update
            else
            {
                // 90% �������� ���� ���� ����
                if (isEnter)
                {
                    // Lobby �� ���� �۾�
                    if (NextScene == "Lobby")
                    {
                        Managers.Network.ConnectToLobbyServer();
                    }
                    // Game �� ���� �۾�
                    else if (NextScene == "Game")
                    {
                        switch (GameChannel)
                        {
                            case 0:
                                C_LeaveIndoor leaveIndoorPacket = new C_LeaveIndoor();
                                Managers.Network.Send(leaveIndoorPacket);
                                break;
                            case 1:
                                Managers.Network.ConnectToGameServer1();
                                break;
                        }
                    }
                    // Indoor_ �� ���� �۾�
                    else if (NextScene.Contains("Indoor"))
                    {
                        C_EnterIndoor enterIndoorPacket = new C_EnterIndoor()
                        {
                            Place = (Place)Enum.Parse(typeof(Place), NextScene)
                        };
                        Managers.Network.Send(enterIndoorPacket);
                    }

                    isEnter = false;
                }

                timer += Time.unscaledDeltaTime;
                loadingBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

                if (loadingBar.fillAmount >= 1f)
                {
                    AsyncOp.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
