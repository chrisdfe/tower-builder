using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.Definitions
{
    public class EntityDefinitionsList { }

    public class EntityDefinitionsList<KeyType, EntityType, DefinitionType> : EntityDefinitionsList
        where KeyType : struct
        where EntityType : Entity<KeyType>
        where DefinitionType : EntityDefinition<KeyType>
    {
        public DefinitionQueries Queries { get; }

        public virtual List<DefinitionType> Definitions { get; } = new List<DefinitionType>();

        public EntityDefinitionsList()
        {
            Queries = new DefinitionQueries(Definitions);
        }

        public class DefinitionQueries
        {
            List<DefinitionType> Definitions;

            public DefinitionQueries(List<DefinitionType> Definitions)
            {
                this.Definitions = Definitions;
            }

            public DefinitionType FindByKey(KeyType key) =>
                Definitions.Find(definition => definition.key.Equals(key));

            public DefinitionType FindByTitle(string title) =>
                Definitions.Find(definition => definition.title == title);

            public List<string> FindAllCategories() =>
                Definitions.Aggregate(new HashSet<string>(), (acc, definition) =>
                {
                    acc.Add(definition.category);
                    return acc;
                }).ToList();

            public string FindFirstCategory() => FindAllCategories()[0];

            public List<DefinitionType> FindByCategory(string category) =>
                Definitions.FindAll(definition => definition.category == category);

            public List<CategoryDefinitionType> FindByCategory<CategoryDefinitionType>(string category) where CategoryDefinitionType : EntityDefinition =>
                FindByCategory(category).Select(definition => definition as CategoryDefinitionType).ToList();

            public DefinitionType FindFirstInCategory(string category) =>
                Definitions.Find(definition => definition.category == category);

        }
    }
}