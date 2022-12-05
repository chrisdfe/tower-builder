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
        public AssetList<TransportationItem.Key> assetList = new AssetList<TransportationItem.Key>();

        [HideInInspector]
        public TransportationItem transportationItem;

        List<MeshWrapper> meshWrapperList = new List<MeshWrapper>();
        List<Transform> meshList = new List<Transform>();

        public delegate MeshWrapper MeshWrapperFactory(Transform meshTransform);

        static Dictionary<TransportationItem.Key, MeshWrapperFactory> MeshWrapperKeyMap = new Dictionary<TransportationItem.Key, MeshWrapperFactory>() {
            {
                TransportationItem.Key.Ladder,
                (meshTransform) => new LadderMeshWrapper(meshTransform)
            },
            {
                TransportationItem.Key.Escalator,
                (meshTransform) => new EscalatorMeshWrapper(meshTransform)
            },
            {
                TransportationItem.Key.Doorway,
                (meshTransform) => new DoorwayMeshWrapper(meshTransform)
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

            MaterialsReplacer replacer = GetComponent<MaterialsReplacer>();
            replacer.ReplaceMaterials();
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

            MeshWrapperFactory meshWrapperFactory = MeshWrapperKeyMap[transportationItem.key];

            meshWrapperList = transportationItem.cellCoordinatesList.items.Select((cellCoordinates) =>
            {
                MeshWrapper meshWrapper = meshWrapperFactory(assetList.FindByKey(transportationItem.key).transform);
                meshWrapper.CreateMesh();
                meshWrapper.mesh.SetParent(transform, false);
                meshWrapper.mesh.position = GameWorldUtils.CellCoordinatesToPosition(cellCoordinates, 1f);
                meshWrapper.mesh.Translate(new Vector3(0, 0, -2f));

                // TODO - change the api of this to only need a cell coordinates list
                // i.e SetTileability(transportationItem.cellCoordinatesList)
                Tileable.OccupiedCellMap occupiedCellMap =
                    Tileable.OccupiedCellMap.FromCellCoordinatesList(cellCoordinates, transportationItem.cellCoordinatesList);

                meshWrapper.SetTileability(occupiedCellMap);

                return meshWrapper;
            }).ToList();
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
            GameWorldTransportationManager transportationManager = GameWorldTransportationManager.Find();
            GameObject prefab = transportationManager.assetList.FindByKey(GameWorldTransportationManager.AssetKey.TransportationItem);
            GameObject transportationItemGameObject = Instantiate<GameObject>(prefab);

            transportationItemGameObject.transform.parent = parent;

            GameWorldTransportationItem gameWorldTransportationItem = transportationItemGameObject.GetComponent<GameWorldTransportationItem>();
            return gameWorldTransportationItem;
        }

        /*
            Internal classes
        */
        public abstract class MeshWrapper
        {
            public Transform mesh { get; private set; }

            Transform prefabMeshTransform;
            Tileable tileable;

            Transform tileabilityWrapper;
            Transform tileabilityCellVariantsWrapper;

            public MeshWrapper(Transform prefabMeshTransform)
            {
                this.prefabMeshTransform = prefabMeshTransform;
            }

            public virtual void Setup() { }

            public virtual void Teardown() { }

            public void CreateMesh()
            {
                mesh = Instantiate(
                    prefabMeshTransform,
                    Vector3.zero,
                    Quaternion.identity
                );
            }

            public void SetTileability(Tileable.OccupiedCellMap occupiedCellMap)
            {
                tileable = Tileable.FromModel(mesh);

                if (tileable != null)
                {
                    tileable.ProcessModel(mesh, occupiedCellMap);
                }
            }
        }

        public class LadderMeshWrapper : MeshWrapper
        {
            public LadderMeshWrapper(Transform meshTransform) : base(meshTransform) { }
        }

        public class EscalatorMeshWrapper : MeshWrapper
        {
            public EscalatorMeshWrapper(Transform meshTransform) : base(meshTransform) { }
        }

        public class DoorwayMeshWrapper : MeshWrapper
        {
            public DoorwayMeshWrapper(Transform meshTransform) : base(meshTransform) { }
        }
    }
}