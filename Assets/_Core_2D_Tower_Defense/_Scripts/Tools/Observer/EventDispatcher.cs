using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : Singleton<EventDispatcher>
{
    // Dictionary lưu trữ các sự kiện xảy ra trong game, key là EventID, value là 1 Action
    private Dictionary<EventID, Action<object>> gameEventsManager = new Dictionary<EventID, Action<object>>();

    // Đăng ký lắng nghe sự kiện
    public void RegisterListener(EventID eventID, Action<object> callBackAction)
    {
        // Nếu id của sự kiện đã tồn tại trong Dictionary thì cho lắng nghe thêm callBackAction
        if (gameEventsManager.ContainsKey(eventID))
        {
            gameEventsManager[eventID] += callBackAction;
        }
        // Nếu id của sự kiện chưa tồn tại trong Dictionary, thêm nó vào Dictionary rồi cho lắng nghe thêm callBackAction
        else
        {
            gameEventsManager.Add(eventID, null);
            gameEventsManager[eventID] += callBackAction;
        }
    }

    // Bắn sự kiện cho những object đăng ký lắng nghe sự kiện
    public void PostEvent(EventID eventID)
    {
        // Nếu trong Dictionary không có id truyền vào thì thông báo không có object nào lắng nghe sự kiện
        if (!gameEventsManager.ContainsKey(eventID))
        {
            Debug.Log("Event has no Listener");
        }
        
        var callbacks = gameEventsManager[eventID];
        // if there's no listener remain, then do nothing
        if (callbacks != null)
        {
            callbacks(eventID);
        }
        else
        {
            Debug.Log("PostEvent " + eventID + "but no listener remain, Remove this key");
            gameEventsManager.Remove(eventID);
        }
    }

    // Huỷ đăng ký sự kiện
    public void RemoveListener(EventID eventID, Action<object> callBackAction)
    {
        // Nếu trong Dictionary có chứa id truyền vào thì huỷ đăng ký lắng nghe sự kiện callBackAction
        if (gameEventsManager.ContainsKey(eventID))
        {
            gameEventsManager[eventID] -= callBackAction;
        }
        // Nếu trong Dictionary không chứa id truyền vào thì thông báo không tìm thấy key
        else
        {
            Debug.Log("Not Found EventID with id: " + eventID);
        }
    }

    // Huỷ đăng ký tất cả sự kiện của tất cả Object
    public void RemoveAllListeners()
    {
        // Xoá hết sự kiện trong Dictionary
        gameEventsManager.Clear();
    }

    private void OnDestroy()
    {
        RemoveAllListeners();
    }
}