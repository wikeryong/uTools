using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class wwHttpManager : MonoBehaviour {

	public static wwHttpManager instance 
	{
		get;
		private set;
	}

    public static readonly int CONNECT_TIMEOUT = 20;

    private static List<wwHttpClient> requestList = new List<wwHttpClient>();

	void Awake()
	{
		instance = this;
	}

	void OnDestroy()
	{
		instance = null;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    CheckRequest();
	}

    /// <summary>
    /// 是否打开日志
    /// </summary>
    /// <param name="openLog"></param>
    public static void OpenLog(bool openLog)
    {
        wwDebug.consoleLogEnabled = openLog;
    }

    public static void StartHttp(wwHttpClient client)
    {
        if (instance == null)
        {
            wwDebug.LogWarning("wwHttpManager not instantiate");
            return;
        }
        requestList.Add(client);

        instance.StartCoroutine(client.PostRequest());
    }

    public static wwHttpClient CreateHttpRequest(string url, string reqData, int timeoutSeconds, wwHttpEvent.HttpVoidDelege timeoutCallback, wwHttpEvent.HttpVoidDelege errorCallback, wwHttpEvent.HttpVoidDelege successCallback)
    {
        if (instance == null)
        {
            wwDebug.LogWarning("wwHttpManager not instantiate");
            return null;
        }

        wwHttpClient client = CreatePostClient( url, reqData, timeoutSeconds, timeoutCallback, errorCallback, successCallback);
        return client;
    }

    public static wwHttpClient CreateHttpRequest(string url, string reqData, int timeoutSeconds, wwHttpEvent.HttpVoidDelege callback)
    {
        return CreateHttpRequest(url, reqData, timeoutSeconds, callback, callback, callback);
    }

    public static wwHttpClient CreateHttpRequest(string url, string reqData, wwHttpEvent.HttpVoidDelege callback)
    {
        return CreateHttpRequest(url, reqData, CONNECT_TIMEOUT, callback, callback, callback);
    }

    public static wwHttpClient CreateHttpRequest(string url, string reqData, int timeoutSeconds)
    {
        return CreateHttpRequest( url, reqData, timeoutSeconds, null, null, null);
    }

    public static wwHttpClient CreateHttpRequest(string url, string reqData)
    {
        return CreateHttpRequest( url, reqData, CONNECT_TIMEOUT, null, null, null);
    }
    public static wwHttpClient CreateHttpRequest(string url, wwHttpEvent.HttpVoidDelege callback)
    {
        return CreateHttpRequest(url, null, CONNECT_TIMEOUT, callback, callback, callback);
    }
    public static wwHttpClient CreateHttpRequest(string url)
    {
        return CreateHttpRequest(url, null, CONNECT_TIMEOUT, null, null, null);
    }




    public static wwHttpClient CreateUpdFileRequest(string url, string uploadFile, int timeoutSeconds, wwHttpEvent.HttpVoidDelege timeoutCallback, wwHttpEvent.HttpVoidDelege errorCallback, wwHttpEvent.HttpVoidDelege successCallback)
    {
        if (instance == null)
        {
            wwDebug.LogWarning("wwHttpManager not instantiate");
            return null;
        }

        wwHttpClient client = CreateUploadClient(url, uploadFile, timeoutSeconds, timeoutCallback, errorCallback, successCallback);
        return client;
    }

    public static wwHttpClient CreateUpdFileRequest(string url, string uploadFile, int timeoutSeconds, wwHttpEvent.HttpVoidDelege callback)
    {
        return CreateUpdFileRequest(url, uploadFile, timeoutSeconds, callback, callback, callback);
    }

    public static wwHttpClient CreateUpdFileRequest(string url, string uploadFile, wwHttpEvent.HttpVoidDelege callback)
    {
        return CreateUpdFileRequest(url, uploadFile, CONNECT_TIMEOUT, callback, callback, callback);
    }

    public static wwHttpClient CreateUpdFileRequest(string url, string uploadFile, int timeoutSeconds)
    {
        return CreateUpdFileRequest(url, uploadFile, timeoutSeconds, null, null, null);
    }

    public static wwHttpClient CreateUpdFileRequest(string url, string uploadFile)
    {
        return CreateUpdFileRequest(url, uploadFile, CONNECT_TIMEOUT, null, null, null);
    }
    public static wwHttpClient CreateUpdFileRequest(string url, wwHttpEvent.HttpVoidDelege callback)
    {
        return CreateUpdFileRequest(url, null, CONNECT_TIMEOUT, callback, callback, callback);
    }


    public static wwHttpClient CreatePostClient(string url, string reqData, int timeoutSeconds, wwHttpEvent.HttpVoidDelege timeoutCallback, wwHttpEvent.HttpVoidDelege errorCallback, wwHttpEvent.HttpVoidDelege successCallback)
    {
        //Uri uri = new Uri(url);
        wwHttpInfo httpInfo = new wwHttpInfo();
        httpInfo.url = url;
        httpInfo.requestData = reqData;
        httpInfo.timeoutSeconds = timeoutSeconds;
        httpInfo.timeoutDelege = timeoutCallback;
        httpInfo.successDelege = successCallback;
        httpInfo.errorDelege = errorCallback;
        wwHttpPost client = new wwHttpPost();
        client.httpInfo = httpInfo;
        return client;
    }

    public static wwHttpClient CreateUploadClient(string url, string uploadFile, int timeoutSeconds, wwHttpEvent.HttpVoidDelege timeoutCallback, wwHttpEvent.HttpVoidDelege errorCallback, wwHttpEvent.HttpVoidDelege successCallback)
    {
        //Uri uri = new Uri(url);
        wwHttpInfo httpInfo = new wwHttpInfo();
        httpInfo.url = url;
        httpInfo.uploadFile = uploadFile;
        httpInfo.timeoutSeconds = timeoutSeconds;
        httpInfo.timeoutDelege = timeoutCallback;
        httpInfo.successDelege = successCallback;
        httpInfo.errorDelege = errorCallback;
        wwHttpUploadFile client = new wwHttpUploadFile();
        client.httpInfo = httpInfo;
        return client;
    }

    private void CheckRequest()
    {
        for (int i = 0; i < requestList.Count; i++)
        {
            wwHttpClient tempClient = requestList[i];
            tempClient.CheckTimeout(wwRealTime.deltaTime);
            if (tempClient.httpInfo.isFinish)
            {
                requestList.RemoveAt(i);
                break;
            }
            if (tempClient.isStop)
            {
                requestList.RemoveAt(i);
                break;
            }
        }
    }
}
