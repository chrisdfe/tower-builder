using System;
using TowerBuilder.DataTypes;

namespace TowerBuilder.ApplicationState
{
    public delegate void ListEvent<ItemType>(ListWrapper<ItemType> list);
    public delegate void ItemEvent<ItemType>(ItemType item);

    [Serializable]
    public class ListStateSlice<ItemType> : StateSlice
    {
        public ListWrapper<ItemType> list { get; protected set; } = new ListWrapper<ItemType>();

        public delegate void ListEvent(ListWrapper<ItemType> list);
        public ListEvent onItemsAdded;
        public ListEvent onItemsRemoved;
        public ListEvent onItemsUpdated;

        public ListEvent onListUpdated;

        public delegate void ItemEvent(ItemType item);

        public ListStateSlice(AppState appState) : base(appState) { }

        public virtual void Add(ListWrapper<ItemType> newItemsList)
        {
            list.Add(newItemsList);

            onItemsAdded?.Invoke(newItemsList);
            onListUpdated?.Invoke(list);
        }

        public virtual void Add(ItemType item)
        {
            ListWrapper<ItemType> newItemsList = new ListWrapper<ItemType>();
            newItemsList.Add(item);

            list.Add(newItemsList);
            onItemsAdded?.Invoke(newItemsList);
            onListUpdated?.Invoke(list);
        }

        public virtual void Remove(ListWrapper<ItemType> removedItemsList)
        {
            list.Remove(removedItemsList);

            onItemsRemoved?.Invoke(removedItemsList);
            onListUpdated?.Invoke(list);
        }

        public virtual void Remove(ItemType item)
        {
            ListWrapper<ItemType> removedItemsList = new ListWrapper<ItemType>();
            removedItemsList.Add(item);

            list.Remove(removedItemsList);
            onItemsRemoved?.Invoke(removedItemsList);
            onListUpdated?.Invoke(list);
        }
    }
}
