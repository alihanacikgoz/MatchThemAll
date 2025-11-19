using System;
using MatchThemAll.Scripts.Runtime.Signals;
using NaughtyAttributes;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Controllers
{
    public class ItemController : MonoBehaviour
    {
        #region Variables

        [Foldout("References"), SerializeField] private Material baseMaterial;
        [Foldout("References"), SerializeField] private Material outlineMaterial;
        [Foldout("References"), SerializeField] private Renderer rendererComp;
        
        #endregion
        
        #region Unity Methods

        private void Awake()
        {
            baseMaterial = rendererComp.material;
            rendererComp.materials = new Material[] {baseMaterial};
        }

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

        private void DisableShadows(GameObject item)
        {
            if (!item) return;
            var meshRenderer = item.GetComponentInChildren<MeshRenderer>();
            if (!meshRenderer) return;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        private void DisablePhysics(GameObject item)
        {
            if (item.TryGetComponent(out Rigidbody rb))
                rb.isKinematic = true;
            if (item.TryGetComponent(out Collider colliderComponent))
                colliderComponent.enabled = false;
        }

        public void SetTransform(Vector3 position, Vector3 scale)
        {
            transform.localPosition = position;
            transform.localScale = scale;
        }
        
        public void OnItemSelected()
        {
            rendererComp.materials = new Material[] {baseMaterial, outlineMaterial };
        }
        
        public void OnItemDeselected()
        {
            rendererComp.materials = new Material[] {baseMaterial };
        }
        #endregion
    }
}