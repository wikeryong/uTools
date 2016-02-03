using UnityEngine;
using System.Collections;

public class wwDemo : MonoBehaviour {

    wwHttpClient test1Client;

    void Start()
    {
        test1Client = wwHttpManager.CreateHttpRequest("http://www.baidu.com", "333", TestCallback);
    }

    private void TestCallback(wwHttpInfo info)
    {
        wwDebug.Log("Callback  RESP CODE:" + info.resultCode);
        wwDebug.Log("Callback  RESP:" + info.responseData);
        wwDebug.Log("Callback  ERROR:" + info.www.error);
    }
	// Update is called once per frame

    void OnGUI()
    {
        if (GUI.Button(new Rect(0,0,100,50),"普通请求"))
        {
            test1Client.StartRequest();
        }

        if (GUI.Button(new Rect(120, 0, 100, 50), "取消普通请求"))
        {
            test1Client.Cancel();
        }

        if (GUI.Button(new Rect(0, 60, 100, 50), "添加到请求队列"))
        {
            wwHttpClient client =
                wwHttpManager.CreateHttpRequest("http://www.baidu.com", TestCallback);
            wwHttpQueue.AddToQueue(client);
        }
    }
}
