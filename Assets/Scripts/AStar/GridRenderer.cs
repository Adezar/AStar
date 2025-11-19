using AStar.GridModel;
using UnityEngine;

namespace AStar
{
    public class GridRenderer : MonoBehaviour
    {
        [SerializeField] private Shader shader;

        private readonly int _colorId = Shader.PropertyToID("_Color");
        
        private NodeGrid _nodeGrid;
        private Mesh _cellMesh;
        private Material _cellMaterial;

        public void InitRenderer(NodeGrid nodeGrid)
        {
            CreateCellMesh();
            _nodeGrid = nodeGrid;
        }

        private void Update()
        {
            if (_nodeGrid == null) return;
            if (_nodeGrid.Count <= 0) return;

            RenderGrid();
        }

        private void CreateCellMesh()
        {
            _cellMesh = new Mesh();
            
            var pivot = new Vector3(0.4f, 0.4f, 0f);

            _cellMesh.vertices = new Vector3[]
            {
                new Vector3(0,    0,    0) - pivot,
                new Vector3(0.8f, 0,    0) - pivot,
                new Vector3(0.8f, 0.8f, 0) - pivot,
                new Vector3(0,    0.8f, 0) - pivot
            };
            
            _cellMesh.triangles = new int[]
            {
                0, 3, 2,
                2, 1, 0
            };
            
            _cellMesh.RecalculateNormals();
            _cellMesh.UploadMeshData(false);
            _cellMaterial = new Material(shader);
        }

        private void RenderGrid()
        {
            for (int x = 0; x < _nodeGrid.Width; x++)
            {
                for (int y = 0; y < _nodeGrid.Height; y++)
                {
                    var propertyBlock = new MaterialPropertyBlock();

                    if (!_nodeGrid[x, y].Walkable)
                    {
                        propertyBlock.SetColor(_colorId, new Color32(0x15, 0x15, 0x15, 0xFF));
                    }
                    else
                    {
                        propertyBlock.SetColor(_colorId, Color.white);
                    }

                    if (_nodeGrid[x, y].Start)
                    {
                        propertyBlock.SetColor(_colorId, Color.green);
                    }
                    else if (_nodeGrid[x, y].End)
                    {
                        propertyBlock.SetColor(_colorId, Color.red);
                    }

                    if (_nodeGrid[x, y].OpenSet && !_nodeGrid[x, y].Start && !_nodeGrid[x, y].End)
                        propertyBlock.SetColor(_colorId, new Color32(0xFF, 0x9E, 0x00, 0xFF));

                    if (_nodeGrid[x, y].ClosedSet && !_nodeGrid[x, y].Start && !_nodeGrid[x, y].End)
                        propertyBlock.SetColor(_colorId, new Color32(0xB8, 0x71, 0x00, 0xFF));

                    if (_nodeGrid[x, y].FinalPath && !_nodeGrid[x, y].Start && !_nodeGrid[x, y].End)
                        propertyBlock.SetColor(_colorId, Color.magenta);

                    Graphics.DrawMesh(_cellMesh, new Vector3(x, y, 0), Quaternion.identity, _cellMaterial, 0, null, 0,
                        propertyBlock);
                }
            }

        }
    }
}
