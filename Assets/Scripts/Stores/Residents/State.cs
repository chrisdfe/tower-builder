using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Residents
{
    public class State
    {
        public List<Resident> residents { get; private set; } = new List<Resident>();

        public delegate void ResidentsEvent(List<Resident> residents);
        public ResidentsEvent onResidentsUpdated;

        public delegate void ResidentEvent(Resident resident);
        public ResidentEvent onResidentAdded;
        public ResidentEvent onResidentDestroyed;

        public List<ResidentFurnitureOwnership> residentFurnitureOwnerships = new List<ResidentFurnitureOwnership>();

        public void AddResident(Resident resident)
        {
            residents.Add(resident);

            if (onResidentAdded != null)
            {
                onResidentAdded(resident);
            }
        }

        public void RemoveResident(Resident resident)
        {
            residents.Remove(resident);

            if (onResidentDestroyed != null)
            {
                onResidentDestroyed(resident);
            }
        }
    }
}