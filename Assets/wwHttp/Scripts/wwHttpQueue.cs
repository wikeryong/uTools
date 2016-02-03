using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 管理所有的请求队列，只有当前一个请求处理完后才会进行下一个请求
/// </summary>
public class wwHttpQueue : MonoBehaviour
{

    private Queue<wwHttpClient> requestQueue;
    public static wwHttpQueue instance { private set; get; }

    public static int size
    {
        get 
        { 
            if (instance == null) return 0;

            if (instance.requestQueue == null)
            {
                return 0;
            }
            return instance.requestQueue.Count;
        }
    }

    public static wwHttpClient currentClient
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            if (instance.requestQueue == null)
            {
                return null;
            }
            return instance.requestQueue.Peek();
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        requestQueue = new Queue<wwHttpClient>();
    }

    void Destryo()
    {
        instance = null;
    }

    private bool hasRequest = false;
	// Update is called once per frame
	void Update ()
	{
	    if (hasRequest) return;
	    if (requestQueue == null) return;
	    if (requestQueue.Count <= 0) return;
        wwHttpClient client = requestQueue.Dequeue();
	    if (client == null) return;

        StartCoroutine(StartRequest(client));
	}

    private IEnumerator StartRequest(wwHttpClient client)
    {
        hasRequest = true;
        client.StartRequest();
        yield return StartCoroutine(client.yieldable);
        hasRequest = false;
    }

    public static void AddToQueue(wwHttpClient client)
    {
        if (instance == null) return;
        if (instance.requestQueue == null)
        {
            wwDebug.LogWarning("Http Queue not instantiate!");
            return;
        }
        instance.requestQueue.Enqueue(client);
    }

}
