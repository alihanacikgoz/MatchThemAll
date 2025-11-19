using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MatchThemAll.Scripts.Runtime.Managers
{
    public class PoolManager : MonoBehaviour
    {
        #region Singleton

        public static PoolManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region Variables

        #region Private Variables

        private Dictionary<string, IObjectPool<GameObject>> _pools = new Dictionary<string, IObjectPool<GameObject>>();

        #endregion

        #endregion

        public void InitializePool(int defaultCapacity, int maxCapacity, string poolName = null,
            Transform parent = null, GameObject prefab = null)
        {
            if (prefab == null)
            {
                Debug.LogError($"Prefab cannot be null");
                return;
            }

            string poolId = $"{poolName}_{prefab.name}";

            if (_pools.ContainsKey(poolId))
            {
                Debug.LogWarning($"Pool with ID '{poolId}' already exists");
                return;
            }

            var localPrefab = prefab;
            var localParent = parent;
            var localPoolName = poolName;

            var pool = new ObjectPool<GameObject>(
                createFunc: () => CreateItem(localPoolName, localParent, localPrefab),
                actionOnGet: (item) => item.SetActive(true),
                actionOnRelease: (item) => item.SetActive(false),
                actionOnDestroy: (item) => Destroy(item),
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxCapacity
            );

            _pools.Add(poolId, pool);
            Debug.Log($"Pool initialized : {poolId} (Capacity: {defaultCapacity}, Max Size: {maxCapacity})");
        }

        private GameObject CreateItem(string poolName, Transform parent, GameObject prefab)
        {
            GameObject go = Instantiate(prefab, parent, true);
            go.name = $"{poolName}_{prefab.name}";
            go.SetActive(false);
            return go;
        }

        public GameObject GetItem(string poolName, string prefabName)
        {
            string poolId = $"{poolName}_{prefabName}";

            if (_pools.TryGetValue(poolId, out var pool))
            {
                return pool.Get();
            }

            Debug.LogError($"Pool with ID '{poolId}' does not exist");
            return null;
        }

        public void ReturnToPool(GameObject obj, string poolName, string prefabName)
        {
            string poolId = $"{poolName}_{prefabName}";

            if (_pools.TryGetValue(poolId, out var pool))
            {
                pool.Release(obj);
            }
            else
            {
                Debug.LogError($"Pool with ID '{poolId}' does not exist");
                Destroy(obj);
            }
        }

        public void ClearPool(string poolName, string prefabName)
        {
            string poolId = $"{poolName}_{prefabName}";

            if (_pools.TryGetValue(poolId, out var pool))
            {
                pool.Clear();
                Debug.Log($"Pool cleared : {poolId}");
            }
        }

        public void ClearAllPools()
        {
            foreach (var pool in _pools.Values)
            {
                pool.Clear();
                Debug.Log("All pools cleared");
            }
        }

        public GameObject CreateItems(string poolId, bool activateItems = false)
        {
            var item = _pools[poolId].Get();
            item.SetActive(activateItems);
            return item;
        }
    }
}