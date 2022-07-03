using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Rooms.Furniture
{
    public class RoomFurnitureTemplate
    {
        public string title = "None";
        public string category = "None";

        // the room 'sub cells' that this furniture takes up
        public List<RoomFurnitureCoordinates> cells = new List<RoomFurnitureCoordinates>();

        public List<RoomFurnitureAttributesBase> attributes = new List<RoomFurnitureAttributesBase>();
    }
}