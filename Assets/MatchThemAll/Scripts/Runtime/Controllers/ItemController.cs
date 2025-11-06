using System;
using MatchThemAll.Scripts.Runtime.Signals;
using NaughtyAttributes;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Controllers
{
    public class ItemController : MonoBehaviour
    {
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
            DisableShadows(item);
            DisablePhysics(item);
        }

        public void DisableShadows(GameObject item)
        {
            if (!item) return;
            var meshRenderer = item.GetComponentInChildren<MeshRenderer>();
            if (!meshRenderer) return;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        public void DisablePhysics(GameObject item)
        {
            if (item.TryGetComponent(out Rigidbody rb))
                rb.isKinematic = true;
        }

        public void SetTransform(Vector3 position, Vector3 scale)
        {
            transform.localPosition = position;
            transform.localScale = scale;
        }

        #endregion
    }
}