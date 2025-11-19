using System.Threading;
using AStar.GridModel;
using AStar.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AStar
{
    public class PathFindingController : MonoBehaviour
    {
        private CreateGridPanelView _createGridPanelView;
        private EditPanelView _editPanelView;
        private MainPanelView _mainPanelView;
        private FindPathPanelView _findPathPanelView;
        
        private GridEditor _gridEditor;
        private GridRenderer _gridRenderer;

        private NodeGrid _nodeGrid;
        private AStarAlgorithm _aStarAlgorithm;
        
        private Camera _mainCamera;
        
        
        private bool _isEightDirections;

        private CancellationTokenSource _cancellationFindPathTokenSource;


        public void InitController(
            CreateGridPanelView createGridPanelView, EditPanelView editPanelView, MainPanelView mainPanelView, FindPathPanelView findPathPanelView,
            NodeGrid nodeGrid, AStarAlgorithm aStarAlgorithm, GridEditor gridEditor, 
            Camera mainCamera)
        {
            _createGridPanelView = createGridPanelView;
            _editPanelView = editPanelView;
            _mainPanelView = mainPanelView;
            _findPathPanelView = findPathPanelView;
            
            _mainCamera = mainCamera;
            
            _gridEditor = gridEditor;
            _nodeGrid = nodeGrid;
            _aStarAlgorithm = aStarAlgorithm;

            ShowCreateGridPanel();
        }
        
        private void ShowCreateGridPanel()
        {
            _createGridPanelView.gameObject.SetActive(true);
            _editPanelView.gameObject.SetActive(false);
            _mainPanelView.gameObject.SetActive(false);
            _findPathPanelView.gameObject.SetActive(false);
            
            _nodeGrid.ClearGrid();
        }
        
        private void ShowEditPanel()
        {
            _createGridPanelView.gameObject.SetActive(false);
            _editPanelView.gameObject.SetActive(true);
            _mainPanelView.gameObject.SetActive(false);
            _findPathPanelView.gameObject.SetActive(false);

            _gridEditor.EditorMode = EGridEditorMode.Start;

            _nodeGrid.ClearPathFlags();
        }
        
        private void ShowMainPanel()
        {
            _createGridPanelView.gameObject.SetActive(false);
            _editPanelView.gameObject.SetActive(false);
            _mainPanelView.gameObject.SetActive(true);
            _findPathPanelView.gameObject.SetActive(false);
            
            _isEightDirections = false;
        }
        
        private void ShowFindPathPanel()
        {
            _createGridPanelView.gameObject.SetActive(false);
            _editPanelView.gameObject.SetActive(false);
            _mainPanelView.gameObject.SetActive(false);
            _findPathPanelView.gameObject.SetActive(true);
        }
        
        private void CreateGrid(int width, int height)
        {
            _nodeGrid.NewNodeGrid(width, height);
            
            ResetCamToGridCenter();

            ShowEditPanel();
        }

        private void ResetCamToGridCenter()
        {
            var gridBound = Mathf.Max(_nodeGrid.Width, _nodeGrid.Height, 0) * 1.5f;
            var halfFov = _mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad;
            var distance = gridBound * 0.5f / Mathf.Tan(halfFov);

            _mainCamera.transform.position = new Vector3(
                (_nodeGrid.Width-1f) * 0.5f,
                (_nodeGrid.Height-1f) * 0.5f,
                - distance
            );
        }

        private void ChangeEditMode(EGridEditorMode editorMode)
        {
            _gridEditor.EditorMode = editorMode;
        }

        private void FinishEdit()
        {
            _gridEditor.EditorMode = EGridEditorMode.Noneditable;
            
            ShowMainPanel();
        }
        
        private void FindPath()
        {
            if (_cancellationFindPathTokenSource != null)
                _cancellationFindPathTokenSource.Cancel();
            
            _cancellationFindPathTokenSource = new CancellationTokenSource();
            
            _aStarAlgorithm.FindPath(_isEightDirections, _cancellationFindPathTokenSource.Token, FindPathFinished).Forget();
            
            ShowFindPathPanel();
        }

        private void FindPathFinished()
        {
            ShowMainPanel();
        }

        private void ChooseEightDirections(bool isEightDirections)
        {
            _isEightDirections = isEightDirections;
        }
        
        private void StopFindPath()
        {
            if (_cancellationFindPathTokenSource != null)
                _cancellationFindPathTokenSource.Cancel();
            
            ShowMainPanel();
        }
        

        private void OnEnable()
        {
            _createGridPanelView.CreateGridRequested += CreateGrid;
            _editPanelView.ChangeEditModeRequested += ChangeEditMode;
            _editPanelView.FinishEditRequested += FinishEdit;
            
            _mainPanelView.EditGridRequested += ShowEditPanel;
            _mainPanelView.FindPathRequested += FindPath;
            _mainPanelView.NewGridRequested += ShowCreateGridPanel;
            
            _findPathPanelView.StopRequested += StopFindPath;
            
            _mainPanelView.ChooseEightDirectionsRequested += ChooseEightDirections;
        }

        private void OnDisable()
        {
            _createGridPanelView.CreateGridRequested -= CreateGrid;
            _editPanelView.ChangeEditModeRequested -= ChangeEditMode;
            _editPanelView.FinishEditRequested -= FinishEdit;
            
            _mainPanelView.EditGridRequested -= ShowEditPanel;
            _mainPanelView.FindPathRequested -= FindPath;
            _mainPanelView.NewGridRequested -= ShowCreateGridPanel;
            
            _findPathPanelView.StopRequested -= StopFindPath;
            
            _mainPanelView.ChooseEightDirectionsRequested -= ChooseEightDirections;
        }
    }
}

