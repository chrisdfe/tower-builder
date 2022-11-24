using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using UnityEngine;

namespace TowerBuilder.ApplicationState
{
    public class BedCreationWatcher : WatcherBase
    {
        public BedCreationWatcher(AppState appState) : base(appState) { }

        public override void Setup()
        {
            appState.Furnitures.events.onFurnituresAdded += OnFurnituresAdded;
            appState.Furnitures.events.onFurnituresBuilt += OnFurnituresBuilt;
        }

        public override void Teardown()
        {
            appState.Furnitures.events.onFurnituresAdded -= OnFurnituresAdded;
            appState.Furnitures.events.onFurnituresBuilt -= OnFurnituresBuilt;
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
            Debug.Log("MoveNewResidentInIfThereIsRoom");

            if (furniture.isInBlueprintMode) return;

            // TODO -
            // 1) check that the bedroom is accessible from the front door
            // 2) 
            Debug.Log("furniture.homeSlotCount");
            Debug.Log(furniture.homeSlotCount);

            if (furniture.homeSlotCount == 0) return;

            ResidentsList residentsLivingAtSlotList = appState.FurnitureHomeSlotOccupations.queries.GetResidentsLivingAt(furniture);

            Debug.Log("residentsLivingAtSlotList");
            Debug.Log(residentsLivingAtSlotList.Count);

            if (residentsLivingAtSlotList.Count >= furniture.homeSlotCount) return;

            // Move a new resident in
            Resident resident = new Resident();

            // place it where the resident is for now
            resident.cellCoordinates = furniture.cellCoordinates;
            appState.Residents.AddResident(resident);

            // add ownership
            FurnitureHomeSlotOccupation homeSlotOccupation = new FurnitureHomeSlotOccupation()
            {
                resident = resident,
                furniture = furniture
            };
            appState.FurnitureHomeSlotOccupations.AddFurnitureHomeSlotOccupation(homeSlotOccupation);
        }
    }
}
