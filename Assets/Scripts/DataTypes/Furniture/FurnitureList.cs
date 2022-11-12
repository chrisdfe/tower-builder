using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Rooms;

namespace TowerBuilder.DataTypes.Furnitures
{
    public class FurnitureList
    {
        public List<Furniture> items { get; private set; } = new List<Furniture>();

        public int Count { get { return items.Count; } }

        public FurnitureList() { }

        public FurnitureList(Furniture furniture)
        {
            this.items = new List<Furniture> { furniture }; ;
        }

        public FurnitureList(List<Furniture> furnitureList)
        {
            this.items = furnitureList;
        }

        public FurnitureList(FurnitureList furnitureList)
        {
            this.items = this.items.Concat(furnitureList.items).ToList();
        }

        public void Add(Furniture furniture)
        {
            if (!items.Contains(furniture))
            {
                items.Add(furniture);
            }
        }

        public void Add(FurnitureList furnitureList)
        {
            this.items = this.items.Concat(furnitureList.items).ToList();
        }

        public void Remove(Furniture furniture)
        {
            if (items.Contains(furniture))
            {
                items.Remove(furniture);
            }
        }

        public void Remove(FurnitureList furnitureList)
        {
            this.items.RemoveAll(furniture => furnitureList.Contains(furniture));
        }

        public bool Contains(Furniture furniture)
        {
            return items.Contains(furniture);
        }

        public FurnitureList FindFurnitureByRoom(Room room)
        {
            return new FurnitureList(items.FindAll(furniture => furniture.room == room));
        }
    }
}