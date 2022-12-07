using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Routes;

using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Residents
{
    public class ResidentsList : ListWrapper<Resident>
    {
        public ResidentsList() : base() { }
        public ResidentsList(Resident resident) : base(resident) { }
        public ResidentsList(List<Resident> residentsList) : base(residentsList) { }
        public ResidentsList(ResidentsList residentsList) : base(residentsList) { }

        public Resident FindResidentAtCell(CellCoordinates cellCoordinates)
        {
            return items.Find(resident => resident.cellCoordinates.Matches(cellCoordinates));
        }
    }
}