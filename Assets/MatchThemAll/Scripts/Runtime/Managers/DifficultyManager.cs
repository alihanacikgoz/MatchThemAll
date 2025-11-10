using System;
using MatchThemAll.Scripts.Runtime.Enums;
using MatchThemAll.Scripts.Runtime.Signals;
using NaughtyAttributes;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Managers
{
    public class DifficultyManager : MonoBehaviour
    {
        #region Variables

        #region SerializeField Variables

        [Foldout("Settings"), SerializeField] private Difficulty difficulty;

        #endregion

        #endregion
        
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

        private DifficultyManager()
        {
            SetDifficulty();
        }
        
        public void SetDifficulty()
        {
            DifficultySignals.onDifficultyChanged?.Invoke(difficulty);
        }
        
        
    }
}