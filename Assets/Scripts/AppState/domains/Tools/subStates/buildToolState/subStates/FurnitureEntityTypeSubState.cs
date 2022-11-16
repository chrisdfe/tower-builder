using TowerBuilder.DataTypes.Furnitures;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public class FurnitureEntityTypeSubState : EntityTypeSubState
        {
            public Furniture blueprintFurniture { get; private set; }

            public class Events
            {
                public delegate void blueprintUpdateEvent(Furniture blueprintFurniture);
                public blueprintUpdateEvent onBlueprintFurnitureUpdated;
            }

            Events events;

            public FurnitureEntityTypeSubState(BuildToolState buildToolState) : base(buildToolState) { }

            public override void Setup()
            {
                base.Setup();

                CreateBlueprintFurniture();
            }

            public override void Teardown()
            {
                base.Teardown();

                DestroyBlueprintFurniture();
            }

            public override void EndBuild()
            {
                base.EndBuild();

                // TODO - validate
                BuildBlueprintFurniture();
                CreateBlueprintFurniture();
            }

            public override void OnSelectionBoxUpdated()
            {
                base.OnSelectionBoxUpdated();

                ResetBlueprintFurniture();
            }

            void CreateBlueprintFurniture()
            {
                blueprintFurniture = new Furniture();
                blueprintFurniture.isInBlueprintMode = true;
                blueprintFurniture.cellCoordinates = Registry.appState.UI.currentSelectedCell;
                Registry.appState.Furnitures.AddFurniture(blueprintFurniture);
            }

            void DestroyBlueprintFurniture()
            {
                Registry.appState.Furnitures.RemoveFurniture(blueprintFurniture);
                blueprintFurniture = null;
            }

            void BuildBlueprintFurniture()
            {
                Registry.appState.Furnitures.BuildFurniture(blueprintFurniture);
                blueprintFurniture = null;
            }

            void ResetBlueprintFurniture()
            {
                DestroyBlueprintFurniture();
                CreateBlueprintFurniture();
            }
        }
    }
}
