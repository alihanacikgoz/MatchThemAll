using System;
using System.Collections.Generic;
using MatchThemAll.Scripts.Runtime.Data;
using MatchThemAll.Scripts.Runtime.Enums;
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

        [Foldout("Settings"), SerializeField] private Difficulty difficulty;
        [Foldout("Settings"), SerializeField] private int defaultCapacity = 10;
        [Foldout("Settings"), SerializeField] private int maxCapacity = 50;
        [Foldout("Settings"), SerializeField] private string poolName = "ItemPool";
        [Foldout("Settings"), SerializeField] private float initialSize;

        #endregion

        #endregion

        private void OnEnable()
        {
            LevelVariableRegulator();
        }

        private void LevelVariableRegulator()
        {
            LevelData levelData = LevelManager.Instance.GetCurrentLevel();
            difficulty = levelData.levelDifficulty;
            defaultCapacity = levelData.itemsToMatchCount;
            maxCapacity = levelData.itemsToMaxCount;
        }

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
                    if (poolItem.transform.gameObject.CompareTag("RedAmber"))
                    {
                        poolItem.transform.localScale = Vector3.one * (initialSize - 0.2f);
                    }
                    else
                    {
                        poolItem.transform.localScale = Vector3.one * initialSize;
                    }
                    poolItem.transform.SetParent(transform);
                }
            }
        }
        
    }
}