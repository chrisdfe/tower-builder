using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.TransportationItems;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldTransportationItem : MonoBehaviour
    {
        public enum ModelKey
        {
            Ladder
        }

        public AssetList<ModelKey> assetList = new AssetList<ModelKey>();

        [HideInInspector]
        public TransportationItem transportationItem;

        LadderMeshWrapper ladderMeshWrapper;

        public delegate TransportationItemMeshWrapperBase MeshWrapperFactory(GameWorldTransportationItem gameWorldTransportationItem, Transform meshTransform);
        static Dictionary<ModelKey, MeshWrapperFactory> MeshWrapperKeyMap = new Dictionary<ModelKey, MeshWrapperFactory>() {
            { ModelKey.Ladder, (gameWorldTransportationItem, meshTransform) => new LadderMeshWrapper(gameWorldTransportationItem, meshTransform) }
        };

        /*
            Lifecycle Methods
        */
        void Awake()
        {
            transform.localPosition = Vector3.zero;

            ladderMeshWrapper = new LadderMeshWrapper(this, assetList.FindByKey(ModelKey.Ladder).transform);
        }

        void OnDestroy() { }

        public void Setup()
        {
            UpdatePosition();
            CreateMesh();
        }

        public void Teardown()
        {
        }

        public void Reset()
        {
        }

        public void OnBuild()
        {
        }

        /* 
            Public Interface
        */
        public void UpdatePosition()
        {
            transform.localPosition = GameWorldMapCellHelpers.CellCoordinatesToPosition(transportationItem.cellCoordinatesList.items[0], 1f);
        }

        /* 
            Internals
        */
        void CreateMesh()
        {
            TransformUtils.DestroyChildren(transform);
            ladderMeshWrapper.CreateMesh();

            // for now just copy the cube into every other cell
            // foreach (CellCoordinates cellCoordinates in transportationItem.cellCoordinatesList.items)
            // {
            //     Transform cubeClone = Instantiate(cube, GameWorldMapCellHelpers.CellCoordinatesToPosition(cellCoordinates), Quaternion.identity);
            //     cubeClone.SetParent(transform);
            //     cubeClone.Translate(new Vector3(0, 0, 0.5f));
            // }
        }

        /* 
            Static API
         */
        public static GameWorldTransportationItem Create(Transform parent)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Map/TransportationItems/TransportationItem");
            GameObject roomGameObject = Instantiate<GameObject>(prefab);

            roomGameObject.transform.parent = parent;

            GameWorldTransportationItem gameWorldTransportationItem = roomGameObject.GetComponent<GameWorldTransportationItem>();
            return gameWorldTransportationItem;
        }

        /*
            Internal classes
        */
        public abstract class TransportationItemMeshWrapperBase
        {
            public Transform meshTransform;
            public GameWorldTransportationItem gameWorldTransportationItem { get; private set; }

            public TransportationItemMeshWrapperBase(GameWorldTransportationItem gameWorldTransportationItem, Transform meshTransform)
            {
                this.gameWorldTransportationItem = gameWorldTransportationItem;
                this.meshTransform = meshTransform;
            }

            public virtual void Setup() { }

            public virtual void Teardown() { }

            public void CreateMesh()
            {
                Transform cubeClone = Instantiate(
                    meshTransform,
                    GameWorldMapCellHelpers.CellCoordinatesToPosition(gameWorldTransportationItem.transportationItem.cellCoordinatesList.items[0]),
                    Quaternion.identity
                );
                cubeClone.SetParent(gameWorldTransportationItem.transform);
                // cubeClone.Translate(new Vector3(0, 0, 0.5f));
            }
        }

        public class LadderMeshWrapper : TransportationItemMeshWrapperBase
        {
            public LadderMeshWrapper(GameWorldTransportationItem gameWorldTransportationItem, Transform meshTransform) : base(gameWorldTransportationItem, meshTransform) { }
        }
    }
}