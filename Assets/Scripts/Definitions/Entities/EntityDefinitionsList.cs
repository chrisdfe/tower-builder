using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.Definitions
{
    public interface IEntityDefinitionsList
    {
        public List<EntityDefinition> EntityDefinitions { get; }
        public IQueries Queries { get; }

        public interface IQueries
        {
            public EntityDefinition FindByTitle(string title);

            public List<string> FindAllCategories();

            public string FindFirstCategory();

            public ListWrapper<EntityDefinition> FindByCategory(string category);

            public EntityDefinition FindByKey<DefinitionKeyType>(DefinitionKeyType key) where DefinitionKeyType : struct;

            public EntityDefinition FindFirstInCategory(string category);
        }
    }

    public class EntityDefinitionsList<KeyType, DefinitionType> : IEntityDefinitionsList
        where KeyType : struct
        where DefinitionType : EntityDefinition<KeyType>
    {
        public IEntityDefinitionsList.IQueries Queries { get; }

        public virtual List<DefinitionType> Definitions { get; } = new List<DefinitionType>();

        public List<EntityDefinition> EntityDefinitions =>
            Definitions.ConvertAll<EntityDefinition>(definition => definition as EntityDefinition);

        public EntityDefinition defaultDefinition => Definitions[0];

        public EntityDefinitionsList()
        {
            Queries = new DefinitionQueries(Definitions);
        }

        public class DefinitionQueries : IEntityDefinitionsList.IQueries
        {
            ListWrapper<DefinitionType> Definitions;

            public DefinitionQueries(List<DefinitionType> Definitions)
            {
                this.Definitions = new ListWrapper<DefinitionType>(Definitions);
            }

            public EntityDefinition FindByKey<DefinitionKeyType>(DefinitionKeyType key)
                where DefinitionKeyType : struct =>
                    Definitions.Find(definition => (
                        (definition.key.GetType()) == key.GetType()) &&
                        (Convert.ToInt32(definition.key)).Equals(Convert.ToInt32(key))
                    );

            public EntityDefinition FindByTitle(string title) =>
                Definitions.Find(definition => definition.title == title);

            public List<string> FindAllCategories() =>
                Definitions.items.Aggregate(new HashSet<string>(), (acc, definition) =>
                {
                    acc.Add(definition.category);
                    return acc;
                }).ToList();

            public string FindFirstCategory() => FindAllCategories()[0];

            public ListWrapper<EntityDefinition> FindByCategory(string category) =>
                Definitions.FindAll(definition => definition.category == category)
                           .ConvertAll<EntityDefinition>();

            public EntityDefinition FindFirstInCategory(string category) =>
                Definitions.Find(definition => definition.category == category) as EntityDefinition;
        }
    }
}