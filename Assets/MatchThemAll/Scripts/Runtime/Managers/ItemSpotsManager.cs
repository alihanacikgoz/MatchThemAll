using System.Collections.Generic;
using System.Linq;
using MatchThemAll.Scripts.Runtime.Controllers;
using MatchThemAll.Scripts.Runtime.Signals;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;

namespace MatchThemAll.Scripts.Runtime.Managers
{
    public class ItemSpotsManager : MonoBehaviour
    {
        #region Variables

        #region SerializeField Variables

        [Foldout("Elements"), SerializeField] private List<GameObject> itemSpots = new List<GameObject>();
        
        [Foldout("References"), SerializeField] private GameObject itemSpotPrefab;
        [Foldout("References"), SerializeField] private Transform parent;
        
        [Foldout("Settings"), SerializeField] private Vector3 initiationPosition;
        [Foldout("Settings"), SerializeField] private float scaleMultiplier;
        [Foldout("Settings"), SerializeField] private float positionXMultiplier;

        #endregion

        #region Private Variables

        private IObjectPool<GameObject> _itemPool;
        private int _defaultCapacity;
        private int _maxSize;
        private int _difficulty;

        #endregion

        #endregion

        #region Singleton

        public static ItemSpotsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            SetDefaults();
            PoolInit(_defaultCapacity, _maxSize);
            CreatePool();
        }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            InputSignals.onItemClicked += OnItemClicked;
        }

        private void OnDisable()
        {
            InputSignals.onItemClicked -= OnItemClicked;
        }

        #endregion
        
        #region Pool Methods
        
        private void SetDefaults()
        {
            _difficulty = PlayerPrefs.GetInt("Difficulty");
            Debug.Log(_difficulty);
            switch (_difficulty)
            {
                case 0 :
                    _defaultCapacity = 7;
                    _maxSize = 10;
                    positionXMultiplier = 0.0975f;
                    scaleMultiplier = 0.65f;
                    break;
                case 1 :
                    _defaultCapacity = 6;
                    _maxSize = 8;
                    positionXMultiplier = 0.115f;
                    scaleMultiplier = 0.75f;
                    break;
                case 2 :
                    _defaultCapacity = 5;
                    _maxSize = 6;
                    positionXMultiplier = 0.15f;
                    scaleMultiplier = 1f;
                    break;
                default:
                    _defaultCapacity = 4;
                    _maxSize = 4;
                    positionXMultiplier = 0.20f;
                    scaleMultiplier = 1.25f;
                    break;
            }
        }

        private void PoolInit(int defaultCapacity, int maxSize)
        {
            _itemPool = new ObjectPool<GameObject>(
                createFunc: CreateItem,
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroyItem,
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
        }

        private GameObject CreateItem()
        {
            GameObject item = Instantiate(itemSpotPrefab);
            item.name = "ItemSpot";
            item.SetActive(false);
            return item;
        }
        
        private void OnGet(GameObject go)
        {
            go.SetActive(true);
        }
        
        private void OnRelease(GameObject go)
        {
            go.SetActive(false);
        }

        private void OnDestroyItem(GameObject go)
        {
            Destroy(go);
        }
        
        private void CreatePool()
        {
            float positionX = 0;
            for (int i = 0; i < _defaultCapacity; i++)
            {
                GameObject item = _itemPool.Get();
                item.transform.SetParent(parent);
                item.transform.localPosition = new Vector3(initiationPosition.x + positionX, initiationPosition.y, initiationPosition.z);
                item.transform.localRotation = Quaternion.Euler(Vector3.zero); 
                item.transform.localRotation = Quaternion.Euler(new Vector3(-6f,0f,0f));
                item.transform.localScale = Vector3.one * scaleMultiplier;
                itemSpots.Add(item);
                positionX += positionXMultiplier;
            }
        }

        #endregion

        #region Custom Methods

        private void OnItemClicked(GameObject item)
        {
            // 1. Turn the item as a child of the item spot
            for (int i = 0; i < itemSpots.Count(); i++)
            {
                if (itemSpots[i].TryGetComponent(out ItemSpotController itemSpotController))
                {
                    if (!itemSpotController.GetIsOccupied())
                    {
                        itemSpotController.SetAsParent(item);
                        itemSpotController.SetIsOccupied(true);
                        break;
                    }
                }
            }

            // 2. Scale the item down, set its local position 0,0,0
            if (item.TryGetComponent(out ItemController itemController))
            {
                float multiplier = 1f;
                switch (itemController.transform.tag)
                {
                    case "RedAmber":
                        multiplier = 0.10f;
                        break;
                    case "MysticLog":
                        multiplier = 0.13f;
                        break;
                    case "RuneStone":
                        multiplier = 0.13f;
                        break;
                }
                itemController.SetTransform(new Vector3(0f, 0.08f, 0f), Vector3.one * multiplier);
            }
        }

        #endregion
    }
}