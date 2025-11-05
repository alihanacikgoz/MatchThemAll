using System;
using MatchThemAll.Scripts.Runtime.Signals;
using NaughtyAttributes;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Controllers
{
    public class ItemController : MonoBehaviour
    {

        #region Variables

        #region SerializeField Variables
        
        [Foldout("References"), SerializeField] private MeshRenderer meshRenderer;
        
        #endregion

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            InputSignals.onItemClicked += OnItemClicked;
        }
        
        private void OnDisable()
        {
            InputSignals.onItemClicked -= OnItemClicked;
        }

        #endregion

        #region Custom Methods

        private void OnItemClicked(GameObject item)
        {
            DisableShadows();
            DisablePhysics();
        }

        public void DisableShadows()
        {
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        public void DisablePhysics()
        {
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }
        }

        public void SetTransform(Vector3 position, Vector3 scale)
        {
            transform.localPosition = position;
            transform.localScale = scale;
        }

        #endregion
        
    }
}