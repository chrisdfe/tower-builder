using TowerBuilder.DataTypes;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    public class MeshWrapper<KeyType>
        where KeyType : struct
    {
        public const string TILEABLE_WRAPPER_NODE_NAME = "TileableWrapper";

        public virtual KeyType key { get; }
        public AssetList<KeyType> assetList;

        public GameObject prefabMesh { get; private set; }
        public Transform meshTransform { get; private set; }
        public Transform parent { get; private set; }

        Transform tileabilityWrapper;
        CellCoordinates cellCoordinates;
        CellNeighbors cellNeighbors;
        Tileable.CellPosition cellPosition;

        public MeshWrapper(Transform parent, GameObject prefabMesh, CellCoordinates cellCoordinates, CellCoordinatesList cellCoordinatesList)
        {
            this.parent = parent;
            this.key = key;

            this.prefabMesh = prefabMesh;

            this.cellNeighbors = CellNeighbors.FromCellCoordinatesList(cellCoordinates, cellCoordinatesList);
            Debug.Log("cellNeighbors: " + cellNeighbors);

            if (this.cellNeighbors.occupied == CellOrientation.AboveRight)
            {
                Debug.Log("yes, Above right");
            }

            this.cellPosition = Tileable.GetCellPosition(this.cellNeighbors);
        }

        public virtual void Setup()
        {
            InstantiateModel();
            ProcessModel();
        }

        public virtual void Teardown()
        {
            GameObject.Destroy(meshTransform.gameObject);
        }

        protected void InstantiateModel()
        {
            Transform meshTransform = GameObject.Instantiate(prefabMesh).GetComponent<Transform>();
            meshTransform.SetParent(parent);
            meshTransform.localPosition = Vector3.zero;

            // TODO here - use this.cellPosition to position meshTransform?

            this.meshTransform = meshTransform;
        }

        protected void ProcessModel()
        {
            Transform tileabileWrapper = TransformUtils.FindDeepChild(meshTransform, TILEABLE_WRAPPER_NODE_NAME);

            if (tileabileWrapper == null)
            {
                Debug.Log("No TileableWrapper found");
                return;
            }

            Transform child = tileabileWrapper.GetChild(0);

            if (child != null)
            {
                Tileable.CellPosition cellPosition = Tileable.GetCellPosition(cellNeighbors);
                Debug.Log("cellPosition: " + cellPosition);

                foreach (Transform node in child)
                {
                    Tileable.CellPosition nodeCellPosition = Tileable.CellPositionLabelMap.KeyFromValue(node.name);
                    node.localPosition = Vector3.zero;
                    node.gameObject.SetActive(nodeCellPosition == cellPosition);
                }
            }
        }
    }
}