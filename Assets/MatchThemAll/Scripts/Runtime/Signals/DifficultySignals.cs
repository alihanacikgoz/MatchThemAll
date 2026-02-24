using MatchThemAll.Scripts.Runtime.Enums;
using UnityEngine.Events;

namespace MatchThemAll.Scripts.Runtime.Signals
{
    public static class DifficultySignals
    {
        public static UnityAction<Difficulty> onDifficultyChanged;
    }
}