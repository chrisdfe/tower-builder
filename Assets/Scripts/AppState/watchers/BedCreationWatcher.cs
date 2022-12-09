using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using UnityEngine;

namespace TowerBuilder.ApplicationState
{
    public class BedCreationWatcher : WatcherBase
    {
        // Queue<Furniture> furnitureToMoveResidentsInFor = new Queue<Furniture>();

        public BedCreationWatcher(AppState appState) : base(appState) { }

        public override void Setup()
        {
            appState.Entities.Furnitures.events.onItemsAdded += OnFurnituresAdded;
            appState.Entities.Furnitures.events.onItemsBuilt += OnFurnituresBuilt;
        }

        public override void Teardown()
        {
            appState.Entities.Furnitures.events.onItemsAdded -= OnFurnituresAdded;
            appState.Entities.Furnitures.events.onItemsBuilt -= OnFurnituresBuilt;
        }

        void OnFurnituresAdded(FurnitureList furnitureList)
        {
            MoveNewResidentInIfThereIsRoom(furnitureList);
        }

        void OnFurnituresBuilt(FurnitureList furnitureList)
        {
            MoveNewResidentInIfThereIsRoom(furnitureList);
        }

        void MoveNewResidentInIfThereIsRoom(FurnitureList furnitureList)
        {
            furnitureList.items.ForEach(furniture => MoveNewResidentInIfThereIsRoom(furniture));
        }

        void MoveNewResidentInIfThereIsRoom(Furniture furniture)
        {
            if (furniture.isInBlueprintMode || (furniture.definition as FurnitureDefinition).homeSlotCount == 0) return;

            // TODO -
            // 1) check that the bedroom is accessible from the front door

            ResidentsList residentsLivingAtSlotList = appState.FurnitureHomeSlotOccupations.queries.GetResidentsLivingAt(furniture);

            if (residentsLivingAtSlotList.Count >= (furniture.definition as FurnitureDefinition).homeSlotCount) return;

            // Move a new resident in
            Resident resident = new Resident();

            // place it where the resident is for now
            resident.cellCoordinates = furniture.cellCoordinatesList.items[0];
            appState.Entities.Residents.Add(resident);

            // add ownership
            FurnitureHomeSlotOccupation homeSlotOccupation = new FurnitureHomeSlotOccupation()
            {
                resident = resident,
                furniture = furniture
            };
            appState.FurnitureHomeSlotOccupations.Add(homeSlotOccupation);
        }
    }
}
