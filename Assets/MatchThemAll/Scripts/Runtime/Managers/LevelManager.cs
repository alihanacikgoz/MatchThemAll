using System;
using System.Collections.Generic;
using System.Linq;
using MatchThemAll.Scripts.Runtime.Data;
using MatchThemAll.Scripts.Runtime.Enums;
using MatchThemAll.Scripts.Runtime.Signals;
using NaughtyAttributes;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Singleton

        public static LevelManager Instance { get; private set; }

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

        #region SerializeField Variables

        [Tooltip("Oyunun tüm seviye verilerini buraya ekleyin."),Foldout("Level Data"), SerializeField] public List<LevelData> levels;

        #endregion

        #region Private Variables

        private int _currentLevelIndex = 0;
        private LevelData _currentLevelData;

        #endregion

        #endregion

        #region Unity Methods

        private void Start()
        {
            _currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
            LoadLevel(_currentLevelIndex);
        }

        #endregion

        #region Custom Methods

        public void LoadLevel(int levelIndex)
        {
            _currentLevelIndex = levelIndex;

            if (_currentLevelIndex >= levels.Count)
            {
                _currentLevelIndex = 0;
            }

            _currentLevelData = levels.FirstOrDefault(l => l.levelIndex == _currentLevelIndex);

            if (_currentLevelData != null)
            {
                Debug.Log($"Level {_currentLevelData.levelIndex} yükleniyor... Eşleştirilecek öğe sayısı: {_currentLevelData.itemsToMatchCount}");
                PlayerPrefs.SetInt("CurrentLevelIndex", _currentLevelIndex);
                DifficultyManager.Instance.SetDifficulty(_currentLevelData.levelDifficulty);
                
                LevelSignals.onLevelInitialize?.Invoke(_currentLevelData);
            }
            else
            {
                Debug.LogError($"Seviye verisi bulunamadı! Index: {levelIndex}. 'levels' listesini kontrol edin.");
            }
        }

        public void LoadNextLevel()
        {
            LoadLevel(_currentLevelIndex +1);
        }

        public LevelData GetCurrentLevel()
        {
            LevelData levelData = levels[_currentLevelIndex];
            return levelData;
        }

        #endregion
    }
}