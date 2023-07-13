using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace TowerBuilder.DataTypes
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ListWrapper<ItemType>
    {
        [JsonProperty]
        public List<ItemType> items { get; protected set; } = new List<ItemType>();

        public int Count => items.Count;

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
            if (item == null) return;

            if (!items.Contains(item))
            {
                items.Add(item);
            }
        }

        public void Add(List<ItemType> items)
        {
            foreach (ItemType item in items)
            {
                Add(item);
            }
        }

        public void Add(ListWrapper<ItemType> listWrapper)
        {
            Add(listWrapper.items);
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

        public virtual bool Contains(ItemType item)
        {
            return items.Contains(item);
        }

        public ItemType Find(Predicate<ItemType> predicate) => items.Find(predicate);

        public ListWrapper<ItemType> FindAll(Predicate<ItemType> predicate)
        {
            List<ItemType> result = items.FindAll(predicate);
            if (result != null)
            {
                return new ListWrapper<ItemType>(result);
            }

            return null;
        }

        public void ForEach(Action<ItemType> action)
        {
            items.ForEach(action);
        }

        public ListWrapper<ConvertedItemType> ConvertAll<ConvertedItemType>()
            where ConvertedItemType : class =>
            new ListWrapper<ConvertedItemType>(
                items.ConvertAll<ConvertedItemType>(item => item as ConvertedItemType)
            );
    }
}