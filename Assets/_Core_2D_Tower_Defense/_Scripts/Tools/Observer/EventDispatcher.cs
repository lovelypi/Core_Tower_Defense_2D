using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
    private static EventDispatcher instance;

    public static EventDispatcher Instance
    {
        get
        {
            // Nếu chưa có EventDispatcher trên Scene thì tạo 1 cái mới và AddComponent
            if (instance == null)
            {
                GameObject singletonObject = new GameObject();
                instance = singletonObject.AddComponent<EventDispatcher>();
                singletonObject.name = "EventDispatcher (Singleton)";
            }

            return instance;
        }
    }

    private void Awake()
    {
        // Nếu trên Scene có 1 GameObject khác cũng tên là EventDispatcher (Singleton) mà khác InstanceID thì huỷ
        // cái đó đi chỉ giữ lại 1 thể hiện thôi, còn trùng ID thì gán thành instance
        if (instance != null && instance.GetInstanceID() != this.GetInstanceID())
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    private void OnDestroy()
    {
        if (instance == this)
        {
            RemoveAllListeners();
            instance = null;
        }
    }

    // Dictionary lưu trữ các sự kiện xảy ra trong game, key là EventID, value là Action<object>
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
    public void PostEvent(EventID eventID, object param = null)
    {
        // Nếu trong Dictionary không có id truyền vào thì thông báo không có object nào lắng nghe sự kiện
        if (!gameEventsManager.ContainsKey(eventID))
        {
            Debug.Log("Event has no Listener");
            return;
        }

        var callbacks = gameEventsManager[eventID];

        // Nếu không có hàm nào bắt sự kiện thì thôi
        if (callbacks != null)
        {
            callbacks(param);
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
}

#region Extension class

// Khai báo một số “phím tắt” để sử dụng EventDispatcher dễ dàng hơn
public static class EventDispatcherExtension
{
    // Đăng ký lắng nghe sự kiện
    public static void RegisterListener(this MonoBehaviour listener, EventID eventID, Action<object> callback)
    {
        EventDispatcher.Instance.RegisterListener(eventID, callback);
    }

    // Bắn sự kiện cho những object đăng ký lắng nghe sự kiện không truyền tham số 
    public static void PostEvent(this MonoBehaviour sender, EventID eventID)
    {
        EventDispatcher.Instance.PostEvent(eventID);
    }

    // Bắn sự kiện cho những object đăng ký lắng nghe sự kiện có truyền tham số
    public static void PostEvent(this MonoBehaviour listener, EventID eventID, object param)
    {
        EventDispatcher.Instance.PostEvent(eventID, param);
    }
}

#endregion