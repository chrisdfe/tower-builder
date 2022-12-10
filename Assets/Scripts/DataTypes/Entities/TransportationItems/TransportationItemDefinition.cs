using System.Collections.Generic;
using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities.TransportationItems
{
    public class TransportationItemDefinition : EntityDefinition<TransportationItem.Key>
    {
        public delegate List<(CellCoordinates, CellCoordinates)> EntranceExitBuilder(TransportationItem transportationItem);
        public EntranceExitBuilder entranceExitBuilder = (TransportationItem transportationItem) => new List<(CellCoordinates, CellCoordinates)>();
    }
}