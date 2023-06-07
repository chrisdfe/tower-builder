using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityDefinitionsList
    {
        public virtual List<EntityDefinition> Definitions { get; } = new List<EntityDefinition>();

        public EntityDefinition defaultDefinition => Definitions[0];

        public EntityDefinition FindByKey(string key) =>
            Definitions.Find(definition => (definition.key == key));

        public EntityDefinition FindByTitle(string title) =>
            Definitions.Find(definition => definition.title == title);

        public List<string> FindAllCategories() =>
            Definitions.Aggregate(new HashSet<string>(), (acc, definition) =>
            {
                acc.Add(definition.category);
                return acc;
            }).ToList();

        public string FindFirstCategory() => FindAllCategories()[0];

        public ListWrapper<EntityDefinition> FindByCategory(string category) =>
            new ListWrapper<EntityDefinition>(
                Definitions.FindAll(definition => definition.category == category)
            );

        public EntityDefinition FindFirstInCategory(string category) =>
            Definitions.Find(definition => definition.category == category) as EntityDefinition;
    }
}