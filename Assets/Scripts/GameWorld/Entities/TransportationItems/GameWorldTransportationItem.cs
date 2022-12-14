using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.GameWorld.Entities;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities.TransportationItems
{
    [RequireComponent(typeof(EntityMeshWrapper))]
    public class GameWorldTransportationItem : MonoBehaviour
    {
        [HideInInspector]
        public TransportationItem transportationItem;

        EntityMeshWrapper entityMeshWrapper;

        /*
            Lifecycle Methods
        */
        void Awake()
        {
            transform.localPosition = Vector3.zero;
        }

        void OnDestroy()
        {
            Teardown();
        }

        public void Setup()
        {
            CreateMesh();
            UpdatePosition();
        }

        public void Teardown()
        {
        }

        public void Reset() { }

        public void OnBuild() { }


        void UpdatePosition()
        {
            // transform.position = GameWorldUtils.CellCoordinatesToPosition(transportationItem.cellCoordinatesList.bottomLeftCoordinates);
        }

        /* 
            Internals
        */
        void CreateMesh()
        {
            // TransformUtils.DestroyChildren(transform);

            GameWorldTransportationManager transportationManager = GameWorldTransportationManager.Find();

            GameObject prefabMesh = transportationManager.meshAssets.FindByKey(transportationItem.key);

            entityMeshWrapper = GetComponent<EntityMeshWrapper>();
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = transportationItem.cellCoordinatesList;
            entityMeshWrapper.Setup();
        }

        void DestroyMesh()
        {
            entityMeshWrapper.Teardown();
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