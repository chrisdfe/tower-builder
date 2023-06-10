using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.EntityGroups.Rooms;

namespace TowerBuilder.ApplicationState.Tools.Inspect
{
    public class State : ToolStateBase
    {
        public struct Input
        {
            public Room currentInspectedRoom;
        }

        public delegate void InspectedEntityListEvent(ListWrapper<Entity> entityList);
        public InspectedEntityListEvent onInspectedEntityListUpdated;

        public delegate void CurrentSelectedEntityEvent(Entity entity);
        public CurrentSelectedEntityEvent onCurrentSelectedEntityUpdated;


        public ListWrapper<Entity> inspectedEntityList { get; private set; } = new ListWrapper<Entity>();
        public int inspectedEntityIndex { get; private set; } = -1;

        public Entity inspectedEntity
        {
            get
            {
                if (inspectedEntityIndex == -1)
                {
                    return null;
                }

                return inspectedEntityList.items[inspectedEntityIndex];
            }
        }

        public State(AppState appState, Tools.State state, Input input) : base(appState, state)
        {
        }

        public override void Setup()
        {
            base.Setup();

            appState.Entities.Furnitures.onItemsRemoved += OnFurnituresRemoved;
            appState.Entities.Residents.onItemsRemoved += OnResidentsRemoved;

            appState.UI.onSecondaryActionPerformed += OnSecondaryActionPerformed;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.Furnitures.onItemsRemoved -= OnFurnituresRemoved;
            appState.Entities.Residents.onItemsRemoved -= OnResidentsRemoved;

            appState.UI.onSecondaryActionPerformed -= OnSecondaryActionPerformed;

            inspectedEntityList = new ListWrapper<Entity>();
            inspectedEntityIndex = -1;

            if (onInspectedEntityListUpdated != null)
            {
                onInspectedEntityListUpdated(inspectedEntityList);
            }

            if (onCurrentSelectedEntityUpdated != null)
            {
                onCurrentSelectedEntityUpdated(inspectedEntity);
            }
        }

        public override void OnSelectionStart(SelectionBox selectionBox)
        {
            if (appState.UI.currentSelectedCellEntityList != inspectedEntityList)
            {
                inspectedEntityIndex = -1;
            }

            inspectedEntityList = appState.UI.currentSelectedCellEntityList;

            if (inspectedEntityList.Count > 0)
            {
                // Start at the top and work down as the user clicks
                if (inspectedEntityIndex > 0)
                {
                    // move down the list
                    inspectedEntityIndex = inspectedEntityIndex - 1;
                }
                else
                {
                    // back up to the top
                    inspectedEntityIndex = inspectedEntityList.Count - 1;
                }
            }
            else
            {
                if (inspectedEntityIndex != -1)
                {
                    inspectedEntityIndex = -1;
                }
            }

            if (onInspectedEntityListUpdated != null)
            {
                onInspectedEntityListUpdated(inspectedEntityList);
            }

            if (onCurrentSelectedEntityUpdated != null)
            {
                onCurrentSelectedEntityUpdated(inspectedEntity);
            }
        }

        /* 
            Event handlers
         */
        void OnFurnituresRemoved(ListWrapper<Entity> furnitureList)
        {
            OnEntitiesRemoved<Entity>(furnitureList);
        }

        void OnResidentsRemoved(ListWrapper<Entity> residentsList)
        {
            OnEntitiesRemoved<Entity>(residentsList);
        }

        void OnEntitiesRemoved<EntityType>(ListWrapper<EntityType> entityList)
            where EntityType : Entity
        {
            bool shouldReset = false;

            foreach (Entity entity in entityList.items)
            {
                if (inspectedEntityList.Contains(entity))
                {
                    shouldReset = true;
                    inspectedEntityList.Remove(entity);
                }
            }

            if (shouldReset)
            {
                // back up to the top
                inspectedEntityIndex = inspectedEntityList.Count - 1;

                if (onCurrentSelectedEntityUpdated != null)
                {
                    onCurrentSelectedEntityUpdated(inspectedEntity);
                }
            }
        }

        void OnSecondaryActionPerformed()
        {
            if (!(inspectedEntity is Resident)) return;

            Resident resident = (inspectedEntity as Resident);
            CellCoordinates targetCellCoordinates = appState.UI.currentSelectedCell;
            ListWrapper<Furniture> furnituresAtTarget = appState.Entities.Furnitures.FindFurnituresAtCell(targetCellCoordinates);

            if (furnituresAtTarget.Count > 0)
            {
                // TODO - a different/smarter way of doing this
                Furniture furnitureAtTarget = furnituresAtTarget.items[0];
                appState.Behaviors.Residents.SendResidentTo(resident, furnitureAtTarget);
            }
            else
            {
                appState.Behaviors.Residents.SendResidentTo(resident, targetCellCoordinates);
            }
        }
    }
}