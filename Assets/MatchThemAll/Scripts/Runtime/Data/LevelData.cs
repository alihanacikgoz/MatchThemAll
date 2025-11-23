using MatchThemAll.Scripts.Runtime.Enums;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Data
{
    /// <summary>
    /// Bir seviyenin tüm verilerini tutan ScriptableObject.
    /// Unity Editor'da Assets > Create > MatchThemAll > Level Data menüsünden yeni seviyeler oluşturulabilir.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelData", menuName = "MatchThemAll/Level Data", order = 0)]
    public class LevelData : ScriptableObject
    {
        [Header("Level Settings")]
        public int levelIndex;
        public Difficulty levelDifficulty;
        public int itemsToMatchCount;
        public int itemsToMaxCount;
        public float timeLimitInSeconds;
        public int requiredScore;
    }
}