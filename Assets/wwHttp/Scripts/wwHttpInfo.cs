using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 请求，返回数据类
/// </summary>
public class wwHttpInfo {
	public string url;         //请求url

    public string requestData;
    public string uploadFile;//要上传的文件 
	public int timeoutSeconds; //超时时间，秒
	public float costTime;     //请求花费时间：秒
	public bool isFinish;//请求是否完成
	
	public int resultCode;      //错误代码xxHttpErrorCode
	public string errorData;   //错误数据
	public WWW www;

    public string responseData
    {
        get
        {
            try
            {
                return www.text;
            }
            catch (Exception e)
            {
                wwDebug.LogException(e);
            }
            return null;
        }
    }

    public Texture2D texture
    {
        get
        {
            try
            {
                return www.texture;
            }
            catch (Exception e)
            {
                wwDebug.LogException(e);
            }
            return null;
        }
    }

    public byte[] bytes
    {
        get
        {
            try
            {
                return www.bytes;
            }
            catch (Exception e)
            {
                wwDebug.LogException(e);
            }
            return null;
        }
    }

    public float requestProgress
    {
        get
        {
            try
            {
                return www.uploadProgress;
            }
            catch (Exception e)
            {
                wwDebug.LogException(e);
            }
            return 0f;
        }
    }

	public wwHttpEvent.HttpVoidDelege timeoutDelege;
	public wwHttpEvent.HttpVoidDelege errorDelege;
	public wwHttpEvent.HttpVoidDelege successDelege;
}

public class wwHttpEvent
{
	public delegate void HttpVoidDelege(wwHttpInfo info);
}

public class xxHttpResultCode
{
    public static readonly int CODE_SUCCESS = 0;     //成功
    public static readonly int CODE_TIMEOUT = 1;     //超时
    public static readonly int CODE_REQ_ERROR = 2;   //请求url出错
    public static readonly int CODE_RESP_NULL = 3;   //返回为空
    public static readonly int CODE_CANCEL = 4;   //用户取消

}
