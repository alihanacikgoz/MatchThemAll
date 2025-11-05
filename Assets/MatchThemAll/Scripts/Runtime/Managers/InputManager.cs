using MatchThemAll.Scripts.Runtime.Controllers;
using MatchThemAll.Scripts.Runtime.Signals;
using UnityEngine;

namespace MatchThemAll.Scripts.Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
    
        private Camera _camera;
        private RaycastHit _selectedItem;
    
        void Start()
        {
            _camera = Camera.main;
        }
    
        void Update()
        {
            if (Input.GetMouseButtonDown(0)) HandleClick();
        }

        private void HandleClick()
        {
            
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit,100,layerMask))
            {
                _selectedItem = hit;
            }
            Debug.Log("Clicked on : " + _selectedItem.collider.gameObject.name);
        
            if(!_selectedItem.collider)return;
            if (!_selectedItem.collider.TryGetComponent(out ItemController itemController)) return;

            
            
            InputSignals.onItemClicked?.Invoke(_selectedItem.collider.gameObject);
        }
    }
}
