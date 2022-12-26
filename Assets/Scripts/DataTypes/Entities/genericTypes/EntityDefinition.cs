using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Validators;
using TowerBuilder.DataTypes.Validators.Entities;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityDefinition
    {
        public string title = "None";
        public string category = "None";

        public Entity.Resizability resizability = Entity.Resizability.Flexible;

        public CellCoordinatesList blockCellsTemplate = new CellCoordinatesList();
        public bool staticBlockSize = true;

        public int pricePerCell = 100;

        public int[] zLayers = new[] { 0 };

        // public delegate AttributeModifierCreatorFactory

        public delegate EntityValidator ValidatorFactory(Entity entity);
        public virtual ValidatorFactory validatorFactory => (Entity entity) => new EntityValidator(entity);
    }

    public class EntityDefinition<KeyType> : EntityDefinition
        where KeyType : struct
    {
        public KeyType key;
    }
}