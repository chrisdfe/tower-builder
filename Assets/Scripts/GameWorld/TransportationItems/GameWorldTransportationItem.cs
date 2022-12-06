using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.TransportationItems;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldTransportationItem : MonoBehaviour
    {
        [HideInInspector]
        public TransportationItem transportationItem;

        List<MeshWrapper> meshWrapperList = new List<MeshWrapper>();

        /*
            Lifecycle Methods
        */
        void Awake()
        {
            transform.localPosition = Vector3.zero;
        }

        void OnDestroy()
        {
            DestroyMesh();
        }

        public void Setup()
        {
            UpdatePosition();
            CreateMesh();
        }

        public void Teardown() { }

        public void Reset() { }

        public void OnBuild() { }

        /* 
            Public Interface
        */
        public void UpdatePosition() { }

        /* 
            Internals
        */
        void CreateMesh()
        {
            TransformUtils.DestroyChildren(transform);

            // MeshWrapperFactory meshWrapperFactory = MeshWrapperKeyMap[transportationItem.key];

            GameWorldTransportationManager transportationManager = GameWorldTransportationManager.Find();
            AssetList<TransportationItem.Key> meshAssetList = transportationManager.meshAssets;

            meshWrapperList = transportationItem.cellCoordinatesList.items.Select((cellCoordinates) =>
            {
                GameObject mesh = meshAssetList.FindByKey(transportationItem.key);

                // MeshWrapper meshWrapper = meshWrapperFactory(assetList.FindByKey(transportationItem.key).transform);
                MeshWrapper meshWrapper = new MeshWrapper(transform, meshAssetList, transportationItem.key);
                meshWrapper.Setup();
                meshWrapper.meshTransform.position = GameWorldUtils.CellCoordinatesToPosition(cellCoordinates, 1f);
                meshWrapper.meshTransform.Translate(new Vector3(0, 0, -2f));

                meshWrapper.SetTileability(cellCoordinates, transportationItem.cellCoordinatesList);
                // TODO here - MaterialReplacer

                return meshWrapper;
            }).ToList();
        }

        void DestroyMesh()
        {
            foreach (MeshWrapper meshWrapper in meshWrapperList)
            {
                meshWrapper.Teardown();
            }
        }
        /* 
            Static API
         */
        public static GameWorldTransportationItem Create(Transform parent)
        {
            GameWorldTransportationManager transportationManager = GameWorldTransportationManager.Find();
            GameObject prefab = transportationManager.prefabAssets.FindByKey(GameWorldTransportationManager.AssetKey.TransportationItem);
            GameObject transportationItemGameObject = Instantiate<GameObject>(prefab);

            transportationItemGameObject.transform.parent = parent;

            GameWorldTransportationItem gameWorldTransportationItem = transportationItemGameObject.GetComponent<GameWorldTransportationItem>();
            return gameWorldTransportationItem;
        }

        /*
            Internal classes
        */
        public class MeshWrapper : MeshWrapper<TransportationItem.Key>
        {
            public MeshWrapper(Transform parent, AssetList<TransportationItem.Key> assetList, TransportationItem.Key key)
                : base(parent, assetList, key) { }
        }
    }
}