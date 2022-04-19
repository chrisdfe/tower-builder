using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.Stores.Rooms.Furniture
{
    public static class Constants
    {
        // 5 x 5 x 5 for now
        public static RoomFurnitureCoordinates ROOM_CELL_SUBDIVISIONS = new RoomFurnitureCoordinates(5, 5, 5);

        public static List<RoomFurnitureTemplate> ROOM_FURNITURE_DEFINITIONS = new List<RoomFurnitureTemplate>()
        {
            new RoomFurnitureTemplate() {
                title = "Bed",
                category = "Sleeping",
                cells = new List<RoomFurnitureCoordinates>() {
                    new RoomFurnitureCoordinates(0, 0, 0),
                    new RoomFurnitureCoordinates(0, 0, 1)
                }
            },

            new RoomFurnitureTemplate() {
                title = "Desk",
                category = "Work",
                cells = new List<RoomFurnitureCoordinates>() {
                    new RoomFurnitureCoordinates(0, 0, 0)
                }
            }
        };
    }
}