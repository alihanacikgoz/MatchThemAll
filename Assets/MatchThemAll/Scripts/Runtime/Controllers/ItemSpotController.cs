using NaughtyAttributes;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Controllers
{
    public class ItemSpotController : MonoBehaviour
    {
        #region Variables

        #region SerializeFieldVariables

        [Foldout("Checkings"), SerializeField] public bool isOccupied;

        #endregion

        #endregion
        
        public void SetIsOccupied(bool occupation)
        {
            isOccupied = occupation;
        }

        public bool GetIsOccupied()
        {
            return isOccupied;
        }

        public void SetAsParent(GameObject child)
        {
            child.transform.SetParent(transform);
        }
    }
}