using System.Collections.Generic;
using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityDefinition
    {
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
        public virtual ValidatorFactory validatorFactory => (Entity entity) => new EntityValidator(entity);
    }
}