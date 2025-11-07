using UnityEngine;
using UnityEngine.Events;

namespace MatchThemAll.Scripts.Runtime.Signals
{
    public static class InputSignals
    {
        public static UnityAction<GameObject> onItemClicked;
        public static UnityAction<GameObject> onItemSelected;
        public static UnityAction<GameObject> onItemDeselected;
    }
}