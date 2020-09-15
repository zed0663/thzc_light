using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpHelper
{
    public enum MethodType
    {
        GET,
        POST
    }
    //请求之后要有回调

    public class DownloadHanlderType
    {
        public const string kHttpBYTE = "BYTE";
        public const string kHttpTEXT = "TEXT";
    }

    public static void Request(MonoBehaviour mono, string url, MethodType method, Dictionary<string, object> form, Action<object> callback, Action<object> errorCallback, string responseType)
    {
        if (method == MethodType.POST)
        {
            url = CreateGetData(url, form);
            mono.StartCoroutine(Request(url, null, callback, errorCallback, responseType));
        }
        else if (method == MethodType.GET)
        {
            WWWForm formData = CreatePostData(form);
            mono.StartCoroutine(Request(url, formData, callback, errorCallback, responseType));
        }
    }

    static IEnumerator Request(string url, WWWForm form, Action<object> callback, Action<object> errorCallback, string dateType)
    {
        UnityWebRequest request = null;
        if (form == null)
            request = UnityWebRequest.Get(url);
        else
            request = UnityWebRequest.Post(url, form);

        yield return request.SendWebRequest();

        bool isError = false;
        if (request.isHttpError || request.isNetworkError)
        {
            isError = true;
            Debug.LogErrorFormat("Request Error: {0}", request.error);
        }

        if (request.isDone)
        {
            if (isError)
            {
                errorCallback?.Invoke(request.error);
            }
            else
            {
                if (dateType == DownloadHanlderType.kHttpTEXT)
                {
                    callback?.Invoke(request.downloadHandler.text);
                }
                else if (dateType == DownloadHanlderType.kHttpBYTE)
                {
                    callback?.Invoke(request.downloadHandler.data);
                }
                else
                {
                    Debug.LogError("不能这样...");
                }
            }
        }
    }

    private static string CreateGetData(string url, Dictionary<string, object> form)
    {
        string data = "";
        if (form != null && form.Count > 0)
        {
            foreach (var item in form)
            {
                data += item.Key + "=";
                data += item.Value.ToString() + "&";
            }
        }
        if (url.IndexOf("?") == -1)
            url += "?";
        else
            url += "&";

        url += data.TrimEnd(new char[] { '&' });
        return url;
    }

    private static WWWForm CreatePostData(Dictionary<string, object> formData)
    {
        WWWForm form = new WWWForm();
        if (formData != null && formData.Count > 0)
        {
            foreach (var item in formData)
            {
                if (item.Value is byte[])
                    form.AddBinaryData(item.Key, item.Value as byte[]);
                else
                    form.AddField(item.Key, item.Value.ToString());
            }
        }
        return form;
    }
}
