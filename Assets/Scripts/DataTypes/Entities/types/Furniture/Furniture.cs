namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class Furniture : Entity
    {
        public override string idKey { get => "furniture"; }

        public Furniture() : base() { }
        public Furniture(Input input) : base(input) { }
        public Furniture(FurnitureDefinition furnitureDefinition) : base(furnitureDefinition) { }

        public FurnitureBehavior behavior;
    }
}