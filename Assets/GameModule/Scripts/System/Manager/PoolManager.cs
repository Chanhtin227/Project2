using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [System.Serializable]
    public class PoolItem
    {
        public string key;
        public GameObject prefab;
        public int initialSize = 10;
    }

    [Header("Pool Config")]
    public List<PoolItem> pools = new List<PoolItem>();

    private Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<GameObject, string> prefabToKey = new Dictionary<GameObject, string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        foreach (var item in pools)
        {
            var queue = new Queue<GameObject>();
            for (int i = 0; i < item.initialSize; i++)
            {
                var obj = Instantiate(item.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            poolDict[item.key] = queue;
            prefabToKey[item.prefab] = item.key;
        }
    }

    // --- API ---

    // Bản chuẩn (tự động reset transform)
    public GameObject Get(string key)
    {
        if (!poolDict.ContainsKey(key))
        {
            Debug.LogWarning($"[PoolManager] Key {key} chưa được đăng ký!");
            return null;
        }

        GameObject obj = null;
        if (poolDict[key].Count > 0)
        {
            obj = poolDict[key].Dequeue();
        }
        else
        {
            var prefab = pools.Find(p => p.key == key).prefab;
            obj = Instantiate(prefab, transform);
        }

        obj.transform.SetParent(null, true); // tách khỏi PoolManager
        obj.transform.localScale = Vector3.one; // reset scale
        obj.SetActive(true);
        return obj;
    }

    // Overload tiện cho spawn với vị trí và rotation
    public GameObject Get(string key, Vector3 position, Quaternion rotation)
    {
        var obj = Get(key);
        if (obj == null) return null;

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public T Get<T>(string key) where T : Component
    {
        var obj = Get(key);
        return obj != null ? obj.GetComponent<T>() : null;
    }

    public void Return(GameObject obj, string key)
    {
        if (!poolDict.ContainsKey(key))
        {
            Debug.LogWarning($"[PoolManager] Không tìm thấy key {key}, hủy đối tượng!");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(transform);
        poolDict[key].Enqueue(obj);
    }

    public void Return(GameObject obj, GameObject prefab)
    {
        if (prefabToKey.ContainsKey(prefab))
        {
            Return(obj, prefabToKey[prefab]);
        }
        else
        {
            Debug.LogWarning("[PoolManager] Prefab chưa được đăng ký trong PoolManager!");
            Destroy(obj);
        }
    }
}
