using Cinemachine;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Managers : MonoBehaviour
{
    public void HandleEnterLobby(L_EnterLobby enterPacket)
    {
        StartCoroutine(CoHandleEnterLobby(enterPacket));
    }
    IEnumerator CoHandleEnterLobby(L_EnterLobby enterPacket)
    {
        // Scene �ε� ���
        if (UI_LoadScene.AsyncOp != null)
        {
            while (UI_LoadScene.AsyncOp.isDone == false)
            {
                yield return null;
            }
        }

        // Disconnect invalid access
        if (enterPacket.AccessOk == false)
        {
            Managers.Network.DisconnectSession();
            UI_AlertPopup alertPopup = Managers.UI.ShowPopupUI<UI_AlertPopup>();
            alertPopup.SetMessageText("�̹� �������� �����̰ų�, �ùٸ��� ���� �����Դϴ�!\n���α׷��� �����մϴ�.");
            alertPopup.Quit = true;
            yield break;
        }

        // Lobby player ����
        GameObject player = Managers.Resource.Instantiate("Object/MyLobbyPlayer");
        Player info = player.GetComponent<Player>();
        {
            info.Name = Managers.Network.Name;
            info.HairType = enterPacket.HairType;
            info.FaceType = enterPacket.FaceType;
            info.JacketType = enterPacket.JacketType;
            info.HairColor = enterPacket.HairColor;
            info.FaceColor_X = enterPacket.FaceColor.X;
            info.FaceColor_Y = enterPacket.FaceColor.Y;
            info.FaceColor_Z = enterPacket.FaceColor.Z;
        }

        LobbyScene lobbyScene = (LobbyScene)Managers.SceneLoad.CurrentScene;
        lobbyScene.PlayerInfo = info;
        yield break;
    }

    public void HandleEnterGame(S_EnterGame enterGamePacket)
    {
        StartCoroutine(CoHandleEnterGame(enterGamePacket));
    }
    IEnumerator CoHandleEnterGame(S_EnterGame enterGamePacket)
    {
        // Scene �ε� ���
        if (UI_LoadScene.AsyncOp != null)
        {
            while (UI_LoadScene.AsyncOp.isDone == false)
            {
                yield return null;
            }
        }

        // Instantiate my player
        GameObject myPlayer = Managers.Object.Add(enterGamePacket.MyPlayer, myPlayer: true);
        for (int i = 0; i < enterGamePacket.Friends.Count; i++)
        {
            FriendInfo friend = new FriendInfo()
            {
                Name = enterGamePacket.Friends[i].Name,
                HairType = enterGamePacket.Friends[i].HairType,
                FaceType = enterGamePacket.Friends[i].FaceType,
                JacketType = enterGamePacket.Friends[i].JacketType,
                HairColor = enterGamePacket.Friends[i].HairColor,
                FaceColor_X = enterGamePacket.Friends[i].FaceColor.X,
                FaceColor_Y = enterGamePacket.Friends[i].FaceColor.Y,
                FaceColor_Z = enterGamePacket.Friends[i].FaceColor.Z
            };
            Managers.Object.MyPlayer.Friends.Add(friend.Name, friend);
        }
        ((UI_GameScene)Managers.UI.SceneUI).SetFriendList();

        // My player Camera ���� �� ����
        GameObject gameObject = Managers.Resource.Instantiate("Object/MyPlayerFollowCamera");
        CinemachineVirtualCamera followCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        followCamera.Follow = myPlayer.FindChild("PlayerCameraRoot").transform;
    }

    public void HandleSpawn(S_Spawn spawnPacket)
    {
        StartCoroutine(CoHandleSpawn(spawnPacket));
    }
    IEnumerator CoHandleSpawn(S_Spawn spawnPacket)
    {
        // Scene �ε� ���
        if (UI_LoadScene.AsyncOp != null)
        {
            while (UI_LoadScene.AsyncOp.isDone == false)
            {
                yield return null;
            }
        }

        UI_Scene sceneUI = Managers.UI.SceneUI;
        foreach (ObjectInfo obj in spawnPacket.Objects)
        {
            // object ����
            Managers.Object.Add(obj);

            // ģ�� ��� ����
            if (Managers.SceneLoad.CurrentScene.SceneType == Define.Scene.Game)
            {
                if (((UI_GameScene)sceneUI).FriendListSlots.ContainsKey(obj.PlayerInfo.Name))
                {
                    ((UI_GameScene)sceneUI).FriendListSlots[obj.PlayerInfo.Name].SetOnline();
                }
            }
            else
            {
                if (((UI_IndoorScene)sceneUI).FriendListSlots.ContainsKey(obj.PlayerInfo.Name))
                {
                    ((UI_IndoorScene)sceneUI).FriendListSlots[obj.PlayerInfo.Name].SetOnline();
                }
            }
        }
    }

    public void HandleEnterIndoor(S_EnterIndoor enterIndoorPacket)
    {
        StartCoroutine(CoHandleEnterIndoor(enterIndoorPacket));
    }
    IEnumerator CoHandleEnterIndoor(S_EnterIndoor enterIndoorPacket)
    {
        // Scene �ε� ���
        if (UI_LoadScene.AsyncOp != null)
        {
            while (UI_LoadScene.AsyncOp.isDone == false)
            {
                yield return null;
            }
        }

        // Instantiate my player
        GameObject myPlayer = Managers.Object.AddIndoor(enterIndoorPacket.MyPlayer, myPlayer: true);
        for (int i = 0; i < enterIndoorPacket.Friends.Count; i++)
        {
            FriendInfo friend = new FriendInfo()
            {
                Name = enterIndoorPacket.Friends[i].Name,
                HairType = enterIndoorPacket.Friends[i].HairType,
                FaceType = enterIndoorPacket.Friends[i].FaceType,
                JacketType = enterIndoorPacket.Friends[i].JacketType,
                HairColor = enterIndoorPacket.Friends[i].HairColor,
                FaceColor_X = enterIndoorPacket.Friends[i].FaceColor.X,
                FaceColor_Y = enterIndoorPacket.Friends[i].FaceColor.Y,
                FaceColor_Z = enterIndoorPacket.Friends[i].FaceColor.Z
            };
            Managers.Object.MyIndoorPlayer.Friends.Add(friend.Name, friend);
        }
        ((UI_IndoorScene)Managers.UI.SceneUI).SetFriendList();
    }

    public void HandleSpawnIndoor(S_SpawnIndoor spawnIndoorPacket)
    {
        StartCoroutine(CoHandleSpawnIndoor(spawnIndoorPacket));
    }
    IEnumerator CoHandleSpawnIndoor(S_SpawnIndoor spawnIndoorPacket)
    {
        // Scene �ε� ���
        if (UI_LoadScene.AsyncOp != null)
        {
            while (UI_LoadScene.AsyncOp.isDone == false)
            {
                yield return null;
            }
        }

        UI_Scene sceneUI = Managers.UI.SceneUI;
        foreach (ObjectInfo obj in spawnIndoorPacket.Objects)
        {
            // indoor object ����
            Managers.Object.AddIndoor(obj);

            // ģ�� ��� ����
            if (Managers.SceneLoad.CurrentScene.SceneType == Define.Scene.Game)
            {
                if (((UI_GameScene)sceneUI).FriendListSlots.ContainsKey(obj.PlayerInfo.Name))
                {
                    ((UI_GameScene)sceneUI).FriendListSlots[obj.PlayerInfo.Name].SetOnline();
                }
            }
            else
            {
                if (((UI_IndoorScene)sceneUI).FriendListSlots.ContainsKey(obj.PlayerInfo.Name))
                {
                    ((UI_IndoorScene)sceneUI).FriendListSlots[obj.PlayerInfo.Name].SetOnline();
                }
            }
        }
    }
}
