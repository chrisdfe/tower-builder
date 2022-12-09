using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.Definitions
{
    public class EntityDefinitionsList<KeyType, EntityType, DefinitionType>
        where KeyType : struct
        where EntityType : Entity<KeyType>
        where DefinitionType : EntityTemplate<KeyType>
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

            public DefinitionType FindByKey(KeyType key)
            {
                return Definitions.Find(definition => definition.key.Equals(key));
            }

            public List<DefinitionType> FindByCategory(string category)
            {
                return Definitions.FindAll(definition => definition.category == category);
            }

            public List<string> FindAllCategories()
            {
                List<string> result = new List<string>();

                foreach (DefinitionType DefinitionType in Definitions)
                {
                    if (!result.Contains(DefinitionType.category))
                    {
                        result.Add(DefinitionType.category);
                    }
                }

                return result;
            }
        }
    }
}