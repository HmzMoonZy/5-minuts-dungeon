using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgDistribution
{
    public int num = 15;    //每帧最多处理的消息

    public List<Message> msgList = new List<Message>();

    public delegate void ProtolDelegate(Message msg);

    private Dictionary<string, ProtolDelegate> eventDict = new Dictionary<string, ProtolDelegate>();

    private Dictionary<string, ProtolDelegate> onceDict = new Dictionary<string, ProtolDelegate>();

    /// <summary>
    /// 处理消息队列.
    /// </summary>
    public void Update()
    {
        for (int i = 0; i < num; i++)
        {
            if (msgList.Count > 0)
            {
                DispatchEvent(msgList[0]);
                lock (msgList)
                {
                    msgList.RemoveAt(0);
                }
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// 消息分发 
    /// </summary>
    /// <param name="msg"></param>
    public void DispatchEvent(Message msg)
    {
        string token = msg.Token;

        if (eventDict.ContainsKey(token))
        {
            eventDict[token](msg);
        }
        if (onceDict.ContainsKey(token))
        {
            onceDict[token](msg);
            onceDict[token] = null;
            onceDict.Remove(token);
        }
    }

    public void AddListener(string callback, ProtolDelegate protDel)
    {
        if (eventDict.ContainsKey(callback))
            eventDict[callback] += protDel;  
        else
            eventDict[callback] = protDel;
        
    }
    public void AddOnceListener(string callback, ProtolDelegate protDel)
    {
        if (onceDict.ContainsKey(callback))
            onceDict[callback] += protDel;
        else
            onceDict[callback] = protDel;
    }

    public void DelListenner(string callback, ProtolDelegate protDel)
    {
        if (eventDict.ContainsKey(callback))
        {
            eventDict[callback] -= protDel;
            if (eventDict[callback] == null) eventDict.Remove(callback);
        }
    }
    public void DelOnceListenner(string callback, ProtolDelegate protDel)
    {
        if (onceDict.ContainsKey(callback))
        {
            onceDict[callback] -= protDel;
            if (onceDict[callback] == null) onceDict.Remove(callback);
        }
    }
}
