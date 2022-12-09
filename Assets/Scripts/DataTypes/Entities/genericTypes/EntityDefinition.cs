using System.Collections.Generic;
using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityDefinition
    {
        public string title = "None";
        public string category = "None";

        public Dimensions blockSize = Dimensions.one;

        public Entity.Resizability resizability = Entity.Resizability.Flexible;

        public CellCoordinatesList cellCoordinatesList = new CellCoordinatesList(CellCoordinates.zero);

        public int pricePerCell = 100;

        public delegate EntityValidator ValidatorFactory(Entity entity);
        public ValidatorFactory validatorFactory = (Entity entity) => new EntityValidator(entity);
    }

    public class EntityDefinition<KeyType> : EntityDefinition
        where KeyType : struct
    {
        public KeyType key;
    }
}