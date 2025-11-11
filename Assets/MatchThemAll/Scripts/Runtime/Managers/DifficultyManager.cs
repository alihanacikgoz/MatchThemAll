using System;
using MatchThemAll.Scripts.Runtime.Enums;
using MatchThemAll.Scripts.Runtime.Signals;
using NaughtyAttributes;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Managers
{
    public class DifficultyManager : MonoBehaviour
    {
        
        #region Singleton

        public static DifficultyManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        #endregion
        
        [Button]
        public void SetDifficultyEasy()
        {
            Difficulty diff = Difficulty.Easy;
            DifficultySignals.onDifficultyChanged?.Invoke(diff);
        }
        
        [Button]
        public void SetDifficultyMedium()
        {
            Difficulty diff = Difficulty.Medium;
            DifficultySignals.onDifficultyChanged?.Invoke(diff);
        }
        
        [Button]
        public void SetDifficultyHard()
        {
            Difficulty diff = Difficulty.Hard;
            DifficultySignals.onDifficultyChanged?.Invoke(diff);
        }
        
        
    }
}