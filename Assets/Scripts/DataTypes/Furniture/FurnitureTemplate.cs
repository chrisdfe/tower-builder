using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Furniture
{
    public class FurnitureTemplate
    {
        public string title = "None";
        public string category = "None";

        // the room 'sub cells' that this furniture takes up
        public List<FurnitureCoordinates> cells = new List<FurnitureCoordinates>();

        public List<FurnitureAttributesBase> attributes = new List<FurnitureAttributesBase>();
    }
}