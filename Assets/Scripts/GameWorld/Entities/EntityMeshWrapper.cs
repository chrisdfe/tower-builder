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
        public enum ColorKey
        {
            Base,
            Default,
            Hover,
            Inspected,
            Destroy,
            ValidBlueprint,
            InvalidBlueprint
        }

        public static Dictionary<ColorKey, Color> ColorMap = new Dictionary<ColorKey, Color>() {
            { ColorKey.Base, Color.grey },
            { ColorKey.Hover, Color.green },
            { ColorKey.Inspected, Color.cyan },
            { ColorKey.Destroy, Color.red },
            { ColorKey.ValidBlueprint, Color.blue },
            { ColorKey.InvalidBlueprint, Color.red },
        };

        public const string PLACEHOLDER_NODE_NAME = "Placeholder";

        public Transform parent { get; set; }
        public GameObject prefabMesh { get; set; }
        public Entity entity { get; set; }
        public List<EntityMeshCellWrapper> entityCellMeshWrapperList { get; private set; } = new List<EntityMeshCellWrapper>();

        public delegate EntityMeshCellWrapper EntityMeshCellWrapperFactory(
            Transform parent,
            GameObject prefabMesh,
            Entity entity,
            CellNeighbors cellNeighbors,
            Tileable.CellPosition cellPosition
        );

        public EntityMeshCellWrapperFactory CreateEntityMeshCellWrapper =
            (
                Transform parent,
                GameObject prefabMesh,
                Entity entity,
                CellNeighbors cellNeighbors,
                Tileable.CellPosition cellPosition
            ) =>
               new EntityMeshCellWrapper(parent, prefabMesh, entity, cellNeighbors, cellPosition);

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

        public void UpdatePosition()
        {
            transform.localPosition = GameWorldUtils.CellCoordinatesToPosition(
                entity.absoluteCellCoordinatesList.bottomLeftCoordinates,
                1f
            );
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetColor(ColorKey key)
        {
            foreach (var child in entityCellMeshWrapperList)
            {
                child.SetColor(key);
            }
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