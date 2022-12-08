using System.Collections.Generic;
using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityTemplate
    {
        public string title = "None";
        public string category = "None";

        public Dimensions blockSize = Dimensions.one;

        public Entity.Resizability resizability = Entity.Resizability.Flexible;

        public CellCoordinatesList cellCoordinatesList = new CellCoordinatesList(CellCoordinates.zero);

        public int pricePerCell = 100;
    }

    public class EntityTemplate<KeyType> : EntityTemplate
        where KeyType : struct
    {
        public KeyType key;
    }
}