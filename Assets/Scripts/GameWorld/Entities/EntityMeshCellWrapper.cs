using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities
{
    public class EntityMeshCellWrapper
    {
        public const string TILEABLE_WRAPPER_NODE_NAME = "TileableWrapper";

        public GameObject prefabMesh { get; private set; }
        public Transform meshTransform { get; private set; }
        public Transform parent { get; private set; }

        protected Transform tileabilityWrapper;
        protected CellCoordinates cellCoordinates;
        protected CellNeighbors cellNeighbors;
        protected Tileable.CellPosition cellPosition;

        List<MeshRenderer> childrenMeshRenderers = new List<MeshRenderer>();
        Dictionary<MeshRenderer, Color> defaultColorMap = new Dictionary<MeshRenderer, Color>();

        public EntityMeshCellWrapper(
            Transform parent,
            GameObject prefabMesh,
            CellCoordinates cellCoordinates,
            CellNeighbors cellNeighbors,
            Tileable.CellPosition cellPosition
        )
        {
            this.parent = parent;

            this.prefabMesh = prefabMesh;

            this.cellCoordinates = cellCoordinates;
            this.cellNeighbors = cellNeighbors;
            this.cellPosition = cellPosition;
        }

        public virtual void Setup()
        {
            InstantiateModel();
            ProcessModel();
            SetPosition();
        }

        public virtual void Teardown()
        {
            GameObject.Destroy(meshTransform.gameObject);
        }

        public void SetColor(EntityMeshWrapper.ColorKey key)
        {
            Debug.Log("setting color to: " + key);

            foreach (MeshRenderer meshRenderer in childrenMeshRenderers)
            {
                if (key == EntityMeshWrapper.ColorKey.Default)
                {
                    Color defaultColor = defaultColorMap[meshRenderer];
                    SetColor(meshRenderer, defaultColor);
                }
                else
                {
                    Color color = EntityMeshWrapper.ColorMap[key];

                    if (color != null)
                    {
                        SetColor(meshRenderer, color);
                    }
                }
            }

            // TODO - something more extensible than this
            void SetColor(MeshRenderer meshRenderer, Color color)
            {
                Color defaultColor = defaultColorMap[meshRenderer];
                meshRenderer.material.color = new Color(color.r, color.g, color.b, defaultColor.a);
            }
        }

        protected void InstantiateModel()
        {
            meshTransform = GameObject.Instantiate(prefabMesh).GetComponent<Transform>();
            meshTransform.SetParent(parent, false);
            meshTransform.localPosition = Vector3.zero;

            // TODO here - use entity layer instead
            meshTransform.localPosition = GameWorldUtils.CellCoordinatesToPosition(cellCoordinates, 1f);
            meshTransform.Translate(new Vector3(0, 0, -2f));

            childrenMeshRenderers =
                TransformUtils
                    .FindDeepChildrenWhere(meshTransform, (child) => child.GetComponent<MeshRenderer>() != null)
                    .Select(child => child.GetComponent<MeshRenderer>())
                    .ToList();

            defaultColorMap = childrenMeshRenderers.Aggregate(new Dictionary<MeshRenderer, Color>(), (acc, meshRenderer) =>
            {
                acc[meshRenderer] = meshRenderer.material.color;
                return acc;
            });
        }

        protected void ProcessModel()
        {
            Transform tileabileWrapper = TransformUtils.FindDeepChild(meshTransform, TILEABLE_WRAPPER_NODE_NAME);

            if (tileabileWrapper == null)
            {
                // Debug.Log("No TileableWrapper found");
                return;
            }

            Transform child = tileabileWrapper.GetChild(0);

            if (child != null)
            {
                foreach (Transform node in child)
                {
                    Tileable.CellPosition nodeCellPosition = Tileable.CellPositionLabelMap.KeyFromValue(node.name);
                    node.localPosition = Vector3.zero;
                    node.gameObject.SetActive(nodeCellPosition == cellPosition);
                }
            }
        }

        protected void SetPosition()
        {
            // meshTransform.position = GameWorldUtils.CellCoordinatesToPosition(cellCoordinates);
        }
    }
}