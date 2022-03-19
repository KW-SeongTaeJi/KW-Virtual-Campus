using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager
{
    public string BaseUrl { get; set; } = "https://localhost:5001/api";


    // <T>: response packet type, res: response callback function 
    IEnumerator CoSendWebRequest<T>(string extraUrl, string method, object packet, Action<T> resCallback)
    {
        string url = $"{BaseUrl}/{extraUrl}";

        byte[] jsonBytes = null;
        if (packet != null)
        {
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(packet);
            jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
        }

        using (var uwr = new UnityWebRequest(url, method))
        {
            uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                T resPacket = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(uwr.downloadHandler.text);
                resCallback.Invoke(resPacket);
            }
        }
    }
    public void SendPostRequest<T>(string extraUrl, object packet, Action<T> resCallback)
    {
        Managers.Instance.StartCoroutine(CoSendWebRequest(extraUrl, UnityWebRequest.kHttpVerbPOST, packet, resCallback));
    }
}
