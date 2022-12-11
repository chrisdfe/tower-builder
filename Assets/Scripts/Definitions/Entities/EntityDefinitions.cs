using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Freights;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.TransportationItems;

namespace TowerBuilder.Definitions
{
    public class EntityDefinitions
    {
        public DefinitionQueries Queries { get; }

        public RoomDefinitionsList Rooms = new RoomDefinitionsList();
        public FurnitureDefinitionsList Furnitures = new FurnitureDefinitionsList();
        public ResidentDefinitionsList Residents = new ResidentDefinitionsList();
        public TransportationItemDefinitionsList TransportationItems = new TransportationItemDefinitionsList();
        public FreightDefinitionsList Freights = new FreightDefinitionsList();

        public EntityDefinitions()
        {
            Queries = new DefinitionQueries(this);
        }

        public class DefinitionQueries
        {
            EntityDefinitions entityDefinitions;

            public DefinitionQueries(EntityDefinitions entityDefinitions)
            {
                this.entityDefinitions = entityDefinitions;
            }

            public List<string> FindAllCategories(Entity.Type type) =>
                type switch
                {
                    Entity.Type.Room =>
                        entityDefinitions.Rooms.Queries.FindAllCategories(),
                    Entity.Type.Furniture =>
                        entityDefinitions.Furnitures.Queries.FindAllCategories(),
                    Entity.Type.Resident =>
                        entityDefinitions.Residents.Queries.FindAllCategories(),
                    Entity.Type.TransportationItem =>
                        entityDefinitions.TransportationItems.Queries.FindAllCategories(),
                    Entity.Type.Freight =>
                        entityDefinitions.Freights.Queries.FindAllCategories(),
                    _ => null
                };

            public List<EntityDefinition> FindByCategory(Entity.Type type, string category) =>
                type switch
                {
                    Entity.Type.Room =>
                        entityDefinitions.Rooms.Queries.FindByCategory<EntityDefinition>(category),
                    Entity.Type.Furniture =>
                        entityDefinitions.Furnitures.Queries.FindByCategory<EntityDefinition>(category),
                    Entity.Type.Resident =>
                        entityDefinitions.Residents.Queries.FindByCategory<EntityDefinition>(category),
                    Entity.Type.TransportationItem =>
                        entityDefinitions.TransportationItems.Queries.FindByCategory<EntityDefinition>(category),
                    Entity.Type.Freight =>
                        entityDefinitions.Freights.Queries.FindByCategory<EntityDefinition>(category),
                    _ => null
                };

            public string FindFirstCategory(Entity.Type type) => FindAllCategories(type)[0];

            public EntityDefinition FindFirstInCategory(Entity.Type type, string category) =>
                type switch
                {
                    Entity.Type.Room =>
                        entityDefinitions.Rooms.Queries.FindFirstInCategory(category),
                    Entity.Type.Furniture =>
                        entityDefinitions.Furnitures.Queries.FindFirstInCategory(category),
                    Entity.Type.Resident =>
                        entityDefinitions.Residents.Queries.FindFirstInCategory(category),
                    Entity.Type.TransportationItem =>
                        entityDefinitions.TransportationItems.Queries.FindFirstInCategory(category),
                    Entity.Type.Freight =>
                        entityDefinitions.Freights.Queries.FindFirstInCategory(category),
                    _ => null
                };

            public EntityDefinition FindDefinitionByKeyLabel(Entity.Type type, string keyLabel)
            {
                switch (type)
                {
                    case Entity.Type.Room:
                        Room.Key roomKey = Room.KeyLabelMap.KeyFromValue(keyLabel);
                        return entityDefinitions.Rooms.Queries.FindByKey(roomKey);
                    case Entity.Type.Furniture:
                        Furniture.Key furnitureKey = Furniture.KeyLabelMap.KeyFromValue(keyLabel);
                        return entityDefinitions.Furnitures.Queries.FindByKey(furnitureKey);
                    case Entity.Type.Resident:
                        Resident.Key residentKey = Resident.KeyLabelMap.KeyFromValue(keyLabel);
                        return entityDefinitions.Residents.Queries.FindByKey(residentKey);
                    case Entity.Type.TransportationItem:
                        TransportationItem.Key transportationItemKey = TransportationItem.KeyLabelMap.KeyFromValue(keyLabel);
                        return entityDefinitions.TransportationItems.Queries.FindByKey(transportationItemKey);
                    case Entity.Type.Freight:
                        FreightItem.Key freightItemKey = FreightItem.KeyLabelMap.KeyFromValue(keyLabel);
                        return entityDefinitions.Freights.Queries.FindByKey(freightItemKey);
                }

                return null;
            }
        }
    }
}