using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
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

        public GameObject prefabMesh { get; set; }
        public CellCoordinatesList cellCoordinatesList { get; set; }

        List<EntityMeshCellWrapper> entityCellMeshWrapperList = new List<EntityMeshCellWrapper>();

        CellCoordinates originCellCoordinates => cellCoordinatesList.bottomLeftCoordinates;

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
            transform.localPosition = GameWorldUtils.CellCoordinatesToPosition(originCellCoordinates, 1f);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetColor(ColorKey key)
        {
            Debug.Log("SetColor");
            Debug.Log("entityCellMeshWrapperList");
            Debug.Log(entityCellMeshWrapperList.Count);
            foreach (var child in entityCellMeshWrapperList)
            {
                child.SetColor(key);
            }
        }

        protected virtual EntityMeshCellWrapper CreateEntityCellMeshWrapper(
            Transform parent,
            GameObject prefabMesh,
            CellCoordinates cellCoordinates,
            CellNeighbors cellNeighbors,
            Tileable.CellPosition cellPosition
        ) =>
            new EntityMeshCellWrapper(parent, prefabMesh, cellCoordinates, cellNeighbors, cellPosition);

        void DestroyPlaceholder()
        {
            Transform placeholder = transform.Find(PLACEHOLDER_NODE_NAME);

            if (placeholder != null)
            {
                GameObject.Destroy(placeholder.gameObject);
            }
        }

        void CreateEntityCellWrappers()
        {
            Debug.Log("CreateEntityCellWrappers");
            Debug.Log("cellCoordinatesList.Count");
            Debug.Log(cellCoordinatesList.Count);

            entityCellMeshWrapperList = cellCoordinatesList.items
                .Select((cellCoordinates) =>
                {
                    CellNeighbors cellNeighbors = CellNeighbors.FromCellCoordinatesList(cellCoordinates, cellCoordinatesList);

                    Tileable.CellPosition cellPosition = Tileable.GetCellPosition(cellNeighbors);

                    CellCoordinates relativeCellCoordiantes = cellCoordinatesList.AsRelativeCoordinates(cellCoordinates);

                    EntityMeshCellWrapper entityMeshCellWrapper =
                        CreateEntityCellMeshWrapper(
                            transform,
                            prefabMesh,
                            relativeCellCoordiantes,
                            cellNeighbors,
                            cellPosition
                        );

                    entityMeshCellWrapper.Setup();

                    return entityMeshCellWrapper;
                }).ToList();

            Debug.Log("entityCellMeshWrapperList.Count");
            Debug.Log(entityCellMeshWrapperList.Count);
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