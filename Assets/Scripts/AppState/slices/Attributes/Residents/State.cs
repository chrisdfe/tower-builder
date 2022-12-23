using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Attributes.Residents;
using TowerBuilder.DataTypes.Entities.Residents;

namespace TowerBuilder.ApplicationState.Attributes.Residents
{
    using ResidentAtributesStateSlice = AttributesStateSlice<
        ResidentAttribute.Key,
        ResidentAttributesGroup,
        ResidentAttribute,
        ResidentAttribute.Modifier,
        State.Events
    >;

    public class State : ResidentAtributesStateSlice
    {
        public class Input
        {
            ListWrapper<ResidentAttributesGroup> list;
        }

        public new class Events : ResidentAtributesStateSlice.Events { }

        public class Queries
        {
            State state;
            AppState appState;

            public Queries(AppState appState, State state)
            {
                this.appState = appState;
                this.state = state;
            }

            public ResidentAttributesGroup FindByResident(Resident resident) =>
                state.list.Find(attribute => attribute.resident == resident);
        }

        public Queries queries { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public override void Setup()
        {
            base.Setup();

            appState.Entities.Residents.events.onItemsAdded += OnResidentsAdded;
            appState.Entities.Residents.events.onItemsRemoved += OnResidentsRemoved;
            appState.Entities.Residents.events.onItemsBuilt += OnResidentsBuilt;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.Entities.Residents.events.onItemsAdded -= OnResidentsAdded;
            appState.Entities.Residents.events.onItemsRemoved -= OnResidentsRemoved;
            appState.Entities.Residents.events.onItemsBuilt -= OnResidentsBuilt;
        }

        /* 
            Public Interface
         */
        public void AddAttributesForResident(Resident resident)
        {
            ResidentAttributesGroup residentAttributesGroup = new ResidentAttributesGroup(appState, resident);
            Add(residentAttributesGroup);
        }

        public void RemoveAttributesForResident(Resident resident)
        {
            ResidentAttributesGroup residentAttributesGroup = queries.FindByResident(resident);

            if (residentAttributesGroup != null)
            {
                Remove(residentAttributesGroup);
            }
        }

        /* 
            Event handlers
         */
        void OnResidentsAdded(ListWrapper<Resident> residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                if (!resident.isInBlueprintMode)
                {
                    AddAttributesForResident(resident);
                }
            }
        }

        void OnResidentsBuilt(ListWrapper<Resident> residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                AddAttributesForResident(resident);
            }
        }

        void OnResidentsRemoved(ListWrapper<Resident> residentsList)
        {
            foreach (Resident resident in residentsList.items)
            {
                RemoveAttributesForResident(resident);
            }
        }
    }
}