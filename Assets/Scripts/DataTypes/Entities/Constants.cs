using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Foundations;
using TowerBuilder.DataTypes.Entities.Freights;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.InteriorLights;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.Entities.Wheels;
using TowerBuilder.DataTypes.Entities.Windows;

namespace TowerBuilder.DataTypes.Entities
{
    public static class Constants
    {
        // In cells
        public const int FLOOR_HEIGHT = 3;

        public static EnumMap<string, Type> entityTypeKeyTypeMap =
            new EnumMap<string, Type>(
                new Dictionary<string, Type>()
                {
                    { "Entity",             typeof(Entity) },
                    { "Floor",              typeof(Floor) },
                    { "Foundation",         typeof(Foundation) },
                    { "FreightItem",        typeof(FreightItem) },
                    { "Furniture",          typeof(Furniture) },
                    { "InteriorLight",      typeof(InteriorLight) },
                    { "Resident",           typeof(Resident) },
                    { "TransportationItem", typeof(TransportationItem) },
                    { "Wheel",              typeof(Wheel) },
                    { "Window",             typeof(Window) },
                }
            );
    }
}