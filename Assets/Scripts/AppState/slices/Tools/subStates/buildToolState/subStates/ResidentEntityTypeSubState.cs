using TowerBuilder.DataTypes.Entities.Residents;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public class ResidentEntityTypeSubState : EntityTypeSubState
        {
            public Resident blueprintResident { get; private set; }

            public class Events
            {
                public delegate void blueprintUpdateEvent(Resident blueprintResident);
                public blueprintUpdateEvent onBlueprintResidentUpdated;
            }

            public Events events;

            public ResidentEntityTypeSubState(BuildToolState buildToolState) : base(buildToolState) { }

            public override void Setup()
            {
                base.Setup();

                CreateBlueprintResident();
            }

            public override void Teardown()
            {
                base.Teardown();

                DestroyBlueprintResident();
            }

            public override void EndBuild()
            {
                base.EndBuild();

                // TODO - validate
                BuildBlueprintResident();
                CreateBlueprintResident();
            }

            public override void OnSelectionBoxUpdated()
            {
                base.OnSelectionBoxUpdated();

                ResetBlueprintResident();
            }

            void CreateBlueprintResident()
            {
                blueprintResident = new Resident();
                blueprintResident.isInBlueprintMode = true;
                blueprintResident.cellCoordinates = Registry.appState.UI.currentSelectedCell;
                Registry.appState.Residents.Add(blueprintResident);
            }

            void DestroyBlueprintResident()
            {
                Registry.appState.Residents.Remove(blueprintResident);
                blueprintResident = null;
            }

            void BuildBlueprintResident()
            {
                Registry.appState.Residents.BuildResident(blueprintResident);
                blueprintResident = null;
            }

            void ResetBlueprintResident()
            {
                DestroyBlueprintResident();
                CreateBlueprintResident();
            }
        }
    }
}
