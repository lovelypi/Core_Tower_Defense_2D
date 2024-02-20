using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PoolingManager
{
    private const int Default_Pool_Size = 3;

    // Dictionary chứa tất cả các Pool
    public static Dictionary<int, Pool> pools;

    public static void Init(GameObject prefab = null, int initQuantity = Default_Pool_Size)
    {
        if (pools == null)
        {
            pools = new Dictionary<int, Pool>();
        }

        if (prefab != null)
        {
            var prefabID = prefab.GetInstanceID();
            if (!pools.ContainsKey(prefabID))
            {
                pools[prefabID] = new Pool(prefab, initQuantity);
            }
        }
    }

    // Chuẩn bị trước một lượng quantity GameObject từ pool và đưa chúng vào trạng thái inactive,
    // tăng hiệu suất khi cần sử dụng nhiều GameObject cùng một lúc vì chúng đã được tạo sẵn
    // và có thể được kích hoạt mà không cần instantiate mới
    public static void PoolPreLoad(GameObject prefab, int quantity, Transform newParent = null)
    {
        Init(prefab, 1);
        pools[prefab.GetInstanceID()].Preload(quantity, newParent);
    }

    // Chuẩn bị trước đối tượng từ pool trên toàn bộ class, không phụ thuộc vào một pool cụ thể nào.
    public static GameObject[] Preload(GameObject prefab, int quantity = 1, Transform parent = null)
    {
        Init(prefab, quantity);
        // Tạo mảng lưu trữ các GameObject chuẩn bị sẵn.
        var gameObjectArr = new GameObject[quantity];
        for (int i = 0; i < quantity; i++)
        {
            gameObjectArr[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
            if (parent != null)
            {
                gameObjectArr[i].transform.SetParent(parent);
            }
        }

        // Đưa hết vào hàng đợi
        for (int i = 0; i < quantity; i++)
            Despawn(gameObjectArr[i]);
        return gameObjectArr;
    }
    
    // Spawn ra 1 prefab
    public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        Init(prefab);
        return pools[prefab.GetInstanceID()].Spawn(pos, rot);
    }

    public static GameObject Spawn(GameObject prefab)
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity);
    }

    public static T Spawn<T>(T prefab) where T : Component
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity);
    }

    public static T Spawn<T>(T prefab, Vector3 pos, Quaternion rot) where T : Component
    {
        Init(prefab.gameObject);
        return pools[prefab.gameObject.GetInstanceID()].Spawn<T>(pos, rot);
    }

    // Trả 1 GameObject
    public static void Despawn(GameObject gameObject, UnityAction actionDespawn = null)
    {
        Pool p = null;
        foreach (var pool in pools.Values)
        {
            if (pool.memberIDs.Contains(gameObject.GetInstanceID()))
            {
                p = pool;
                break;
            }
        }

        if (p == null)
        {
            Debug.LogFormat("Object '{0}' wasn't spawned from a pool. Destroying it instead.", gameObject.name);
            Object.Destroy(gameObject);
        }
        else
        {
            actionDespawn?.Invoke();
            p.Despawn(gameObject);
        }
    }

    public static int GetStackCount(GameObject prefab)
    {
        if (pools == null)
            pools = new Dictionary<int, Pool>();
        if (prefab == null) return 0;
        return pools.ContainsKey(prefab.GetInstanceID()) ? pools[prefab.GetInstanceID()].NotActiveGameObjectCount : 0;
    }

    // Clear Dictionary
    public static void ClearPool()
    {
        if (pools != null)
        {
            pools.Clear();
        }
    }
}

public class Pool
{
    // Một số duy nhất được thêm vào tên của đối tượng khi được instantiate,
    // tránh trùng lặp tên và xác định thức tự instantiate
    private int id = 1;

    // Một hàng đợi (Queue<GameObject>) chứa các đối tượng không hoạt động (inactive). Hàng đợi này giữ các đối tượng sẵn sàng để được sử dụng lại thay vì instantiate mới khi cần thiết.
    private readonly Queue<GameObject> notActiveQueue;

    // HashSet HashSet<int> chứa các GetInstanceID() của đối tượng đã được instantiate từ prefab,
    // giúp kiểm tra xem đối tượng thuộc pool nào khi cần thực hiện việc Despawn.
    public readonly HashSet<int> memberIDs;

    // Prefab GameObject sẽ được Pool ra
    private readonly GameObject prefab;

    // Cho biết số lượng đối tượng hiện có trong hàng đợi 
    public int NotActiveGameObjectCount
    {
        get => notActiveQueue.Count;
    }

    // Constructor
    public Pool(GameObject prf, int initQuantity)
    {
        prefab = prf;
        notActiveQueue = new Queue<GameObject>(initQuantity);
        memberIDs = new HashSet<int>();
    }

    // tạo trước một lượng đối tượng và thêm chúng vào hàng đợi 
    public void Preload(int initQuantity, Transform parent = null)
    {
        for (int i = 0; i < initQuantity; i++)
        {
            // Tạo Gameobject, Tắt đi rồi đưa vào hàng đợi
            var gameObject = Object.Instantiate(prefab, parent);
            gameObject.name = prefab.name + " " + id;
            memberIDs.Add(gameObject.GetInstanceID());
            gameObject.SetActive(false);
            notActiveQueue.Enqueue(gameObject);
        }
    }

    public GameObject Spawn(Vector3 pos, Quaternion rot)
    {
        while (true)
        {
            GameObject gameObject;
            if (notActiveQueue.Count == 0)
            {
                gameObject = Object.Instantiate(prefab, pos, rot);
                gameObject.name = prefab.name + " " + id;
                memberIDs.Add(gameObject.GetInstanceID());
            }
            else
            {
                // Lấy ra GameObject cuối cùng 
                gameObject = notActiveQueue.Dequeue();
                if (gameObject == null)
                {
                    // GameObject lấy ra từ hàng đợi không còn tồn tại.
                    // Các nguyên nhân có thể xảy ra nhất là:
                    // - GameObject bị Destroy() trước đó 
                    // - LoadScene (sẽ phá hủy tất cả các GameObject). Điều này có thể được ngăn chặn bằng DontDestroyOnLoad
                    continue;
                }
            }

            gameObject.transform.position = pos;
            gameObject.transform.rotation = rot;
            gameObject.SetActive(true);
            return gameObject;
        }
    }

    public T Spawn<T>(Vector3 pos, Quaternion rot)
    {
        return Spawn(pos, rot).GetComponent<T>();
    }

    // Đưa đối tượng về lại pool để tái sử dụng
    public void Despawn(GameObject gameObject)
    {
        // Nếu GameObject đó đã tắt rồi thì thôi
        if (!gameObject.activeSelf)
        {
            return;
        }
        
        // Còn không thì tắt nó rồi trả nó về hàng đợi
        gameObject.SetActive(false);
        notActiveQueue.Enqueue(gameObject);
    }
}