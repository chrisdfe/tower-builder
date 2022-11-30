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
        public AssetList<TransportationItem.Key> assetList = new AssetList<TransportationItem.Key>();

        [HideInInspector]
        public TransportationItem transportationItem;

        List<MeshWrapperBase> meshWrapperList = new List<MeshWrapperBase>();
        List<Transform> meshList = new List<Transform>();

        public delegate MeshWrapperBase MeshWrapperFactory(GameWorldTransportationItem gameWorldTransportationItem, Transform meshTransform);

        static Dictionary<TransportationItem.Key, MeshWrapperFactory> MeshWrapperKeyMap = new Dictionary<TransportationItem.Key, MeshWrapperFactory>() {
            {
                TransportationItem.Key.Ladder,
                (gameWorldTransportationItem, meshTransform) => new LadderMeshWrapper(gameWorldTransportationItem, meshTransform)
            },
            {
                TransportationItem.Key.Escalator,
                (gameWorldTransportationItem, meshTransform) => new EscalatorMeshWrapper(gameWorldTransportationItem, meshTransform)
            },
            {
                TransportationItem.Key.Doorway,
                (gameWorldTransportationItem, meshTransform) => new DoorwayMeshWrapper(gameWorldTransportationItem, meshTransform)
            }
        };

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
            // transform.localPosition = GameWorldMapCellHelpers.CellCoordinatesToPosition(transportationItem.cellCoordinatesList.items[0], 1f);
        }

        /* 
            Internals
        */
        void CreateMesh()
        {
            TransformUtils.DestroyChildren(transform);

            MeshWrapperFactory meshWrapperFactory = MeshWrapperKeyMap[transportationItem.key];
            MeshWrapperBase meshWrapper = meshWrapperFactory(this, assetList.FindByKey(transportationItem.key).transform);

            // for now just copy the prefab into every cell
            foreach (CellCoordinates cellCoordinates in transportationItem.cellCoordinatesList.items)
            {
                Transform meshTransform = meshWrapper.CreateMesh();
                meshTransform.SetParent(transform);
                meshTransform.localPosition = GameWorldMapCellHelpers.CellCoordinatesToPosition(cellCoordinates, 1f);

                meshWrapperList.Add(meshWrapper);
                meshList.Add(meshTransform);
                // meshWrapper.meshTransform.Translate(new Vector3(0, 0, 0.5f));
            }
        }

        void DestroyMesh()
        {
            foreach (Transform meshTransform in meshList)
            {
                GameObject.Destroy(transform);
            }
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
        public abstract class MeshWrapperBase
        {
            public GameWorldTransportationItem gameWorldTransportationItem { get; private set; }

            Transform prefabMeshTransform;

            public MeshWrapperBase(GameWorldTransportationItem gameWorldTransportationItem, Transform prefabMeshTransform)
            {
                this.gameWorldTransportationItem = gameWorldTransportationItem;
                this.prefabMeshTransform = prefabMeshTransform;
            }

            public virtual void Setup() { }

            public virtual void Teardown() { }

            public Transform CreateMesh()
            {
                return Instantiate(
                    prefabMeshTransform,
                    GameWorldMapCellHelpers.CellCoordinatesToPosition(gameWorldTransportationItem.transportationItem.cellCoordinatesList.items[0]),
                    Quaternion.identity
                );
            }
        }

        public class LadderMeshWrapper : MeshWrapperBase
        {
            public LadderMeshWrapper(GameWorldTransportationItem gameWorldTransportationItem, Transform meshTransform) : base(gameWorldTransportationItem, meshTransform) { }
        }

        public class EscalatorMeshWrapper : MeshWrapperBase
        {
            public EscalatorMeshWrapper(GameWorldTransportationItem gameWorldTransportationItem, Transform meshTransform) : base(gameWorldTransportationItem, meshTransform) { }
        }

        public class DoorwayMeshWrapper : MeshWrapperBase
        {
            public DoorwayMeshWrapper(GameWorldTransportationItem gameWorldTransportationItem, Transform meshTransform) : base(gameWorldTransportationItem, meshTransform) { }
        }
    }
}