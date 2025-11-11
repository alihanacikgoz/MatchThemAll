using System;
using MatchThemAll.Scripts.Runtime.Enums;
using MatchThemAll.Scripts.Runtime.Signals;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Managers
{
    public class GameManager : MonoBehaviour
    {

        #region Singleton

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance!=null && Instance!=this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            DifficultySignals.onDifficultyChanged += OnDifficultyChanged;
        }

        private void OnDisable()
        {
            DifficultySignals.onDifficultyChanged -= OnDifficultyChanged;
        }

        #endregion
        

        private void OnDifficultyChanged(Difficulty difficulty)
        {
            PlayerPrefs.SetInt("Difficulty", (int) difficulty);
        }
    }
}