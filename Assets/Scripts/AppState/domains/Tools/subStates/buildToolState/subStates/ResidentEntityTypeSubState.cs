using TowerBuilder.DataTypes.Residents;
using UnityEngine;

namespace TowerBuilder.State.Tools
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

            Events events;

            public ResidentEntityTypeSubState(BuildToolState buildToolState) : base(buildToolState) { }

            public override void Setup()
            {
                base.Setup();

                Debug.Log("hello");

                CreateBlueprintResident();
            }

            public override void Teardown()
            {
                base.Teardown();

                Debug.Log("goodby");

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
                Registry.appState.Residents.AddResident(blueprintResident);
            }

            void DestroyBlueprintResident()
            {
                Registry.appState.Residents.RemoveResident(blueprintResident);
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
