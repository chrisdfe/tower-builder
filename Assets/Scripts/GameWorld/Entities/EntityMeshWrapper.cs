using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities
{
    public class EntityMeshWrapper : MonoBehaviour, ISetupable
    {
        public enum OverlayColorKey
        {
            Base,
            Default,
            Hover,
            Inspected,
            Destroy,
            ValidBlueprint,
            InvalidBlueprint
        }

        public static Dictionary<OverlayColorKey, Color> OverlayColorMap = new Dictionary<OverlayColorKey, Color>() {
            { OverlayColorKey.Base, Color.black },
            { OverlayColorKey.Hover, Color.white },
            { OverlayColorKey.Inspected, Color.cyan },
            { OverlayColorKey.Destroy, Color.red },
            { OverlayColorKey.ValidBlueprint, Color.blue },
            { OverlayColorKey.InvalidBlueprint, Color.red },
        };

        public const string PLACEHOLDER_NODE_NAME = "Placeholder";

        public Transform parent { get; set; }
        public GameObject prefabMesh { get; set; }
        public Entity entity { get; set; }
        public List<EntityMeshCellWrapper> entityCellMeshWrapperList { get; private set; } = new List<EntityMeshCellWrapper>();
        public Vector3 positionOffset = Vector3.zero;

        public delegate EntityMeshCellWrapper EntityMeshCellWrapperFactory(
            Transform parent,
            GameObject prefabMesh,
            Entity entity,
            CellCoordinates cellCoordinates,
            CellNeighbors cellNeighbors,
            Tileable.CellPosition cellPosition
        );

        public EntityMeshCellWrapperFactory CreateEntityMeshCellWrapper =
            (
                Transform parent,
                GameObject prefabMesh,
                Entity entity,
                CellCoordinates cellCoordinates,
                CellNeighbors cellNeighbors,
                Tileable.CellPosition cellPosition
            ) =>
               new EntityMeshCellWrapper(parent, prefabMesh, entity, cellCoordinates, cellNeighbors, cellPosition);

        public virtual void Setup()
        {
            UpdatePosition();
            DestroyPlaceholder();
            CreateEntityCellWrappers();
        }

        public virtual void Teardown()
        {
            DestroyEntityCellWrappers();
        }

        public void Reset()
        {
            Teardown();
            Setup();
        }

        public void Update()
        {
            UpdatePosition();
        }

        public void SetOverlayColor(OverlayColorKey key)
        {
            foreach (var child in entityCellMeshWrapperList)
            {
                child.SetOverlayColor(key);
            }
        }

        public void UpdatePosition()
        {
            transform.localPosition = GameWorldUtils.CellCoordinatesToPosition(
                Registry.appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).bottomLeftCoordinates,
                1f
            ) + positionOffset;
        }

        void DestroyPlaceholder()
        {
            TransformUtils.DestroyChildren(parent);

            Transform placeholder = transform.Find(PLACEHOLDER_NODE_NAME);

            if (placeholder != null)
            {
                GameObject.Destroy(placeholder.gameObject);
            }
        }

        void CreateEntityCellWrappers()
        {
            // TODO here - replace this with "absoluteCellCoordinatesList"?
            entityCellMeshWrapperList = entity.relativeCellCoordinatesList.items
                .Select((cellCoordinates) =>
                {
                    CellNeighbors cellNeighbors = CellNeighbors.FromCellCoordinatesList(cellCoordinates, entity.relativeCellCoordinatesList);

                    Tileable.CellPosition cellPosition = Tileable.GetCellPosition(cellNeighbors);

                    EntityMeshCellWrapper entityMeshCellWrapper =
                        CreateEntityMeshCellWrapper(
                            transform,
                            prefabMesh,
                            entity,
                            cellCoordinates,
                            cellNeighbors,
                            cellPosition
                        );

                    entityMeshCellWrapper.Setup();

                    return entityMeshCellWrapper;
                }).ToList();
        }

        void DestroyEntityCellWrappers()
        {
            foreach (EntityMeshCellWrapper entityMeshCellWrapper in entityCellMeshWrapperList)
            {
                entityMeshCellWrapper.Teardown();
            }
        }
    }
}