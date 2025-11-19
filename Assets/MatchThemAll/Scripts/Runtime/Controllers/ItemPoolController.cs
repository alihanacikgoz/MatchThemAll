using System;
using System.Collections.Generic;
using MatchThemAll.Scripts.Runtime.Managers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;

namespace MatchThemAll.Scripts.Runtime.Controllers
{
    public class ItemPoolController : MonoBehaviour
    {
        #region Singleton

        public static ItemPoolController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        #endregion


        #region Variables

        #region SerializeField Variables

        [Foldout("References"), SerializeField]
        private List<GameObject> itemPrefabs = new List<GameObject>();

        [Foldout("Settings"), SerializeField] private int defaultCapacity = 10;
        [Foldout("Settings"), SerializeField] private int maxCapacity = 50;
        [Foldout("Settings"), SerializeField] private string poolName = "ItemPool";

        #endregion

        #endregion

        private void Start()
        {
            foreach (var item in itemPrefabs)
            {
                PoolManager.Instance.InitializePool(defaultCapacity, maxCapacity, poolName, transform, item);
            }

            foreach (var item in itemPrefabs)
            {
                for (int i = 0; i < defaultCapacity; i++)
                {
                    var poolId = $"{poolName}_{item.name}";
                    var poolItem = PoolManager.Instance.CreateItems(poolId, true);
                    poolItem.transform.SetParent(transform);
                }
            }
        }
        
    }
}