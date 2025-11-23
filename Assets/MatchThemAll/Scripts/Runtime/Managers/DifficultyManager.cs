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
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        public void SetDifficulty(Difficulty diff)
        {
            DifficultySignals.onDifficultyChanged?.Invoke(diff);
        }
    }
}