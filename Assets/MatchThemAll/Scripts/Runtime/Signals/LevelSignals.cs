using MatchThemAll.Scripts.Runtime.Data;
using UnityEngine.Events;

namespace MatchThemAll.Scripts.Runtime.Signals
{
    public static class LevelSignals
    {
        public static UnityAction<LevelData> onLevelInitialize;
        public static UnityAction onLevelStart;
        public static UnityAction onLevelSuccess;
        public static UnityAction onLevelFailed;
    }
}