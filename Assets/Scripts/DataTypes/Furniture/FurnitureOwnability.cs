using TowerBuilder.DataTypes.Residents;

namespace TowerBuilder.DataTypes.Furnitures
{
    public class FurnitureOwnabilityBase { }

    public class FurnitureOwnabilityUnownable : FurnitureOwnabilityBase { }

    public class FurnitureOwnabilityOwnable : FurnitureOwnabilityBase
    {
        public Resident resident;
    }
}