using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Freights;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.InteriorLights;
using TowerBuilder.DataTypes.Entities.InteriorWalls;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.Entities.Vehicles;
using TowerBuilder.DataTypes.Entities.Wheels;
using TowerBuilder.DataTypes.Entities.Windows;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class EntityDefinitions
    {
        public static Dictionary<System.Type, System.Type> EntityDefinitionEntityTypeMap = new Dictionary<System.Type, System.Type>() {
            { typeof(RoomDefinition), typeof(Room) },
            { typeof(InteriorWallDefinition), typeof(InteriorWall) },
            { typeof(InteriorLightDefinition), typeof(InteriorLight) },
            { typeof(FloorDefinition), typeof(Floor) },
            { typeof(FurnitureDefinition), typeof(Furniture) },
            { typeof(ResidentDefinition), typeof(Resident) },
            { typeof(TransportationItemDefinition), typeof(TransportationItem) },
            { typeof(FreightDefinition), typeof(FreightItem) },
            { typeof(WheelDefinition), typeof(Wheel) },
            { typeof(VehicleDefinition), typeof(Vehicle) },
            { typeof(WindowDefinition), typeof(Window) },
        };

        public DefinitionQueries Queries { get; }

        public RoomDefinitionsList Rooms = new RoomDefinitionsList();
        public FloorDefinitionsList Floors = new FloorDefinitionsList();
        public InteriorWallDefinitionsList InteriorWalls = new InteriorWallDefinitionsList();
        public InteriorLightDefinitionsList InteriorLights = new InteriorLightDefinitionsList();
        public FurnitureDefinitionsList Furnitures = new FurnitureDefinitionsList();
        public ResidentDefinitionsList Residents = new ResidentDefinitionsList();
        public TransportationItemDefinitionsList TransportationItems = new TransportationItemDefinitionsList();
        public FreightDefinitionsList Freights = new FreightDefinitionsList();
        public WheelDefinitionsList Wheels = new WheelDefinitionsList();
        public VehicleDefinitionsList Vehicles = new VehicleDefinitionsList();
        public WindowDefinitionsList Windows = new WindowDefinitionsList();

        public Dictionary<Type, IEntityDefinitionsList> entityDefinitionsMap { get; }

        public EntityDefinitions()
        {
            Queries = new DefinitionQueries(this);

            entityDefinitionsMap = new Dictionary<Type, IEntityDefinitionsList>() {
                { typeof(Room),               Rooms },
                { typeof(Floor),              Floors },
                { typeof(InteriorWall),       InteriorWalls },
                { typeof(InteriorLight),      InteriorLights },
                { typeof(Furniture),          Furnitures },
                { typeof(Resident),           Residents },
                { typeof(TransportationItem), TransportationItems },
                { typeof(FreightItem),        Freights },
                { typeof(Wheel),              Wheels },
                { typeof(Vehicle),            Vehicles },
                { typeof(Window),             Windows },
            };
        }

        IEntityDefinitionsList DefinitionFromEntityType(Type type) =>
            entityDefinitionsMap[type];

        public class DefinitionQueries
        {
            EntityDefinitions Definitions;

            public DefinitionQueries(EntityDefinitions entityDefinitions)
            {
                this.Definitions = entityDefinitions;
            }

            public List<string> FindAllCategories(Type type) =>
                Definitions.DefinitionFromEntityType(type)?.Queries.FindAllCategories();

            public ListWrapper<EntityDefinition> FindByCategory(Type type, string category) =>
                Definitions.DefinitionFromEntityType(type)?.Queries.FindByCategory(category);

            public string FindFirstCategory(Type type) => FindAllCategories(type)[0];

            public EntityDefinition FindFirstInCategory(Type type, string category) =>
                Definitions.DefinitionFromEntityType(type)?.Queries.FindFirstInCategory(category);

            public EntityDefinition FindByKey<KeyType>(Type type, KeyType key)
                where KeyType : struct =>
                Definitions.DefinitionFromEntityType(type)?.Queries.FindByKey<KeyType>(key);


            public EntityDefinition FindDefinitionByKeyLabel(EntityDefinition entityDefinition, string keyLabel) =>
                entityDefinition switch
                {
                    RoomDefinition =>
                        Definitions.Rooms.Queries.FindByKey(Room.KeyLabelMap.KeyFromValue(keyLabel)),
                    InteriorWallDefinition =>
                        Definitions.InteriorWalls.Queries.FindByKey(InteriorWall.KeyLabelMap.KeyFromValue(keyLabel)),
                    FloorDefinition =>
                        Definitions.Floors.Queries.FindByKey(Floor.KeyLabelMap.KeyFromValue(keyLabel)),
                    FurnitureDefinition =>
                        Definitions.Furnitures.Queries.FindByKey(Furniture.KeyLabelMap.KeyFromValue(keyLabel)),
                    ResidentDefinition =>
                        Definitions.Residents.Queries.FindByKey(Resident.KeyLabelMap.KeyFromValue(keyLabel)),
                    TransportationItemDefinition =>
                        Definitions.TransportationItems.Queries.FindByKey(TransportationItem.KeyLabelMap.KeyFromValue(keyLabel)),
                    FreightDefinition =>
                        Definitions.Freights.Queries.FindByKey(FreightItem.KeyLabelMap.KeyFromValue(keyLabel)),
                    VehicleDefinition =>
                        Definitions.Vehicles.Queries.FindByKey(Vehicle.KeyLabelMap.KeyFromValue(keyLabel)),
                    WindowDefinition =>
                        Definitions.Windows.Queries.FindByKey(Window.KeyLabelMap.KeyFromValue(keyLabel)),
                    _ => null
                };
        }
    }
}