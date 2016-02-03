using System;
using UnityEngine;
using System.Collections;

public class wwHttpClient
{

    public bool isStop { private set; get; }
    public bool isStart { private set; get; }
    public wwHttpInfo httpInfo;
    private wwYieldable _yieldable = new wwYieldable();
    public wwYieldable yieldable
    {
        get
        {
            return _yieldable;
        }
    }

    public WWW www
    {
        get { return httpInfo.www; }
    }

    /// <summary>
    /// 发起请求
    /// </summary>
    public void StartRequest()
    {
        if (!httpInfo.isFinish && isStart)
        {
            wwDebug.LogWarning("Request is start!");
            return;
        }
        wwHttpManager.StartHttp(this);
    }
	
    public virtual bool CheckTimeout(float time)
    {
        //wwDebug.Log(httpInfo.www.url);
        if (isStop) return false;
        if (httpInfo.costTime > httpInfo.timeoutSeconds)
        {
            isStop = true;
            httpInfo.resultCode = xxHttpResultCode.CODE_TIMEOUT;
            httpInfo.errorData = "Http timeout";
            if (httpInfo.www != null)
                httpInfo.www.Dispose();
            return true;
        }
        httpInfo.costTime += time;
        return false;
    }

    /// <summary>
    /// 取消
    /// </summary>
    public virtual void Cancel()
    {
        if (isStop) return;

        isStop = true;
        httpInfo.resultCode = xxHttpResultCode.CODE_CANCEL;
        httpInfo.errorData = "Http cancel";
        if (httpInfo.www != null)
            httpInfo.www.Dispose();
    }

    public virtual WWW CreateWWW()
    {
        return null;
    }

    public IEnumerator PostRequest()
    {
        isStop = false;
        isStart = true;
        WWW www = CreateWWW();
        yield return www;

        try
        {
            wwHttp.SetCookie(www);
            if (!string.IsNullOrEmpty(www.error))
            {
                httpInfo.resultCode = xxHttpResultCode.CODE_REQ_ERROR;
                httpInfo.errorData = www.error;
                if (httpInfo.errorDelege != null)
                {
                    httpInfo.errorDelege(httpInfo);
                }
                wwDebug.LogWarning(string.Format("Http Error:{0}", www.error));
                RequestFinish();
                yield break;
            }

            if (string.IsNullOrEmpty(www.text))
            {
                httpInfo.resultCode = xxHttpResultCode.CODE_RESP_NULL;
                httpInfo.errorData = "Http Response is null";
                if (httpInfo.errorDelege != null)
                {
                    httpInfo.errorDelege(httpInfo);
                }
                wwDebug.LogWarning("Http Response Is Null!");
                RequestFinish();
                yield break;
            }
            wwDebug.Log("Http Response:" + www.text);
            httpInfo.resultCode = xxHttpResultCode.CODE_SUCCESS;
            if (httpInfo.successDelege != null)
            {
                httpInfo.successDelege(httpInfo);
            }
            RequestFinish();
        }
        catch (System.Exception e)
        {
            wwDebug.LogWarning("Http Request exception:" + e.StackTrace);
            if (httpInfo.resultCode == 0)
            {
                httpInfo.resultCode = xxHttpResultCode.CODE_REQ_ERROR;
                httpInfo.errorData = "Http exception";
            }
            if (httpInfo.errorDelege != null)
            {
                httpInfo.errorDelege(httpInfo);
            }
            RequestFinish();
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


    public void RequestFinish()
    {
        wwDebug.Log(string.Format("HttpClient request [{0}] finish,Cost {1} seconds", httpInfo.url, httpInfo.costTime.ToString("0.00")));
        _yieldable.Finish();
        httpInfo.isFinish = true;
        isStart = false;
    }
}
