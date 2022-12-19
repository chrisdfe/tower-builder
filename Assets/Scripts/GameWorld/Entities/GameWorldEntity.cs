using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes.Entities;
using UnityEngine;

namespace TowerBuilder.GameWorld.Entities
{
    [RequireComponent(typeof(EntityMeshWrapper))]
    public class GameWorldEntity : MonoBehaviour
    {
        [HideInInspector]
        public Entity entity { get; set; }

        [HideInInspector]
        public GameObject prefabMesh { get; set; }

        [HideInInspector]
        public EntityMeshWrapper customMeshWrapper;

        EntityMeshWrapper entityMeshWrapper => customMeshWrapper ?? GetComponent<EntityMeshWrapper>();

        public void Setup()
        {
            UpdateEntityColor();
        }

        public void Teardown() { }

        public void UpdateEntityColor()
        {
            Entity inspectedEntity = Registry.appState.Tools.inspectToolState.inspectedEntity;
            ToolState toolState = Registry.appState.Tools.toolState;

            bool hasUpdated = false;

            switch (toolState)
            {
                case (ToolState.Build):
                    SetBuildStateColor();
                    break;
                case (ToolState.Destroy):
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
                    if (entity.validator.isValid)
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
                // not supported yet
            }

            void SetInspectStateColor()
            {
                if (inspectedEntity == entity)
                {
                    entityMeshWrapper.SetColor(EntityMeshWrapper.ColorKey.Inspected);
                    hasUpdated = true;
                }
            }
        }
    }
}