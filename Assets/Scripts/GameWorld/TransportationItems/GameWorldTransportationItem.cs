using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldTransportationItem : MonoBehaviour
    {
        [HideInInspector]
        public TransportationItem transportationItem;

        List<MeshWrapper<TransportationItem.Key>> meshWrapperList = new List<MeshWrapper<TransportationItem.Key>>();

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

            GameWorldTransportationManager transportationManager = GameWorldTransportationManager.Find();

            GameObject prefabMesh = transportationManager.meshAssets.FindByKey(transportationItem.key);

            meshWrapperList = transportationItem.cellCoordinatesList.items.Select((cellCoordinates) =>
            {
                MeshWrapper<TransportationItem.Key> meshWrapper =
                     new MeshWrapper<TransportationItem.Key>(transform, prefabMesh, cellCoordinates, transportationItem.cellCoordinatesList);

                meshWrapper.Setup();

                // TODO here - use entity layer instead
                meshWrapper.meshTransform.position = GameWorldUtils.CellCoordinatesToPosition(cellCoordinates, 1f);
                meshWrapper.meshTransform.Translate(new Vector3(0, 0, -2f));

                return meshWrapper;
            }).ToList();
        }

        void DestroyMesh()
        {
            foreach (MeshWrapper<TransportationItem.Key> meshWrapper in meshWrapperList)
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
    }
}