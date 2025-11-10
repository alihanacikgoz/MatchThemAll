using MatchThemAll.Scripts.Runtime.Enums;
using UnityEngine.Events;

namespace MatchThemAll.Scripts.Runtime.Signals
{
    public class DifficultySignals
    {
        public static UnityAction<Difficulty> onDifficultyChanged;
    }
}