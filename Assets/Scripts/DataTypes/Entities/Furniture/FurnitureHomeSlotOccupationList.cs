using System.Linq;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class FurnitureHomeSlotOccupationList : ListWrapper<FurnitureHomeSlotOccupation>
    {
        public Furniture GetHomeFurnitureFor(Resident resident)
        {
            FurnitureHomeSlotOccupation slot = items.Find(furnitureHomeSlotOccupation => furnitureHomeSlotOccupation.resident == resident);

            if (slot != null)
            {
                return slot.furniture;
            }

            return null;
        }

        public ListWrapper<Resident> GetResidentsLivingAt(Furniture furniture)
        {
            return new ListWrapper<Resident>(
                items
                    .FindAll(furnitureHomeSlotOccupation => furnitureHomeSlotOccupation.furniture == furniture)
                    .Select(furnitureHomeSlotOccupation => furnitureHomeSlotOccupation.resident)
                    .ToList()
            );
        }
    }
}