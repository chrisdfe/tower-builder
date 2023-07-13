using System;
using Newtonsoft.Json;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityDefinitionJsonConverter : JsonConverter<EntityDefinition>
    {
        public override void WriteJson(JsonWriter writer, EntityDefinition entityDefinition, JsonSerializer serializer)
        {
            string definitionKey = Entities.Definitions.entityDefinitionsKeyMap.ValueFromKey(entityDefinition.GetType());
            string typeKey = entityDefinition.key;
            writer.WriteValue($"{definitionKey}:{typeKey}");
        }

        public override EntityDefinition ReadJson(JsonReader reader, Type objectType, EntityDefinition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("I haven't written this yet.");

            // string key = Entities.Definitions.entityDefinitionsKeyMap.ValueFromKey(value.GetType());
            // // string entityDefinitionKey = (string)reader.Value;
            // // return Entities.Definitions.FindByKey(entityDefinitionKey);
            // return new EntityDefinition();
        }
    }
}