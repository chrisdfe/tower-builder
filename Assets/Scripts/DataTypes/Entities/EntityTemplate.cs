using System.Collections.Generic;
using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities
{
    public class EntityTemplate
    {
        public string title = "None";
        public string category = "None";

        public CellCoordinatesList cellCoordinatesList = new CellCoordinatesList(CellCoordinates.zero);
    }

    public class EntityTemplate<KeyType> : EntityTemplate
        where KeyType : struct
    {
        public KeyType key;
    }
}