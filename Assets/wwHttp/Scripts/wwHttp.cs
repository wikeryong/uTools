using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class wwHttp {

    public static string CurrentCookie = string.Empty;

    public static Encoding DEFAULT_ENCODING = System.Text.UTF8Encoding.Default;
    public static WWW WWWPost(string url, Dictionary<string, string> headers, byte[] reqData)
    {
        if (headers == null)
        {
            headers = new Dictionary<string, string>();
        }
        //添加Header信息
        if (!string.IsNullOrEmpty(CurrentCookie))
        {
            headers.Add("Cookie", CurrentCookie);
        }
        wwDebug.Log("Request headers:");
        foreach (KeyValuePair<string,string> keyVal in headers)
        {
            wwDebug.Log(keyVal.Key+","+keyVal.Value);
        }
        wwDebug.Log("Request url:"+url);
        WWW www = null;
        if (reqData == null)
        {
            www = new WWW(url);
        }
        else
        {
            wwDebug.Log("Request body:" + DEFAULT_ENCODING.GetString(reqData));
            www = new WWW(url, reqData, headers);
        }
        return www;
    }
    public static WWW WWWPost(string url, string reqData)
    {
        byte[] b = null;
        if (!string.IsNullOrEmpty(reqData))
        {
            b = DEFAULT_ENCODING.GetBytes(Uri.EscapeDataString(reqData));
        }
        return WWWPost(url, null, b);
    }

    public static void SetCookie(WWW www)
    {
        if (www != null && www.responseHeaders != null)
        {
            Dictionary<string, string> head = www.responseHeaders;
            foreach (string key in head.Keys)
            {
                wwDebug.Log("Response Header:" + key + "=" + head[key]);
                if ("SET-COOKIE".Equals(key.ToUpper()))
                {
                    wwDebug.Log("Set Cookie:" + head[key]);
                    CurrentCookie = head[key];
                    break;
                }
            }
        }
    }
    public static WWW WWWPost(string url, WWWForm form)
    {
        if (form == null)
        {
            form = new WWWForm();
        }
        //添加Header信息
        if (!string.IsNullOrEmpty(CurrentCookie))
        {
            form.AddField("Cookie", CurrentCookie);
        }
        wwDebug.Log("Request headers:");
        foreach (KeyValuePair<string, string> keyVal in form.headers)
        {
            wwDebug.Log(keyVal.Key + "," + keyVal.Value);
        }
        wwDebug.Log("Request url:" + url);
        wwDebug.Log("Request body:" + DEFAULT_ENCODING.GetString(form.data));
        WWW www = new WWW(url, form);
        return www;
    }
}
