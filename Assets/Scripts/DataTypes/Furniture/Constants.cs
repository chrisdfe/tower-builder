using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes.Furniture
{
    public static class Constants
    {
        // 5 x 5 x 5 for now
        public static FurnitureCoordinates ROOM_CELL_SUBDIVISIONS = new FurnitureCoordinates(5, 5, 5);

        public static List<FurnitureTemplate> ROOM_FURNITURE_DEFINITIONS = new List<FurnitureTemplate>()
        {
            new FurnitureTemplate() {
                title = "Bed",
                category = "Sleeping",
                cells = new List<FurnitureCoordinates>() {
                    new FurnitureCoordinates(0, 0, 0),
                    new FurnitureCoordinates(0, 0, 1)
                }
            },

            new FurnitureTemplate() {
                title = "Desk",
                category = "Work",
                cells = new List<FurnitureCoordinates>() {
                    new FurnitureCoordinates(0, 0, 0)
                }
            }
        };
    }
}