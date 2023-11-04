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
        public EntityTypeManagerBase manager = null;

        protected EntityMeshWrapper entityMeshWrapper => customMeshWrapper ?? GetComponent<EntityMeshWrapper>();

        protected virtual string meshAssetKey => "Default";

        protected Transform placeholder;

        protected virtual EntityMeshWrapper.EntityMeshCellWrapperFactory CreateEntityMeshCellWrapper =>
            (
                Transform parent,
                GameObject prefabMesh,
                Entity entity,
                CellCoordinates cellCoordinates,
                CellNeighbors cellNeighbors,
                Tileable.CellPosition cellPosition
            ) =>
               new EntityMeshCellWrapper(parent, prefabMesh, entity, cellCoordinates, cellNeighbors, cellPosition);

        void OnDestroy()
        {
            Teardown();
        }

        public virtual void Setup()
        {
            SetupMeshWrapper();
            UpdateColor();
        }

        public virtual void Teardown() { }

        public void Update()
        {
            UpdatePosition();
            UpdateColor();
        }

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
            string meshKey = GetMeshKey();

            GameObject prefabMesh = manager.meshAssets.ValueFromKey(meshKey);

            entityMeshWrapper.parent = transform;
            entityMeshWrapper.entity = entity;
            entityMeshWrapper.prefabMesh = prefabMesh;
            entityMeshWrapper.CreateEntityMeshCellWrapper = CreateEntityMeshCellWrapper;

            entityMeshWrapper.Setup();

            string GetMeshKey()
            {
                string meshKey = entity.definition.meshKey;

                if (!manager.meshAssets.HasKey(meshKey))
                {
                    Debug.LogWarning($"Key '{meshKey}' not found in {entity.typeLabel} mesh assets");
                    meshKey = "Default";
                }

                return meshKey;
            }
        }

        protected void DefaultUpdateEntityColor()
        {
            State.Key currentKey = Registry.appState.Tools.currentKey;

            bool hasUpdated;

            switch (currentKey)
            {
                case State.Key.Build:
                    hasUpdated = SetBuildStateColor();
                    break;
                case State.Key.Destroy:
                    hasUpdated = SetDestroyStateColor();
                    break;
                default:
                    hasUpdated = SetInspectStateColor();
                    break;
            }

            if (!hasUpdated)
            {
                entityMeshWrapper.SetOverlayColor(EntityMeshWrapper.OverlayColorKey.Default);
            }
        }

        protected virtual bool SetBuildStateColor()
        {
            if (entity.isInBlueprintMode)
            {
                if (entity.buildValidator.isValid)
                {
                    entityMeshWrapper.SetOverlayColor(EntityMeshWrapper.OverlayColorKey.ValidBlueprint);
                }
                else
                {
                    entityMeshWrapper.SetOverlayColor(EntityMeshWrapper.OverlayColorKey.InvalidBlueprint);
                }

                return true;
            }

            return false;
        }

        protected virtual bool SetDestroyStateColor()
        {
            // CellCoordinatesBlockList selectedBlocks = Registry.appState.UI.currentSelectedBlockList;
            ListWrapper<Entity> entitiesToDelete = Registry.appState.Tools.Destroy.entitiesToDelete;

            // if (entitiesToDelete.Count == null) return;

            foreach (Entity entityToDelete in entitiesToDelete.items)
            {
                if (entityToDelete == entity)
                {
                    entityMeshWrapper.SetOverlayColor(EntityMeshWrapper.OverlayColorKey.Destroy);
                    return true;
                }
            }

            return false;
        }

        protected virtual bool SetInspectStateColor()
        {
            Entity inspectedEntity = Registry.appState.Tools.Inspect.inspectedEntity;

            if (inspectedEntity == entity)
            {
                entityMeshWrapper.SetOverlayColor(EntityMeshWrapper.OverlayColorKey.Inspected);
                return true;
            }
            else if (Registry.appState.UI.entitiesInSelection.Contains(entity))
            {
                entityMeshWrapper.SetOverlayColor(EntityMeshWrapper.OverlayColorKey.Hover);
                return true;
            }

            return false;
        }
    }
}