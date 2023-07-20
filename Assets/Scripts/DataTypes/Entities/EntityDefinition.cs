using System.Collections.Generic;
using Newtonsoft.Json;
using TowerBuilder.DataTypes;
using TowerBuilder.Systems;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityDefinition : ISaveable<EntityDefinition.Input>
    {
        public class Input
        {
            public string referenceKey;
        }

        public class Fragment
        {
            public string definitionKey;
            public string key;

            public static Fragment FromInput(Input input)
            {
                string[] pieces = input.referenceKey.Split(":");

                return new Fragment()
                {
                    definitionKey = pieces[0],
                    key = pieces[1]
                };
            }
        }

        public virtual string key { get; set; } = null;

        public virtual string title { get; set; } = "None";

        public virtual string category { get; set; } = "None";

        public virtual string meshKey { get; set; } = "Default";

        public virtual Resizability resizability { get; set; } = Resizability.Flexible;

        public virtual CellCoordinatesList blockCellsTemplate { get; set; } = new CellCoordinatesList();
        public virtual bool staticBlockSize { get; set; } = true;

        public virtual int pricePerCell { get; set; } = 100;

        public virtual int[] zLayers { get; set; } = new[] { 0 };

        public delegate EntityValidator ValidatorFactory(Entity entity);
        public virtual ValidatorFactory buildValidatorFactory => (Entity entity) => new EmptyEntityValidator(entity);
        public virtual ValidatorFactory destroyValidatorFactory => (Entity entity) => new EmptyEntityValidator(entity);

        public string typeKey =>
            Definitions.entityDefinitionsKeyMap.ValueFromKey(this.GetType());

        public Input ToInput() =>
            new Input()
            {
                referenceKey = $"{typeKey}:{key}"
            };

        public void ConsumeInput(Input input)
        {
            throw new JsonSerializationException("Entities.FindDefinitionByInput instead.");
        }
    }
}