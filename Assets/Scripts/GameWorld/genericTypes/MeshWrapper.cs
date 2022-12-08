using TowerBuilder.DataTypes;
using UnityEngine;

namespace TowerBuilder.GameWorld
{
    public class MeshWrapper<KeyType>
        where KeyType : struct
    {
        public virtual KeyType key { get; }
        public AssetList<KeyType> assetList;

        public Transform meshTransform { get; private set; }
        // protected Transform wrapper;
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
            OccupiedCellMap occupiedCellMap = OccupiedCellMap.FromCellCoordinatesList(itemCoordinates, cellCoordinatesList);

            tileable = Tileable.FromModel(meshTransform);

            if (tileable != null)
            {
                tileable.ProcessModel(meshTransform, occupiedCellMap);
            }
        }

        protected void LoadModel()
        {
            GameObject mesh = assetList.FindByKey(key);

            Transform meshTransform = GameObject.Instantiate(mesh).GetComponent<Transform>();
            meshTransform.SetParent(parent);
            meshTransform.localPosition = Vector3.zero;

            this.meshTransform = meshTransform;
        }
    }
}