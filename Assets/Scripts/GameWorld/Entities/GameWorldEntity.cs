using System;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities
{
    [RequireComponent(typeof(EntityMeshWrapper))]
    public class GameWorldEntity : MonoBehaviour, ISetupable
    {
        [HideInInspector]
        public Entity entity { get; set; }

        [HideInInspector]
        public GameObject prefabMesh { get; set; }

        [HideInInspector]
        public EntityMeshWrapper customMeshWrapper;

        [HideInInspector]
        public Action<GameWorldEntity> customColorUpdater = null;

        [HideInInspector]
        public EntityTypeManager manager = null;

        protected EntityMeshWrapper entityMeshWrapper => customMeshWrapper ?? GetComponent<EntityMeshWrapper>();

        protected virtual string meshAssetKey => "Default";

        protected Transform placeholder;

        protected virtual EntityMeshWrapper.EntityMeshCellWrapperFactory CreateEntityMeshCellWrapper =>
            (
               Transform parent,
               GameObject prefabMesh,
               CellCoordinates cellCoordinates,
               CellCoordinates relativeCellCoordinates,
               CellNeighbors cellNeighbors,
               Tileable.CellPosition cellPosition
            ) =>
               new EntityMeshCellWrapper(parent, prefabMesh, cellCoordinates, relativeCellCoordinates, cellNeighbors, cellPosition);

        void Awake()
        {
        }

        void Start()
        {
            Setup();
        }

        void OnDestroy()
        {
            Teardown();
        }

        public virtual void Setup()
        {
            placeholder = transform.Find("Placeholder");

            SetupMeshWrapper();
            UpdateColor();
        }

        public virtual void Teardown() { }

        public void UpdatePosition()
        {
            entityMeshWrapper.UpdatePosition();
        }

        public void UpdateColor()
        {
            if (customColorUpdater != null)
            {
                customColorUpdater(this);
            }
            else
            {
                DefaultUpdateEntityColor();
            }
        }

        /*
            Internals
        */
        protected virtual void SetupMeshWrapper()
        {
            GameObject prefabMesh = manager.entityPrefab;

            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.cellCoordinatesList = entity.absoluteCellCoordinatesList;
            entityMeshWrapper.CreateEntityMeshCellWrapper = CreateEntityMeshCellWrapper;

            entityMeshWrapper.Setup();
        }

        void DefaultUpdateEntityColor()
        {
            ApplicationState.Tools.State.Key currentKey = Registry.appState.Tools.currentKey;

            bool hasUpdated = false;

            switch (currentKey)
            {
                case (ApplicationState.Tools.State.Key.Build):
                    SetBuildStateColor();
                    break;
                case (ApplicationState.Tools.State.Key.Destroy):
                    SetDestroyStateColor();
                    break;
                default:
                    SetInspectStateColor();
                    break;
            }

            if (!hasUpdated)
            {
                entityMeshWrapper.SetColor(EntityMeshWrapper.ColorKey.Default);
            }

            void SetBuildStateColor()
            {
                if (entity.isInBlueprintMode)
                {
                    if (entity.isValid)
                    {
                        entityMeshWrapper.SetColor(EntityMeshWrapper.ColorKey.ValidBlueprint);
                    }
                    else
                    {
                        entityMeshWrapper.SetColor(EntityMeshWrapper.ColorKey.InvalidBlueprint);
                    }

                    hasUpdated = true;
                }
            }

            void SetDestroyStateColor()
            {
                CellCoordinatesList cellCoordinatesToDestroyFrom = Registry.appState.Tools.Destroy.cellCoordinatesToDelete;

                // TODO - highlight on a per-cell basis
                if (entity.absoluteCellCoordinatesList.OverlapsWith(Registry.appState.UI.selectionBox.cellCoordinatesList))
                {
                    foreach (EntityMeshCellWrapper entityMeshCellWrapper in entityMeshWrapper.entityCellMeshWrapperList)
                    {
                        if (cellCoordinatesToDestroyFrom.Contains(entityMeshCellWrapper.cellCoordinates))
                        {
                            entityMeshCellWrapper.SetColor(EntityMeshWrapper.ColorKey.Destroy);
                        }
                    }

                    hasUpdated = true;
                }
            }

            void SetInspectStateColor()
            {
                Entity inspectedEntity = Registry.appState.Tools.Inspect.inspectedEntity;

                if (inspectedEntity == entity)
                {
                    entityMeshWrapper.SetColor(EntityMeshWrapper.ColorKey.Inspected);
                    hasUpdated = true;
                }
            }
        }
    }
}