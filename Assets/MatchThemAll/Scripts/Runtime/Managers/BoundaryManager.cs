using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;

namespace MatchThemAll.Scripts.Runtime.Managers
{
    public class BoundaryManager : MonoBehaviour
    {
        #region Variables

        #region SerializeField Variables

        [Foldout("References"), SerializeField]
        private Camera mainCamera;

        [Foldout("References"), SerializeField]
        private List<GameObject> boundaryWalls;

        [Foldout("Settings"), SerializeField] private float distanceMultiplier;
        [Foldout("Settings"), SerializeField] private float wallThickness;
        [Foldout("Settings"), SerializeField] private float wallHeight;
        [Foldout("Settings"), SerializeField] private bool autoSetup = true;

        #endregion

        #region Private Variables

        private float _distance;
        private Vector3 _direction;

        private Vector3 _bottomLeftWorld;
        private Vector3 _bottomRightWorld;
        private Vector3 _topLeftWorld;
        private Vector3 _topRightWorld;

        private Transform _groundTransform;

        #endregion

        #endregion

        #region Singleton

        public static BoundaryManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (autoSetup)
            {
                CalculateScreenBoundaries();
                SetBoundaryWallPosition();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 lineDirection = _direction * distanceMultiplier;
            Gizmos.DrawLine(mainCamera.transform.position, lineDirection);
        }

        #endregion

        #region Custom Methods

        private Vector3 ScreenToWorldPointOnGround(Vector3 screenPoint, float groundY)
        {
            Ray ray = mainCamera.ScreenPointToRay(screenPoint);

            float t = (groundY - ray.origin.y) / ray.direction.y;
            Vector3 worldPoint = ray.origin + ray.direction * t;

            _direction = mainCamera.transform.TransformDirection(Vector3.forward);
            Physics.Raycast(mainCamera.transform.position, _direction, out RaycastHit hit, distanceMultiplier);
            _distance = Vector3.Distance(hit.point, mainCamera.transform.position);
            _groundTransform = hit.transform;

            return worldPoint;
        }


        private void CalculateScreenBoundaries()
        {
            float groundY = _groundTransform != null ? _groundTransform.position.y : 0;

            _bottomLeftWorld = ScreenToWorldPointOnGround(new Vector3(0, 0, 0), groundY);
            _bottomRightWorld = ScreenToWorldPointOnGround(new Vector3(Screen.width, 0, 0), groundY);
            _topLeftWorld = ScreenToWorldPointOnGround(new Vector3(0, Screen.height, 0), groundY);
            _topRightWorld = ScreenToWorldPointOnGround(new Vector3(Screen.width, Screen.height, 0), groundY);
        }

        private void SetBoundaryWallPosition()
        {
            if (boundaryWalls == null || boundaryWalls.Count < 4) return;

            float groundY = _groundTransform != null ? _groundTransform.position.y : 0f;

            GameObject topWall = boundaryWalls[0];
            Vector3 topCenter = (_topLeftWorld + _topRightWorld) / 2f;
            float topWidth = Vector3.Distance(_topLeftWorld, _topRightWorld);
            SetupWall(topWall, topCenter, topWidth, wallThickness, wallHeight, groundY, -1);

            GameObject bottomWall = boundaryWalls[1];
            Vector3 bottomCenter = (_bottomLeftWorld + _bottomRightWorld) / 2f;
            float bottomWidth = Vector3.Distance(_bottomLeftWorld, _bottomRightWorld);
            SetupWall(bottomWall, bottomCenter, bottomWidth, wallThickness, wallHeight, groundY,1.3f);

            GameObject leftWall = boundaryWalls[2];
            Vector3 leftCenter = (_topLeftWorld + _bottomLeftWorld) / 2f;
            float leftHeight = Vector3.Distance(_topLeftWorld, _bottomLeftWorld);
            SetupWall(leftWall, leftCenter, wallThickness, leftHeight, wallHeight, groundY);

            GameObject rightWall = boundaryWalls[3];
            Vector3 rightCenter = (_topRightWorld + _bottomRightWorld) / 2f;
            float rightHeight = Vector3.Distance(_topRightWorld, _bottomRightWorld);
            SetupWall(rightWall, rightCenter, wallThickness, rightHeight, wallHeight, groundY);

            GameObject centerTopCeiling = boundaryWalls[4];
            Vector3 centerTopCenter = mainCamera.transform.position + Vector3.up * 3f;
            //Vector3 centerTopCenter = ((topCenter + bottomCenter) / 2f + (leftCenter + rightCenter) / 2f);
            //float height = mainCamera.transform.position.z + 3f;
            SetupCeiling(centerTopCeiling, centerTopCenter, 30f, 1f, 30f);
            
        }

        private void SetupWall(GameObject wall, Vector3 position, float width, float depth, float height, float groundY, float offset = 0f)
        {
            if (wall == null) return;

            position.y = groundY + (height / 2f);
            position.z += offset;
            wall.transform.position = position;

            BoxCollider boxCollider = wall.GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                boxCollider = wall.AddComponent<BoxCollider>();
            }

            boxCollider.size = new Vector3(width, height, depth);
        }

        private void SetupCeiling(GameObject ceiling, Vector3 position, float width, float depth, float height)
        {
            if (!ceiling) return;
            
            ceiling.transform.position = position;
            
            ceiling.TryGetComponent(out BoxCollider boxCollider);
            if (boxCollider == null)
            {
                boxCollider = ceiling.AddComponent<BoxCollider>();
            }
            
            boxCollider.size = new Vector3(width, height, depth);
        }

        #endregion
    }
}