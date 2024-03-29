using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Residents;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class EngineBehavior : FurnitureBehavior
    {
        public override Key key => FurnitureBehavior.Key.Engine;

        public EngineBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }

        // Vehicle vehicle => appState.Entities.Vehicles.queries.FindVehicleByFurniture(furniture);
        // VehicleAttributes vehicleAttributes => appState.Attributes.Vehicles.queries.FindByVehicle(vehicle);
        AttributeModifier baseEnginePowerModifier;
        AttributeModifier mannedEngineModifier;

        public override void Setup()
        {
            base.Setup();

            baseEnginePowerModifier = new AttributeModifier("Base Engine Power", 1f);
            // AddStaticModifier(baseEnginePowerModifier);
        }

        public override void Teardown()
        {
            base.Teardown();

            // RemoveStaticModifier(baseEnginePowerModifier);
            baseEnginePowerModifier = null;
        }

        protected override void OnInteractStart(Resident resident)
        {
            mannedEngineModifier = new AttributeModifier("Manned Engine Power", 1f);
            // AddStaticModifier(mannedEngineModifier);
        }

        protected override void OnInteractEnd(Resident resident)
        {
            // RemoveStaticModifier(mannedEngineModifier);
            mannedEngineModifier = null;
        }

        // void AddStaticModifier(AttributeModifier modifier) =>
        //     appState.Attributes.Vehicles.AddStaticAttributeModifier(vehicleAttributes, VehicleAttributes.Key.EnginePower, modifier);

        // void RemoveStaticModifier(AttributeModifier modifier) =>
        //     appState.Attributes.Vehicles.RemoveStaticAttributeModifier(vehicleAttributes, VehicleAttributes.Key.EnginePower, modifier);
    }
}