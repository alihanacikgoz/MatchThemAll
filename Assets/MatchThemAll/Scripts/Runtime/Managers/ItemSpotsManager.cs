using System;
using MatchThemAll.Scripts.Runtime.Controllers;
using MatchThemAll.Scripts.Runtime.Signals;
using NaughtyAttributes;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Managers
{
    public class ItemSpotsManager : MonoBehaviour
    {
        #region Variables

        #region SerializeField Variables

        [Foldout("Elements"), SerializeField] private Transform[] itemSpots;

        #endregion

        #endregion

        #region Singleton

        public static ItemSpotsManager Instance;

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

        #region Custom Methods

        private void OnItemClicked(GameObject item)
        {
            // 1. Turn the item as a child of the item spot
            for (int i = 0; i < itemSpots.Length; i++)
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
                itemController.SetTransform(new Vector3(0f, 0.08f, 0f), Vector3.one * 0.13f);
            }
        }

        #endregion
    }
}