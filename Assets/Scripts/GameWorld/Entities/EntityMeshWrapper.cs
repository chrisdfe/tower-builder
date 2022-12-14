using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities
{
    public class EntityMeshWrapper : ISetupable
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

        public GameObject prefabMesh { get; private set; }
        public Transform parent { get; private set; }

        CellCoordinatesList cellCoordinatesList;

        List<EntityMeshCellWrapper> entityCellMeshWrapperList = new List<EntityMeshCellWrapper>();

        public EntityMeshWrapper(Transform parent, GameObject prefabMesh, CellCoordinatesList cellCoordinatesList)
        {
            this.parent = parent;
            this.prefabMesh = prefabMesh;
            this.cellCoordinatesList = cellCoordinatesList;
        }

        public virtual void Setup()
        {
            DestroyPlaceholder();
            CreateEntityCellWrappers();
        }

        public virtual void Teardown()
        {
            DestroyEntityCellWrappers();
            // GameObject.Destroy(meshTransform.gameObject);
        }

        protected virtual EntityMeshCellWrapper CreateEntityCellMeshWrapper(Transform parent, GameObject prefabMesh, CellCoordinates cellCoordinates, CellNeighbors cellNeighbors, Tileable.CellPosition cellPosition) =>
            new EntityMeshCellWrapper(parent, prefabMesh, cellCoordinates, cellNeighbors, cellPosition);


        public void SetColor(ColorKey key)
        {
            foreach (var child in entityCellMeshWrapperList)
            {
                child.SetColor(key);
            }
        }

        void DestroyPlaceholder()
        {
            Transform placeholder = parent.Find(PLACEHOLDER_NODE_NAME);

            if (placeholder != null)
            {
                GameObject.Destroy(placeholder.gameObject);
            }
        }

        void CreateEntityCellWrappers()
        {
            entityCellMeshWrapperList = cellCoordinatesList.items.Select((cellCoordinates) =>
            {
                CellNeighbors cellNeighbors = CellNeighbors.FromCellCoordinatesList(cellCoordinates, cellCoordinatesList);

                Tileable.CellPosition cellPosition = Tileable.GetCellPosition(cellNeighbors);

                EntityMeshCellWrapper entityMeshCellWrapper = CreateEntityCellMeshWrapper(parent, prefabMesh, cellCoordinates, cellNeighbors, cellPosition);
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