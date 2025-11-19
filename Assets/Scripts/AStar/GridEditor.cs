using System;
using AStar.GridModel;
using UnityEngine;

namespace AStar
{
    public enum EGridEditorMode
    {
        Noneditable,
        Walkable,
        Unwalkable,
        Start,
        End
    }
    
    public class GridEditor : MonoBehaviour
    {
        private NodeGrid _nodeGrid;
        private Camera _mainCamera;

        public EGridEditorMode EditorMode { get; set; }

        public void InitEditor(NodeGrid nodeGrid, Camera mainCamera)
        {
            _mainCamera = mainCamera;
            _nodeGrid = nodeGrid;
            EditorMode = EGridEditorMode.Noneditable;
        }

        private void Update()
        {
            if (EditorMode == EGridEditorMode.Noneditable) return;
            
            if (_nodeGrid == null) return;
            if (_nodeGrid.Count <= 0) return;
            
            if (!Input.GetMouseButton(0)) return;

            switch (EditorMode)
            {
                case EGridEditorMode.Walkable:
                    SetWalkable();
                    break;
                case EGridEditorMode.Unwalkable:
                    SetUnwalkable();
                    break;
                case EGridEditorMode.Start:
                    SetStartPoint();
                    break;
                case EGridEditorMode.End:
                    SetEndPoint();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool GetGridPoint(out Vector2Int gridPoint)
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.forward, Vector3.zero);
            
            gridPoint = new Vector2Int();

            if (plane.Raycast(ray, out var pointOnRay))
            {
                var hitPoint = ray.GetPoint(pointOnRay);
                
                if (hitPoint.x < -0.5 || hitPoint.x > _nodeGrid.Width - 0.5f) return false;
                if (hitPoint.y < -0.5 || hitPoint.y > _nodeGrid.Height - 0.5f) return false;
                
                gridPoint = new Vector2Int(Mathf.RoundToInt(hitPoint.x), Mathf.RoundToInt(hitPoint.y));
                
                return true;
            }
            return false;
        }
        
        private void SetEndPoint()
        {
            if (GetGridPoint(out var gridPoint))
            {
                _nodeGrid.SetEnd(gridPoint);
            }
        }

        private void SetStartPoint()
        {
            if (GetGridPoint(out var gridPoint))
            {
                _nodeGrid.SetStart(gridPoint);
            }
        }

        private void SetUnwalkable()
        {
            if (GetGridPoint(out var gridPoint))
            {
                _nodeGrid.SetUnwalkable(gridPoint);
            }
        }

        private void SetWalkable()
        {
            if (GetGridPoint(out var gridPoint))
            {
                _nodeGrid.SetWalkable(gridPoint);
            }
        }
    }
}
