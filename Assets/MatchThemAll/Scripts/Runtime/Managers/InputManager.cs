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
            
            _targetItem.collider.TryGetComponent(out ItemController itemController);
            //Debug.Log($"Selected item : {_targetItem.collider.gameObject.name}");
            
            if(!_targetItem.collider)
            {
                DeselectItem();
                return;
            }
            if (!itemController)
            {
                DeselectItem();
                return;
            }
            DeselectItem();
            
            itemController.OnItemSelected();
            
            _selectedItem = _targetItem.collider.gameObject;
        }

        private void DeselectItem()
        {
            if (!_selectedItem) return;
            _selectedItem.TryGetComponent(out ItemController selectedItemController);
            selectedItemController.OnItemDeselected();
            _selectedItem = null;
        }

        private void HandleMouseUp()
        {
            if (!_selectedItem) return;
            InputSignals.onItemClicked?.Invoke(_selectedItem);
            _selectedItem.TryGetComponent(out ItemController itemController);
            itemController.OnItemDeselected();
            _selectedItem = null;
        }
    }
}
