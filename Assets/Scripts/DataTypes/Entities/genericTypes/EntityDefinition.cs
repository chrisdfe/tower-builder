using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Validators;
using TowerBuilder.DataTypes.Validators.Entities;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityDefinition
    {
        public virtual string title { get; set; } = "None";
        public virtual string category { get; set; } = "None";

        public virtual Entity.Resizability resizability { get; set; } = Entity.Resizability.Flexible;

        public virtual CellCoordinatesList blockCellsTemplate { get; set; } = new CellCoordinatesList();
        public virtual bool staticBlockSize { get; set; } = true;

        public virtual int pricePerCell { get; set; } = 100;

        public virtual int[] zLayers { get; set; } = new[] { 0 };

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