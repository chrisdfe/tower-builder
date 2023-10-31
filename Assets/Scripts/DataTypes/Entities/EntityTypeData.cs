using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityTypeData
    {
        public Type EntityType;
        public string label = "None";
        public int zIndex = 0;
        public Type definitionType;

        public static EntityTypeData Get(Type type) =>
            Map.ValueFromKey(type);

        public static EntityTypeData FindByDefinition(EntityDefinition entityDefinition) =>
            List.Find(entityTypeData => entityTypeData.definitionType == entityDefinition.GetType());

        public static EntityTypeData FindByLabel(string label) =>
            List.Find(entityTypeData => entityTypeData.label == label);

        public static List<EntityTypeData> List => Map.map.Values.ToList();

        static EnumMap<Type, EntityTypeData> Map =
            new EnumMap<Type, EntityTypeData>(
                new Dictionary<Type, EntityTypeData>() {
                    {
                        typeof(Foundations.Foundation),
                        new EntityTypeData()
                        {
                            EntityType = typeof(Foundations.Foundation),
                            label = "Foundation",
                            zIndex = 0,
                            definitionType = typeof(Foundations.FoundationDefinition)
                        }
                    },
                    {
                        typeof(Floors.Floor),
                        new EntityTypeData()
                        {
                            EntityType = typeof(Floors.Floor),
                            label = "Floor",
                            zIndex = 1,
                            definitionType = typeof(Floors.FloorDefinition)
                        }
                    },
                    {
                        typeof(Windows.Window),
                        new EntityTypeData()
                        {
                            EntityType = typeof(Windows.Window),
                            label = "Window",
                            zIndex = 2,
                            definitionType = typeof(Windows.WindowDefinition)
                        }
                    },
                    {
                        typeof(Freights.FreightItem),
                        new EntityTypeData()
                        {
                            EntityType = typeof(Freights.FreightItem),
                            label = "Freight Item",
                            zIndex = 3,
                            definitionType = typeof(Freights.FreightDefinition)
                        }
                    },
                    {
                        typeof(Furnitures.Furniture),
                        new EntityTypeData()
                        {
                            EntityType = typeof(Furnitures.Furniture),
                            label = "Furniture",
                            zIndex = 4,
                            definitionType = typeof(Furnitures.FurnitureDefinition)
                        }
                    },
                    {
                        typeof(InteriorLights.InteriorLight),
                        new EntityTypeData()
                        {
                            EntityType = typeof(InteriorLights.InteriorLight),
                            label = "Interior Light",
                            zIndex = 5,
                            definitionType = typeof(InteriorLights.InteriorLightDefinition)
                        }
                    },
                    {
                        typeof(Residents.Resident),
                        new EntityTypeData()
                        {
                            EntityType = typeof(Residents.Resident),
                            label = "Resident",
                            zIndex = 6,
                            definitionType = typeof(Residents.ResidentDefinition)
                        }
                    },
                    {
                        typeof(TransportationItems.TransportationItem),
                        new EntityTypeData()
                        {
                            EntityType = typeof(TransportationItems.TransportationItem),
                            label = "Transportation Item",
                            zIndex = 7,
                            definitionType = typeof(TransportationItems.TransportationItemDefinition)
                        }
                    },
                    {
                        typeof(Wheels.Wheel),
                        new EntityTypeData()
                        {
                            EntityType = typeof(Wheels.Wheel),
                            label = "Wheel",
                            zIndex = 8,
                            definitionType = typeof(Wheels.WheelDefinition)
                        }
                    },
                }
            );
    }
}