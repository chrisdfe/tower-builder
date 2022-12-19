using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Freights;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.InteriorWalls;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;
using TowerBuilder.DataTypes.Entities.Wheels;

namespace TowerBuilder.Definitions
{
    public class EntityDefinitions
    {
        public static Dictionary<System.Type, System.Type> EntityDefinitionEntityTypeMap = new Dictionary<System.Type, System.Type>() {
            { typeof(RoomDefinition), typeof(Room) },
            { typeof(InteriorWallDefinition), typeof(InteriorWall) },
            { typeof(FloorDefinition), typeof(Floor) },
            { typeof(FurnitureDefinition), typeof(Furniture) },
            { typeof(ResidentDefinition), typeof(Resident) },
            { typeof(TransportationItemDefinition), typeof(TransportationItem) },
            { typeof(FreightDefinition), typeof(FreightItem) },
            { typeof(WheelDefinition), typeof(Wheel) },
        };

        public DefinitionQueries Queries { get; }

        public RoomDefinitionsList Rooms = new RoomDefinitionsList();
        public FloorDefinitionsList Floors = new FloorDefinitionsList();
        public InteriorWallDefinitionsList InteriorWalls = new InteriorWallDefinitionsList();
        public FurnitureDefinitionsList Furnitures = new FurnitureDefinitionsList();
        public ResidentDefinitionsList Residents = new ResidentDefinitionsList();
        public TransportationItemDefinitionsList TransportationItems = new TransportationItemDefinitionsList();
        public FreightDefinitionsList Freights = new FreightDefinitionsList();
        public WheelDefinitionsList Wheels = new WheelDefinitionsList();

        public Dictionary<Entity.Type, IEntityDefinitionsList> entityDefinitionsMap { get; }

        public EntityDefinitions()
        {
            Queries = new DefinitionQueries(this);

            entityDefinitionsMap = new Dictionary<Entity.Type, IEntityDefinitionsList>() {
                { Entity.Type.Room, Rooms },
                { Entity.Type.Floor, Floors },
                { Entity.Type.InteriorWall, InteriorWalls },
                { Entity.Type.Furniture, Furnitures },
                { Entity.Type.Resident, Residents },
                { Entity.Type.TransportationItem, TransportationItems },
                { Entity.Type.Freight, Freights },
                { Entity.Type.Wheel, Wheels },
            };
        }

        IEntityDefinitionsList DefinitionFromEntityType(Entity.Type type) =>
            entityDefinitionsMap[type];

        public class DefinitionQueries
        {
            EntityDefinitions Definitions;

            public DefinitionQueries(EntityDefinitions entityDefinitions)
            {
                this.Definitions = entityDefinitions;
            }

            public List<string> FindAllCategories(Entity.Type type) =>
                Definitions.DefinitionFromEntityType(type)?.Queries.FindAllCategories();

            public ListWrapper<EntityDefinition> FindByCategory(Entity.Type type, string category) =>
                Definitions.DefinitionFromEntityType(type)?.Queries.FindByCategory(category);

            public string FindFirstCategory(Entity.Type type) => FindAllCategories(type)[0];

            public EntityDefinition FindFirstInCategory(Entity.Type type, string category) =>
                Definitions.DefinitionFromEntityType(type)?.Queries.FindFirstInCategory(category);

            public EntityDefinition FindByKey<KeyType>(Entity.Type type, KeyType key)
                where KeyType : struct =>
                Definitions.DefinitionFromEntityType(type)?.Queries.FindByKey<KeyType>(key);


            public EntityDefinition FindDefinitionByKeyLabel(Entity.Type type, string keyLabel)
            {
                switch (type)
                {
                    case Entity.Type.Room:
                        Room.Key roomKey = Room.KeyLabelMap.KeyFromValue(keyLabel);
                        return Definitions.Rooms.Queries.FindByKey(roomKey);
                    case Entity.Type.InteriorWall:
                        InteriorWall.Key interiorWallKey = InteriorWall.KeyLabelMap.KeyFromValue(keyLabel);
                        return Definitions.InteriorWalls.Queries.FindByKey(interiorWallKey);
                    case Entity.Type.Floor:
                        Floor.Key floorKey = Floor.KeyLabelMap.KeyFromValue(keyLabel);
                        return Definitions.Floors.Queries.FindByKey(floorKey);
                    case Entity.Type.Furniture:
                        Furniture.Key furnitureKey = Furniture.KeyLabelMap.KeyFromValue(keyLabel);
                        return Definitions.Furnitures.Queries.FindByKey(furnitureKey);
                    case Entity.Type.Resident:
                        Resident.Key residentKey = Resident.KeyLabelMap.KeyFromValue(keyLabel);
                        return Definitions.Residents.Queries.FindByKey(residentKey);
                    case Entity.Type.TransportationItem:
                        TransportationItem.Key transportationItemKey = TransportationItem.KeyLabelMap.KeyFromValue(keyLabel);
                        return Definitions.TransportationItems.Queries.FindByKey(transportationItemKey);
                    case Entity.Type.Freight:
                        FreightItem.Key freightItemKey = FreightItem.KeyLabelMap.KeyFromValue(keyLabel);
                        return Definitions.Freights.Queries.FindByKey(freightItemKey);
                }

                return null;
            }
        }
    }
}