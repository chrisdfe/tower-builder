using System;
using TowerBuilder.DataTypes;

namespace TowerBuilder.ApplicationState
{
    public delegate void ListEvent<ItemType>(ListWrapper<ItemType> list);
    public delegate void ItemEvent<ItemType>(ItemType item);

    public interface IListStateSlice<ItemType>
    {
        public ListWrapper<ItemType> list { get; }

        public void Add(ListWrapper<ItemType> newItemsList);

        public void Add(ItemType item);

        public void Remove(ListWrapper<ItemType> removedItemsList);

        public void Remove(ItemType item);
    }

    [Serializable]
    public class ListStateSlice<ItemType, EventsType> : StateSlice
        where EventsType : ListStateSlice<ItemType, EventsType>.Events, new()
    {
        public ListWrapper<ItemType> list { get; protected set; } = new ListWrapper<ItemType>();

        public class Events
        {
            public delegate void ListEvent(ListWrapper<ItemType> list);
            public ListEvent onItemsAdded;
            public ListEvent onItemsRemoved;
            public ListEvent onItemsUpdated;

            public ListEvent onListUpdated;

            public delegate void ItemEvent(ItemType item);
        }

        public virtual EventsType events { get; } = new EventsType();

        public ListStateSlice(AppState appState) : base(appState) { }

        public virtual void Add(ListWrapper<ItemType> newItemsList)
        {
            list.Add(newItemsList);

            events.onItemsAdded?.Invoke(newItemsList);
            events.onListUpdated?.Invoke(list);
        }

        public virtual void Add(ItemType item)
        {
            ListWrapper<ItemType> newItemsList = new ListWrapper<ItemType>();
            newItemsList.Add(item);
            Add(newItemsList);
        }

        public virtual void Remove(ListWrapper<ItemType> removedItemsList)
        {
            list.Remove(removedItemsList);
            events.onItemsRemoved?.Invoke(removedItemsList);
            events.onListUpdated?.Invoke(list);
        }

        public virtual void Remove(ItemType item)
        {
            ListWrapper<ItemType> removedItemsList = new ListWrapper<ItemType>();
            removedItemsList.Add(item);
            Remove(removedItemsList);
        }
    }
}
