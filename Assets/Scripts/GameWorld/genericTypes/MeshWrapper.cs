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

        public Transform meshTransform { get; private set; }
        public Transform parent { get; private set; }

        Tileable tileable;
        Transform tileabilityWrapper;

        public MeshWrapper(Transform parent, AssetList<KeyType> assetList, KeyType key)
        {
            this.parent = parent;
            this.assetList = assetList;
            this.key = key;
        }

        public virtual void Setup()
        {
            LoadModel();
        }

        public virtual void Teardown()
        {
            GameObject.Destroy(meshTransform.gameObject);
        }

        public void SetTileability(CellCoordinates itemCoordinates, CellCoordinatesList cellCoordinatesList)
        {
            CellNeighbors cellNeighbors = CellNeighbors.FromCellCoordinatesList(itemCoordinates, cellCoordinatesList);
            ProcessModel(meshTransform, cellNeighbors);
        }

        protected void LoadModel()
        {
            GameObject mesh = assetList.FindByKey(key);

            Transform meshTransform = GameObject.Instantiate(mesh).GetComponent<Transform>();
            meshTransform.SetParent(parent);
            meshTransform.localPosition = Vector3.zero;

            this.meshTransform = meshTransform;
        }

        protected void ProcessModel(Transform model, CellNeighbors cellNeighbors)
        {
            Transform tileabileWrapper = TransformUtils.FindDeepChild(model, TILEABLE_WRAPPER_NODE_NAME);

            if (tileabileWrapper == null)
            {
                Debug.Log("No TileableWrapper found");
                return;
            }

            Transform child = tileabileWrapper.GetChild(0);

            if (child != null)
            {
                Tileable.CellPosition cellPosition = Tileable.GetCellPosition(cellNeighbors);

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