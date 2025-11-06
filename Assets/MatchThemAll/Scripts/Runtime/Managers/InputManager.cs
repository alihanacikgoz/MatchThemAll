using MatchThemAll.Scripts.Runtime.Controllers;
using MatchThemAll.Scripts.Runtime.Signals;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
    
        private Camera _camera;
        private RaycastHit _targetItem;
        private GameObject _selectedItem;
    
        void Start()
        {
            _camera = Camera.main;
        }
    
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                HandleDrag();
            } else if (Input.GetMouseButtonUp(0))
            {
                HandleMouseUp();
            }
        }

        private void HandleDrag()
        {
            
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit,100,layerMask))
            {
                _targetItem = hit;
            }
            Debug.Log("Clicked on : " + _targetItem.collider.gameObject.name);
        
            if(!_targetItem.collider)
            {
                _selectedItem = null;
                return;
            }
            if (!_targetItem.collider.TryGetComponent(out ItemController itemController))
            {
                _selectedItem = null;
                return;
            }
            
            _selectedItem = _targetItem.collider.gameObject;
        }
        
        private void HandleMouseUp()
        {
            if (!_selectedItem) return;
            InputSignals.onItemClicked?.Invoke(_selectedItem);
            _selectedItem = null;
        }
    }
}
