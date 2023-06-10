using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes.Residents;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Residents;

namespace TowerBuilder.ApplicationState.Attributes.Residents
{
    using ResidentAtributesStateSlice = AttributesStateSlice<ResidentAttributes>;

    public class State : ResidentAtributesStateSlice
    {
        public class Input
        {
            ListWrapper<ResidentAttributes> list;
        }

        public State(AppState appState, Input input) : base(appState)
        {
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public override void Setup()
        {
            base.Setup();

            appState.Entities.Residents.onItemsAdded += OnResidentsAdded;
            appState.Entities.Residents.onItemsRemoved += OnResidentsRemoved;
            appState.Entities.Residents.onItemsBuilt += OnResidentsBuilt;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.Residents.onItemsAdded -= OnResidentsAdded;
            appState.Entities.Residents.onItemsRemoved -= OnResidentsRemoved;
            appState.Entities.Residents.onItemsBuilt -= OnResidentsBuilt;
        }

        /* 
            Public Interface
         */
        public void AddAttributesForResident(Resident resident)
        {
            ResidentAttributes residentAttributes = new ResidentAttributes(appState, resident);
            Add(residentAttributes);
        }

        public void RemoveAttributesForResident(Resident resident)
        {
            ResidentAttributes residentAttributes = FindByResident(resident);

            if (residentAttributes != null)
            {
                Remove(residentAttributes);
            }
        }

        /*
            Queries
        */
        public ResidentAttributes FindByResident(Resident resident) =>
            list.Find(attribute => attribute.resident == resident);

        /* 
            Event handlers
         */
        void OnResidentsAdded(ListWrapper<Entity> residentsList)
        {
            foreach (Entity resident in residentsList.items)
            {
                if (!resident.isInBlueprintMode)
                {
                    AddAttributesForResident(resident as Resident);
                }
            }
        }

        void OnResidentsBuilt(ListWrapper<Entity> residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                AddAttributesForResident(resident as Resident);
            }
        }

        void OnResidentsRemoved(ListWrapper<Entity> residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                RemoveAttributesForResident(resident as Resident);
            }
        }
    }
}