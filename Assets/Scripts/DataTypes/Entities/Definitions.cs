using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Foundations;
using TowerBuilder.DataTypes.Entities.Freights;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.InteriorLights;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.Entities.Wheels;
using TowerBuilder.DataTypes.Entities.Windows;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public static class Definitions
    {
        public static FoundationDefinitionsList Foundations = new FoundationDefinitionsList();
        public static FloorDefinitionsList Floors = new FloorDefinitionsList();
        public static InteriorLightDefinitionsList InteriorLights = new InteriorLightDefinitionsList();
        public static FurnitureDefinitionsList Furnitures = new FurnitureDefinitionsList();
        public static ResidentDefinitionsList Residents = new ResidentDefinitionsList();
        public static TransportationItemDefinitionsList TransportationItems = new TransportationItemDefinitionsList();
        public static FreightDefinitionsList Freights = new FreightDefinitionsList();
        public static WheelDefinitionsList Wheels = new WheelDefinitionsList();
        public static WindowDefinitionsList Windows = new WindowDefinitionsList();

        public static Dictionary<Type, EntityDefinitionsList> entityDefinitionsMap =>
            new Dictionary<Type, EntityDefinitionsList>() {
                { typeof(Foundation),         Foundations },
                { typeof(Floor),              Floors },
                { typeof(InteriorLight),      InteriorLights },
                { typeof(Furniture),          Furnitures },
                { typeof(Resident),           Residents },
                { typeof(TransportationItem), TransportationItems },
                { typeof(FreightItem),        Freights },
                { typeof(Wheel),              Wheels },
                { typeof(Window),             Windows },
            };

        public static List<string> FindAllCategories(Type type) =>
            DefinitionFromEntityType(type)?.FindAllCategories();

        public static EntityDefinition FindByKey(Type type, string key) =>
            DefinitionFromEntityType(type)?.FindByKey(key);

        public static ListWrapper<EntityDefinition> FindByCategory(Type type, string category) =>
            DefinitionFromEntityType(type)?.FindByCategory(category);

        public static string FindFirstCategory(Type type) => FindAllCategories(type)[0];

        public static EntityDefinition FindFirstInCategory(Type type, string category) =>
            DefinitionFromEntityType(type)?.FindFirstInCategory(category);

        static EntityDefinitionsList DefinitionFromEntityType(Type type) =>
            entityDefinitionsMap[type];

    }
}