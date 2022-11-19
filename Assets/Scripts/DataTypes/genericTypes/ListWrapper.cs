using System.Collections.Generic;
using System.Linq;

namespace TowerBuilder.DataTypes
{
    public class ListWrapper<ItemType>
    {
        public List<ItemType> items { get; private set; } = new List<ItemType>();

        public int Count { get { return items.Count; } }

        public ListWrapper() { }

        public ListWrapper(ItemType item)
        {
            this.items.Add(item);
        }

        public ListWrapper(List<ItemType> items)
        {
            this.items = items;
        }

        public ListWrapper(ListWrapper<ItemType> itemList)
        {
            this.items = this.items.Concat(itemList.items).ToList();
        }

        public void Add(ItemType item)
        {
            if (!items.Contains(item))
            {
                items.Add(item);
            }
        }

        public void Add(List<ItemType> items)
        {
            items = items.Concat(items).ToList();
        }

        public void Add(ListWrapper<ItemType> listWrapper)
        {
            items = listWrapper.items.Concat(items).ToList();
        }

        public void Remove(ItemType item)
        {
            items.Remove(item);
        }

        public void Remove(List<ItemType> itemsToRemove)
        {
            items.RemoveAll(item => itemsToRemove.Contains(item));
        }

        public void Remove(ListWrapper<ItemType> listWrapper)
        {
            items.RemoveAll(item => listWrapper.items.Contains(item));
        }

        public bool Contains(ItemType item)
        {
            return items.Contains(item);
        }
    }
}