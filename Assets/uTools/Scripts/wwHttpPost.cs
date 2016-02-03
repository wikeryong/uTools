using UnityEngine;
using System.Collections;

public class wwHttpPost : wwHttpClient {
    public override WWW CreateWWW()
    {
        WWW www = wwHttp.WWWPost(httpInfo.url, httpInfo.requestData);
        httpInfo.www = www;
        return www;
    }
}
