using System.Collections.Generic;
using TowerBuilder.DataTypes.Furnitures.Behaviors;

namespace TowerBuilder.DataTypes.Furnitures
{
    public class FurnitureTemplate
    {
        public string key = "None";
        public string title = "None";
        public string category = "None";

        // public List<CellCoordinates> cells = new List<CellCoordinates>();

        // public List<FurnitureAttributesBase> attributes = new List<FurnitureAttributesBase>();

        public delegate FurnitureBehaviorBase FurnitureBehaviorFactory(Furniture furniture);
        public FurnitureBehaviorFactory furnitureBehaviorFactory = (Furniture furniture) => new DefaultBehavior(furniture);
    }
}