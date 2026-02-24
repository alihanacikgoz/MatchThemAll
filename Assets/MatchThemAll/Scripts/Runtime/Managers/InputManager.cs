using MatchThemAll.Scripts.Runtime.Controllers;
using MatchThemAll.Scripts.Runtime.Signals;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


namespace MatchThemAll.Scripts.Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
    
        private Camera _camera;
        private RaycastHit _targetItem;
        private GameObject _selectedItem;
        
        #region Subscriber Methods

        private void Subscribers()
        {
            EnhancedTouchSupport.Enable();
        }
        
        private void Unsubscribers()
        {
            EnhancedTouchSupport.Disable();
        }

        #endregion
        

        #region Unity Methods
        
        private void OnEnable()
        {
            Subscribers();
        }

        void Start()
        {
            _camera = Camera.main;
        }
    
        void Update()
        {
            if (Touch.activeTouches[0].inProgress)
            {
                HandleDrag();
            } else if (Touch.activeTouches[0].phase == TouchPhase.Ended || Touch.activeTouches[0].phase == TouchPhase.Canceled)
            {
                HandleMouseUp();
            }
        }
        
        private void OnDisable()
        {
            Unsubscribers();
        }

        #endregion

        

        private void HandleDrag()
        {
            Ray ray = _camera.ScreenPointToRay(Touch.activeFingers[0].screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit,100,layerMask))
            {
                _targetItem = hit;
            }
            
            _targetItem.collider.TryGetComponent(out ItemController itemController);
            
            
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
            _selectedItem.TryGetComponent(out ItemController itemController);
            itemController.OnItemDeselected();
            InputSignals.onItemClicked?.Invoke(_selectedItem);
            _selectedItem = null;
        }
    }
}
