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
        public static EnumStringMap<Type> TypeLabels = new EnumStringMap<Type>(
            new Dictionary<Type, string>() {
                { typeof(Floor),              "Floor" },
                { typeof(Foundation),         "Foundation" },
                { typeof(InteriorLight),      "InteriorLight" },
                { typeof(Resident),           "Resident" },
                { typeof(Furniture),          "Furniture" },
                { typeof(TransportationItem), "Transportation Item" },
                { typeof(FreightItem),        "Freight" },
                { typeof(Wheel),              "Wheel" },
                { typeof(Window),             "Window" }
            }
        );

        public static EnumMap<Type, Type> EntityTypeEntityDefinitionMap =
            new EnumMap<Type, Type>(
                new Dictionary<Type, Type>() {
                    { typeof(Foundation),         typeof(FoundationDefinition) },
                    { typeof(InteriorLight),      typeof(InteriorLightDefinition) },
                    { typeof(Floor),              typeof(FloorDefinition) },
                    { typeof(Furniture),          typeof(FurnitureDefinition) },
                    { typeof(Resident),           typeof(ResidentDefinition) },
                    { typeof(TransportationItem), typeof(TransportationItemDefinition) },
                    { typeof(FreightItem),        typeof(FreightDefinition) },
                    { typeof(Wheel),              typeof(WheelDefinition) },
                    { typeof(Window),             typeof(WindowDefinition) },
                }
            );

        public static string GetEntityDefinitionLabel(EntityDefinition definition) =>
            definition switch
            {
                FloorDefinition floorDefinition =>
                    Floor.KeyLabelMap.ValueFromKey(floorDefinition.key),
                FoundationDefinition foundationDefinition =>
                    Foundation.KeyLabelMap.ValueFromKey(foundationDefinition.key),
                FurnitureDefinition furnitureDefinition =>
                    Furniture.KeyLabelMap.ValueFromKey(furnitureDefinition.key),
                InteriorLightDefinition interiorLightDefinition =>
                    InteriorLight.KeyLabelMap.ValueFromKey(interiorLightDefinition.key),
                ResidentDefinition residentDefinition =>
                    Resident.KeyLabelMap.ValueFromKey(residentDefinition.key),
                TransportationItemDefinition transportationItemDefinition =>
                    TransportationItem.KeyLabelMap.ValueFromKey(transportationItemDefinition.key),
                FreightDefinition freightDefinition =>
                    FreightItem.KeyLabelMap.ValueFromKey(freightDefinition.key),
                WheelDefinition wheelDefinition =>
                    Wheel.KeyLabelMap.ValueFromKey(wheelDefinition.key),
                WindowDefinition windowDefinition =>
                    Window.KeyLabelMap.ValueFromKey(windowDefinition.key),
                _ => null
            };
    }
}