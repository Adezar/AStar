using AStar;
using AStar.GridModel;
using AStar.Views;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Main components")]
    [SerializeField] private PathFindingController pathFindingController;
    [SerializeField] private GridRenderer gridRenderer;
    [SerializeField] private GridEditor gridEditor;
        
    [Header("Views")]
    [SerializeField] private CreateGridPanelView createGridPanelView;
    [SerializeField] private EditPanelView editPanelView;
    [SerializeField] private MainPanelView mainPanelView;
    [SerializeField] private FindPathPanelView findPathPanelView;
        
    [Header("Common")]
    [SerializeField] private Camera mainCamera;
        
    private NodeGrid _nodeGrid;
    private AStarAlgorithm _aStarAlgorithm;
        
    private void Awake()
    {
        _nodeGrid = new NodeGrid();
        _aStarAlgorithm = new AStarAlgorithm(_nodeGrid);
            
        pathFindingController.InitController(createGridPanelView, editPanelView, mainPanelView, findPathPanelView,
            _nodeGrid, _aStarAlgorithm, gridEditor, mainCamera);
            
        gridRenderer.InitRenderer(_nodeGrid);
        
        gridEditor.InitEditor(_nodeGrid, mainCamera);
    }
}